using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Notifications;
using System.Collections.Concurrent;

namespace WinSize4
{
    public partial class frmMain : Form
    {
        // Delegate for the hook callback function
        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        // Import the SetWinEventHook and UnhookWinEvent functions from user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        // Constants for the events we want to listen for.
        private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        private const uint EVENT_OBJECT_HIDE = 0x8003;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        // A handle to our event hook
        private IntPtr _hhook;

        // Keep a reference to the delegate, otherwise it can be garbage collected
        private WinEventDelegate _winEventDelegate;

        private ConcurrentBag<IntPtr> _minimizedWindowHandles = new ConcurrentBag<IntPtr>();  // Store handles of minimized windows

        public ClsCurrentWindows _currentWindows = new ClsCurrentWindows();  // Windows open right now
        public ClsSavedWindows _savedWindows = new ClsSavedWindows();        // Windows saved on file
        public ClsScreens _screens = new ClsScreens();
        public ClsSettings _settings = new ClsSettings();                    // Global settings
        const int _hotKeyID = 1;
        //const int _hotKeyModifierLeft = 2;    //Alt = 1, Ctrl = 2, Shift = 4, Win(Windows key for opening the start menu) = 8
        //const int _hotKeyModifierRight = 4;   //Alt = 1, Ctrl = 2, Shift = 4, Win(Windows key for opening the start menu) = 8
        bool _ListBoxDoEvents = true;
        bool _closeWindow = false;
        bool _toastClicked = false;
        bool _dirty = false;
        bool isPausedUpdating;
        string _lastTitle = "";
        private bool allowVisible;     // ContextMenu's Show command used
        private bool _userHasTakenControlOfColumns = false; // This flag is true ONLY when the user has manually resized a column.
        private bool _isUserDraggingColumn = false; // A temporary flag to confirm a user drag is in progress.

        //**********************************************
        // Form initialize
        //**********************************************
        public frmMain()
        {
            InitializeComponent();
            _winEventDelegate = new WinEventDelegate(WinEventProc);
            _settings.InitializeAndLoad();
            string path = _settings.ActivePath; // Get the determined path
            ClsDebug.Initialize(path); // Initialize the logger first
            _savedWindows.Load(path);
            _screens.Load(path);
            _screens.AddNewScreens();
            _screens.SetPresent();
            _screens.Save(path);
            // picClearSearch.Visible = false; // Set the initial state of the search clear icon.
            //_savedWindows.Order();
            PopulateListBox();
            //txtVersion.Text = GetType().Assembly.GetName().Version.ToString();
            // Set version: Project -> WinSize4 Properties: Package - General - Assembly version / File version
            txtVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ClsDebug.ClearLog();
            ClsDebug.LogNow("\nScreens:");
            for (int i = 0; i < _screens.ScreenList.Count; i++)
            {
                ClsDebug.LogNow(" Screen " + i + " " + _screens.ScreenList[i].BoundsWidth + " " + _screens.ScreenList[i].BoundsHeight +
                    " Primary: " + _screens.ScreenList[i].Primary + " Present: " + _screens.ScreenList[i].Present);
            }
            ClsDebug.LogNow("\nSaved windows:");
            for (int i = 0; i < _savedWindows.Props.Count; i++)
            {
                ClsDebug.LogNow(" Window " + i + " " + _savedWindows.Props[i].Name + " " + _savedWindows.Props[i].Exe + " " + _savedWindows.Props[i].MonitorBoundsWidth +
                    " " + _savedWindows.Props[i].MonitorBoundsHeight + " " + _savedWindows.Props[i].Primary);
            }
            notifyIcon1.Text = _savedWindows.Props.Count.ToString() + " controlled windows";
            notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Show", null, this.butShow_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Pause", null, this.togglePause);
            notifyIcon1.ContextMenuStrip.Items.Add("Reset moved", null, this.butResetMoved_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, this.butExit_Click);
            cbHotKeyLeft.SelectedIndex = cbHotKeyLeft.FindString(_settings.HotKeyLeft);
            cbHotKeyLeft_SelectedIndexChanged(this, EventArgs.Empty);
            cbSearchTitleInclude_CheckedChanged(this, EventArgs.Empty);
            cbSearchTitleExclude_CheckedChanged(this, EventArgs.Empty);
            cbHotKeyRight.SelectedIndex = cbHotKeyRight.FindString(_settings.HotKeyRight);
            tbHotKeyCharacter.Text = _settings.HotKeyCharacter;
            cbShowAllWindows.Checked = _settings.showAllWindows;
            cbResetIfNewScreen.Checked = _settings.resetIfNewScreen;
            cbRunAtLogin.Checked = _settings.runAtLogin;
            chkPortableMode.Checked = _settings.PortableMode;
            cbIsPaused.Checked = _settings.isPaused;
            chkResetOnMinimize.Checked = _settings.ResetOnMinimize;

            #region ListView Column Initialization
            // Unsubscribe from events to prevent them from firing during the setup.
            this.listView1.Resize -= new System.EventHandler(this.listView1_Resize);
            this.listView1.ColumnWidthChanging -= new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
            this.listView1.ColumnWidthChanged -= new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);

            // ClsDebug.LogNow("[ColumnLayout] Starting ListView initialization in constructor.");
            // Part 1: Always set the desired visual order first. This is static.
            try
            {
                int totalColumns = listView1.Columns.Count;
                if (totalColumns > 3) // Ensure there are enough columns
                {
                    // Find the columns by their text or name
                    ColumnHeader nameCol = listView1.Columns.Cast<ColumnHeader>().FirstOrDefault(c => c.Text == "Name");
                    ColumnHeader widthCol = listView1.Columns.Cast<ColumnHeader>().FirstOrDefault(c => c.Text == "Width");
                    ColumnHeader heightCol = listView1.Columns.Cast<ColumnHeader>().FirstOrDefault(c => c.Text == "Height");
                    ColumnHeader primaryCol = listView1.Columns.Cast<ColumnHeader>().FirstOrDefault(c => c.Text == "Primary");

                    // The final visual order
                    if (nameCol != null) nameCol.DisplayIndex = 0;
                    if (widthCol != null) widthCol.DisplayIndex = 1;
                    if (heightCol != null) heightCol.DisplayIndex = 2;
                    if (primaryCol != null) primaryCol.DisplayIndex = 3;
                }
            }
            catch (Exception ex)
            {
                // ClsDebug.LogNow($"[ColumnLayout] ERROR setting display order: {ex.Message}");
            }
            // Part 2: Load settings OR apply the default auto-expanding layout.
            if (_settings.ListViewColumnWidths != null && _settings.ListViewColumnWidths.Count > 0)
            {
                _userHasTakenControlOfColumns = true; // Since using the user's saved layout, mark that they are in control.
            }
            else
            {
                // ClsDebug.LogNow("[ColumnLayout] No saved widths found. The app will be in control.");
                // --- SCENARIO B: FIRST RUN (NO SAVED SETTINGS) ---
                _userHasTakenControlOfColumns = false; // The application is in control of the layout.
            }
            // ClsDebug.LogNow($"[ColumnLayout] Constructor finished. _userHasTakenControlOfColumns = {_userHasTakenControlOfColumns}");
            #endregion ListView Column Initialization

            ClsDebug.AddText("\nStarting");
            ClsDebug.LogText();
            timer1.Interval = _settings.Interval;
            RegisterListener();
            //this.AddHandler(KeyDownEvent, new KeyEventHandler(KeyDown), true);
            ResetDetailsPanel();
        }

        //**********************************************
        // Set default widths for fixed-size columns
        //**********************************************
        private void SetDefaultFixedColumnWidths()
        {
            // The FIX for the oversized with Autosize columns: Calculate widths manually.
            foreach (ColumnHeader col in listView1.Columns)
            {
                if (col.Text == "Width")
                {
                    col.Width = TextRenderer.MeasureText("Width", listView1.Font).Width + 5; // Text width + padding
                    // ClsDebug.LogNow($"[ColumnLayout] -> Set '{col.Text}' column width to {col.Width}px.");
                }
                else if (col.Text == "Height")
                {
                    col.Width = TextRenderer.MeasureText("Height", listView1.Font).Width + 5;
                    // ClsDebug.LogNow($"[ColumnLayout] -> Set '{col.Text}' column width to {col.Width}px.");
                }
                else if (col.Text == "Primary")
                {
                    col.Width = TextRenderer.MeasureText("Primary", listView1.Font).Width;
                    // ClsDebug.LogNow($"[ColumnLayout] -> Set '{col.Text}' column width to {col.Width}px.");
                }
            }
        }

        //**********************************************
        // Auto-resize the 'Name' column to fill remaining space
        //**********************************************
        private void ResizeNameColumn()
        {
            // ClsDebug.LogNow("[ColumnLayout] ResizeNameColumn() called.");

            ColumnHeader nameCol = listView1.Columns.Cast<ColumnHeader>().FirstOrDefault(c => c.Text == "Name");
            if (nameCol == null)
            {
                // ClsDebug.LogNow("[ColumnLayout] -> Aborting resize because 'Name' column was not found.");
                return;
            }

            int allOtherColumnsWidth = 0;
            foreach (ColumnHeader col in listView1.Columns)
            {
                if (col != nameCol) allOtherColumnsWidth += col.Width;
            }
            // ClsDebug.LogNow($"[ColumnLayout] -> Calculated allOtherColumnsWidth: {allOtherColumnsWidth}px.");

            int availableWidth = listView1.ClientSize.Width - allOtherColumnsWidth;
            // ClsDebug.LogNow($"[ColumnLayout] -> listView.ClientSize.Width is {listView1.ClientSize.Width}px. Calculated availableWidth for 'Name': {availableWidth}px.");

            // Apply the new width, ensuring it's a reasonable value.
            if (availableWidth > 50) // e.g., don't shrink it to nothing
            {
                nameCol.Width = availableWidth;
                // ClsDebug.LogNow($"[ColumnLayout] -> SUCCESS: Set 'Name' column width to {availableWidth}px.");
            }
            // else
            // {
            //     ClsDebug.LogNow($"[ColumnLayout] -> SKIPPED setting width because availableWidth ({availableWidth}px) was too small.");
            // }
        }
        //**********************************************
        // Saving column layout if user has changed it
        //**********************************************
        private void SaveColumnLayout()
        {
            // ClsDebug.LogNow("[ColumnLayout] SaveColumnLayout() called.");
            // 
            // ClsDebug.LogNow("[ColumnLayout] -> Proceeding to save layout to settings object.");
            if (_settings.ListViewColumnWidths == null)
            {
                _settings.ListViewColumnWidths = new Dictionary<string, int>();
            }
            else
            {
                _settings.ListViewColumnWidths.Clear();
            }

            foreach (ColumnHeader col in listView1.Columns)
            {
                _settings.ListViewColumnWidths[col.Text] = col.Width;
                // ClsDebug.LogNow($"[ColumnLayout] -> Saving '{col.Text}' with width {col.Width}px.");
            }

            _settings.SaveToFile();
        }

        //**********************************************
        // Form load
        //**********************************************
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            // Start listening for window minimize and hide events across all applications
            _hhook = SetWinEventHook(EVENT_SYSTEM_MINIMIZESTART, EVENT_OBJECT_HIDE, IntPtr.Zero, _winEventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        //**********************************************
        // Detecting when a managed window is minimized or sent to the tray and reseting its Moved flag
        //**********************************************
        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // 1. Check if a checkbox 'chkResetOnMinimize' is ticked and only care about events for top-level windows.
            if (!chkResetOnMinimize.Checked || idObject != 0 || idChild != 0)
            {
                return;
            }

            try
            {
                // Determine if this is a window that should be managed.
                GetWindowThreadProcessId(hwnd, out uint processId);
                if (processId == 0) return;

                var process = System.Diagnostics.Process.GetProcessById((int)processId);
                if (process == null) return;

                // --- Start Debug Logging ---
                // ClsDebug.LogNow($"[DEBUG] WinEventProc: Event fired for process '{process.ProcessName}' (hwnd: {hwnd}).");

                // Gather more info for the search, just like in the timer.
                StringBuilder titleBuilder = new StringBuilder(256);
                GetWindowText(hwnd, titleBuilder, titleBuilder.Capacity);

                StringBuilder classBuilder = new StringBuilder(256);
                GetClassName(hwnd, classBuilder, classBuilder.Capacity);

                // Create a temporary object just for searching saved rules.
                var searchProps = new ClsWindowProps
                {
                    Exe = process.ProcessName,
                    Title = titleBuilder.ToString(),
                    WindowClass = classBuilder.ToString()
                };

                // ClsDebug.LogNow($"[DEBUG] WinEventProc: Searching for Title='{searchProps.Title}', Exe='{searchProps.Exe}'");

                // If a rule exists for this application, to reset its Moved flag.
                int savedIndex = _savedWindows.GetIndexAllScreens(searchProps, _screens.ScreenList, 0);

                // ClsDebug.LogNow($"[DEBUG] WinEventProc: GetIndexAllScreens returned index: {savedIndex}.");

                // If a configuration exists for this window, add its handle to our list for processing later.
                if (savedIndex > -1)
                {
                    _minimizedWindowHandles.Add(hwnd);
                    //    ClsDebug.LogNow($"[DEBUG] WinEventProc: SUCCESS! Added hwnd {hwnd} to the list.");
                }
            }
            catch (Exception ex)
            {
                // Ignore errors, e.g., if the process closes.
                // ClsDebug.LogNow($"[DEBUG] WinEventProc: An error occurred: {ex.Message}");
            }
        }

        //**********************************************
        /// <summary>
        /// Used by the hot key hook
        /// http://frasergreenroyd.com/c-global-keyboard-listeners-implementation-of-key-hooks/
        /// </summary>
        /// <param name="m"></param>
        //**********************************************
        protected override void WndProc(ref Message m)
        {
            try
            {
                // Stop the timer while we handle a hotkey click
                timer1.Stop();
                bool newWindow = false;
                if (m.Msg == 0x0312 && m.WParam.ToInt32() == _hotKeyID)
                {
                    long hWnd = (long)GetForegroundWindow();
                    int focusIndex = _currentWindows.GetCurrentWindowsIndexForhWnd(hWnd);
                    bool Changed = _screens.AddNewScreens();
                    if (_settings.resetIfNewScreen && Changed)
                        _currentWindows.ResetMoved();

                    // Update the existing window in _currentWindows
                    _currentWindows.UpdateWindowProperties(focusIndex);
                    _currentWindows.Windows[focusIndex].Moved = false;
                    int Tag;
                    int screenIndex = _screens.GetScreenIndexForWindow(_currentWindows.Windows[focusIndex].Props);
                    if (!_savedWindows.UpdateWindowProperties(_currentWindows.Windows[focusIndex].Props, _screens.ScreenList, screenIndex))
                    {
                        newWindow = true;
                        Tag = _savedWindows.AddWindow(_currentWindows.Windows[focusIndex].Props);
                    }
                    PopulateListBox();
                    int savedWindowsIndex = _savedWindows.GetIndexCurrentScreen(_currentWindows.Windows[focusIndex].Props, _screens.ScreenList, screenIndex);
                    int Index = GetListViewIndexByTag(_savedWindows.Props[savedWindowsIndex].Tag);
                    listView1.Items[Index].Selected = true;
                    listView1.Select();

                    ClsWindowProps Props = _currentWindows.GetWindowProperties(hWnd);
                    string newWindowText = newWindow == true ? "WinSize4: Adding as a new window" : "WinSize4: Updating an existing window";
                    new ToastContentBuilder()
                        //.AddHeader("WinSize4", "WinSize4", "action=openConversation&id=WinSize4")
                        .AddText(newWindowText)
                        .AddText("Title: " + Props.Title)
                        .AddText(_savedWindows.Props[savedWindowsIndex].Name)
                        .SetBackgroundActivation()
                        .Show();
                    newWindowText += "\nTitle: " + _savedWindows.Props[savedWindowsIndex].TitleInclude;

                    ToastNotificationManagerCompat.OnActivated += toastArgs =>
                    {
                        _toastClicked = true;
                        allowVisible = true;
                        this.Invoke((MethodInvoker)delegate
                        {
                            Show();
                        });
                        //ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                    };

                    ClsDebug.LogToEvent(new Exception(), EventLogEntryType.Information, newWindowText);
                }
                base.WndProc(ref m);
            }
            catch
            (Exception ex)
            {
                ClsDebug.LogToEvent(ex, EventLogEntryType.Error, "");
            }
            finally
            {
                timer1.Start();
            }
        }
        //**********************************************
        // timer1 click
        //**********************************************
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                allowVisible = true;
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                //ClsDebug._text = "";
                //ClsDebug.AddText("Timer1_Tick starting");
                timer1.Stop();
                long hWnd = (long)GetForegroundWindow();

                // Process any windows that were minimized since the last tick.
                while (_minimizedWindowHandles.TryTake(out IntPtr hwndToReset))
                {
                    // Find the window with this handle
                    var windowToUpdate = _currentWindows.Windows.FirstOrDefault(w => w.hWnd == (long)hwndToReset);
                    if (windowToUpdate != null)
                    {
                        windowToUpdate.Moved = false;
                    }
                }

                if (_settings.isPaused)
                {
                    ClsDebug.AddText("WinSize4 is paused");
                }
                if (hWnd > 0 && _settings.isPaused == false)
                {
                    bool NewScreen = _screens.AddNewScreens();
                    bool ChangedScreens = _screens.SetPresent();
                    if (_settings.resetIfNewScreen && (NewScreen || ChangedScreens))
                    {
                        _currentWindows.ResetMoved();
                        PopulateListBox();
                    }
                    ClsWindowProps currentWindowProps = _currentWindows.GetWindowProperties(hWnd);
                    int currentScreenIndex = _screens.GetScreenIndexForWindow(currentWindowProps);
                    int targetSavedWindowsIndex = _savedWindows.GetIndexAllScreens(currentWindowProps, _screens.ScreenList, currentScreenIndex);
                    int currentWindowsIndex = _currentWindows.GetCurrentWindowsIndexForhWnd(hWnd);
                    string Text = _currentWindows.Windows[currentWindowsIndex].hWnd + " " +
                        _currentWindows.Windows[currentWindowsIndex].Pid + " " +
                        _currentWindows.Windows[currentWindowsIndex].Props.Name + " " +
                        _currentWindows.Windows[currentWindowsIndex].Props.Exe + " " +
                        _currentWindows.Windows[currentWindowsIndex].Props.WindowClass;
                    if (currentWindowProps.Title != _lastTitle)
                    {
                        ClsDebug.LogNow("Checking window: " + Text);

                        // After logging the window we are checking, we immediately check if we found a rule for it.
                        if (targetSavedWindowsIndex > -1)
                        {
                            // If a rule was found, log its name.
                            var matchingRule = _savedWindows.Props[targetSavedWindowsIndex];
                            ClsDebug.LogNow($"-> Matching rule found: '{matchingRule.Name}'");
                        }
                        else
                        {
                            // If no rule was found, log that too. This is very useful for debugging.
                            ClsDebug.LogNow("-> No matching rule found.");
                        }
                    }
                    if (targetSavedWindowsIndex > -1 &&
                        currentWindowProps.Title != "" &&
                        currentWindowProps.Title != _lastTitle &&
                        (_currentWindows.Windows[currentWindowsIndex].Moved == false || _savedWindows.Props[targetSavedWindowsIndex].AlwaysMove == true)
                        && !_savedWindows.Props[targetSavedWindowsIndex].Disabled)
                    {
                        int targetScreenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[targetSavedWindowsIndex]);
                        //bool targetWindowHasParent = ((int)GetParent((IntPtr)hWnd) > 0);
                        bool targetWindowHasParent = ((int)GetWindow((IntPtr)hWnd, 4) > 0);
                        // Only continue if window is not a child window to a current window, or child windows should be considered
                        if (!_savedWindows.Props[targetSavedWindowsIndex].IgnoreChildWindows || !targetWindowHasParent)
                        {
                            // For clarity, get the target properties from your saved settings.
                            var targetProps = _savedWindows.Props[targetSavedWindowsIndex];
                            //ClsDebug.LogNow($"[ColumnLayout] -> Set '{col.Text}' column width to {col.Width}px.");
                            ClsDebug.AddText($"[Compare window's parameters]: currentWindowProps.Left:'{currentWindowProps.Left}' - targetProps.Left:'{targetProps.Left}'");
                            ClsDebug.AddText($"[Compare window's parameters]: currentWindowProps.Left:'{currentWindowProps.Top}' - targetProps.Left:'{targetProps.Top}'");
                            ClsDebug.AddText($"[Compare window's parameters]: currentWindowProps.Left:'{currentWindowProps.Width}' - targetProps.Left:'{targetProps.Width}'");
                            ClsDebug.AddText($"[Compare window's parameters]: currentWindowProps.Left:'{currentWindowProps.Height}' - targetProps.Left:'{targetProps.Height}'");
                            // Compare the current window's geometry with the saved target geometry.
                            // Also ensure that the 'AlwaysMove' flag will bypass this check.
                            if (currentWindowProps.Left == targetProps.Left &&
                                currentWindowProps.Top == targetProps.Top &&
                                currentWindowProps.Width == targetProps.Width &&
                                currentWindowProps.Height == targetProps.Height)
                            //!targetProps.AlwaysMove)
                            {
                                // The window is already in the correct position.
                                // Skip the move, but MUST set the 'Moved' flag to true
                                // to prevent the app from checking it again on the next tick.
                                ClsDebug.AddText("Window is already in the correct position. Skipping move.");
                                ClsDebug.LogText();
                                _currentWindows.Windows[currentWindowsIndex].Moved = true;
                            }
                            else
                            {
                                // The window is not in the correct position, so proceed with the move.
                                ClsDebug.AddText("Moving window: " + Text);

                                _currentWindows.MoveCurrentWindow(currentWindowsIndex,
                                    targetProps,
                                    targetSavedWindowsIndex,
                                    _screens.ScreenList[targetScreenIndex],
                                    new ClsScreenList());
                                _currentWindows.UpdateWindowProperties(currentWindowsIndex);
                                _currentWindows.Windows[currentWindowsIndex].Moved = true;
                                ClsDebug.AddText($"Execution Time: {watch.ElapsedMilliseconds} ms");
                                watch.Stop();
                                ClsDebug.LogText();
                            }
                        }
                    }
                    _lastTitle = currentWindowProps.Title;
                }
            }
            catch
            (Exception ex)
            {
                ClsDebug.LogToEvent(ex, EventLogEntryType.Error, "");
            }
            finally
            {
                timer1.Start();
                _toastClicked = false;
            }
        }

        //**********************************************
        // Populate listbox with saved windows
        //**********************************************
        private void PopulateListBox(string filter = "")
        {
            //ClsDebug.LogNow("[PopulateListBox] PopulateListBox: Starting.");

            try
            {
                // Store the selected item's tag so we can re-select it after filtering.
                int lastSelectedTag = -1;
                if (listView1.SelectedItems.Count > 0)
                {
                    lastSelectedTag = listView1.SelectedItems[0].Tag as int? ?? -1;
                }

                listView1.Items.Clear();
                //ClsDebug.LogNow("[PopulateListBox] PopulateListBox: Items cleared.");

                // Prepare the filter for case-insensitive comparison.
                // An empty filter means "show everything".
                bool isFiltering = !string.IsNullOrWhiteSpace(filter);
                string lowerCaseFilter = isFiltering ? filter.ToLowerInvariant() : "";

                string[] row;
                for (int i = 0; i < _savedWindows.Props.Count; i++)
                {
                    var prop = _savedWindows.Props[i];

                    // --- NEW SEARCH LOGIC ---
                    // If we are filtering, check if this item should be included.
                    if (isFiltering)
                    {
                        // Combine all searchable text into one string for easy checking.
                        // This covers all visible columns and other useful properties.
                        var searchableText = string.Join("|",
                            prop.Name,
                            prop.Exe,
                            prop.WindowClass,
                            prop.TitleInclude,
                            prop.MonitorBoundsWidth.ToString(),
                            prop.MonitorBoundsHeight.ToString(),
                            prop.Primary ? "Yes" : "No",
                            prop.Left.ToString(),
                            prop.Top.ToString(),
                            prop.Width.ToString(),
                            prop.Height.ToString()
                        ).ToLowerInvariant();

                        // If the combined text doesn't contain the filter, skip to the next item.
                        if (!searchableText.Contains(lowerCaseFilter))
                        {
                            continue; // Skip this item
                        }
                    }

                    // --- The rest of the method is the same as before ---
                    string Primary = prop.Primary ? "Yes" : "No";
                    row = new string[] { prop.Name, prop.MonitorBoundsWidth.ToString(), prop.MonitorBoundsHeight.ToString(), Primary };
                    var listViewItem = new ListViewItem(row) { Tag = prop.Tag };

                    // Set the checkbox state from the saved property
                    listViewItem.StateImageIndex = prop.Disabled ? 1 : 0;

                    int screenIndex = _screens.GetScreenIndexForWindow(prop);
                    if (screenIndex == -1)
                    {
                        listView1.Items.Add(listViewItem);
                    }
                    else if (!_screens.ScreenList[screenIndex].Present)
                    {
                        if (_settings.showAllWindows)
                        {
                            listViewItem.ForeColor = Color.Silver;
                            listView1.Items.Add(listViewItem);
                        }
                    }
                    else
                    {
                        listView1.Items.Add(listViewItem);
                    }
                }
                //ClsDebug.LogNow($"[PopulateListBox] PopulateListBox: Finished. Total items in list: {listView1.Items.Count}.");

                // Try to re-select the previously selected item if it's still in the list.
                if (lastSelectedTag != -1)
                {
                    int indexToSelect = GetListViewIndexByTag(lastSelectedTag);
                    if (indexToSelect > -1)
                    {
                        listView1.Items[indexToSelect].Selected = true;
                        listView1.Items[indexToSelect].Focused = true;
                        listView1.EnsureVisible(indexToSelect);
                    }
                }
            }
            catch (Exception ex)
            {
                ClsDebug.LogNow($"[DrawDebug] PopulateListBox: CRITICAL ERROR! {ex.Message}");
            }
        }
        //**********************************************
        // Listbox selection changed
        //**********************************************
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ListBoxDoEvents)
            {
                // If an item is selected...
                if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Tag != null)
                {
                    // ...call the method to load its details.
                    LoadDetailsForItem(listView1.SelectedItems[0]);
                    //ClsDebug.LogNow($"[SelectedIndexChanged] Item selected. Details panel updated. listView1.SelectedItems[0] '{listView1.SelectedItems[0]}'");
                }
            }
        }

        //**********************************************
        /// <summary> Gets Index in ListView1 for Tag </summary>
        /// <returns>int Index</returns>
        //**********************************************
        public int GetListViewIndexByTag(int Tag)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (Tag == (int)listView1.Items[i].Tag)
                {
                    return i;
                }
            }
            return -1;
        }

        //**********************************************
        // Save values
        //**********************************************
        private void SaveValuesForIndex(int index)
        {
            if (index > -1)
            {
                _savedWindows.Props[index].Name = tbName.Text;
                _savedWindows.Props[index].ConsiderWindowClass = cbWindowClass.Checked;
                _savedWindows.Props[index].WindowClass = tbWindowClass.Text;
                _savedWindows.Props[index].TitleInclude = tbTitleInclude.Text;
                _savedWindows.Props[index].TitleExclude = tbTitleExclude.Text;
                _savedWindows.Props[index].SearchTitleInclude = cbSearchTitleInclude.Checked;
                _savedWindows.Props[index].SearchTitleExclude = cbSearchTitleExclude.Checked;
                _savedWindows.Props[index].Exe = tbExe.Text;
                _savedWindows.Props[index].SearchExe = cbSearchExe.Checked;
                _savedWindows.Props[index].IgnoreChildWindows = cbIgnoreChildWindows.Checked;
                _savedWindows.Props[index].AlwaysMove = cbAlwaysMove.Checked;
                _savedWindows.Props[index].CanResize = cbCanResize.Checked;
                if (!cbCustomWidth.Checked)
                {
                    int.TryParse(tbWidth.Text, out int width);
                    _savedWindows.Props[index].Width = width;

                    int.TryParse(tbLeft.Text, out int left);
                    _savedWindows.Props[index].Left = left;
                }
                if (!cbCustomHeight.Checked)
                {
                    int.TryParse(tbHeight.Text, out int height);
                    _savedWindows.Props[index].Height = height;

                    int.TryParse(tbTop.Text, out int top);
                    _savedWindows.Props[index].Top = top;
                }
                _savedWindows.Props[index].MaxWidth = cbCustomWidth.Checked;
                _savedWindows.Props[index].MaxHeight = cbCustomHeight.Checked;
                _savedWindows.Props[index].FullScreen = cbFullScreen.Checked;

                if (radioFullInclude.Checked)
                    _savedWindows.Props[index].SearchTypeInclude = ClsWindowProps.Full;
                if (radioContainsInclude.Checked)
                    _savedWindows.Props[index].SearchTypeInclude = ClsWindowProps.Contains;
                if (radioStartsWithInclude.Checked)
                    _savedWindows.Props[index].SearchTypeInclude = ClsWindowProps.StartsWith;
                if (radioFullExclude.Checked)
                    _savedWindows.Props[index].SearchTypeExclude = ClsWindowProps.Full;
                if (radioContainsExclude.Checked)
                    _savedWindows.Props[index].SearchTypeExclude = ClsWindowProps.Contains;
                if (radioStartsWithExclude.Checked)
                    _savedWindows.Props[index].SearchTypeExclude = ClsWindowProps.StartsWith;
            }
        }

        //**********************************************
        // Register the hot key listener
        //**********************************************
        private void RegisterListener()
        {
            UnregisterListener();
            bool result = RegisterHotKey(this.Handle, _hotKeyID, _settings.GetHotKeyNumber((string)cbHotKeyLeft.SelectedItem)
                + _settings.GetHotKeyNumber((string)cbHotKeyRight.SelectedItem), (int)char.Parse(_settings.HotKeyCharacter));
            if (!result)
            {
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("Could not register hot key listener")
                    //.AddText(_savedWindows.Props[_savedWindows.Props.Count - 1].TitleInclude)
                    .Show();
            }
        }

        //**********************************************
        // Unregister the hot key listener
        //**********************************************
        private void UnregisterListener()
        {
            UnregisterHotKey(this.Handle, _hotKeyID);
        }

        //**********************************************
        // Handles keys pressed
        //**********************************************
        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                butRemove_Click(this, EventArgs.Empty);
            }
        }

        //**********************************************
        // Sets startup at login
        //**********************************************
        private void SetStartup()
        {
            _settings.runAtLogin = cbRunAtLogin.Checked;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (cbRunAtLogin.Checked)
                rk.SetValue("WinSize4", Application.ExecutablePath);
            else
                rk.DeleteValue("WinSize4", false);
        }

        //**********************************************
        // Event handlers
        //**********************************************

        //**********************************************
        // Edit screens
        //**********************************************
        private void butEditScreens_Click(object sender, EventArgs e)
        {
            List<ClsScreenList> screenList = new List<ClsScreenList>(_screens.ScreenList.Count);
            int Index = 0;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            _screens.ScreenList.ForEach((item) =>
            {
                screenList.Add((ClsScreenList)item.Clone());
            });
            ClsWindowProps Win = new ClsWindowProps();
            if (_savedWindows.Props.Count > 0)
                Win = _savedWindows.Props[Index];
            frmScreens Scr = new frmScreens(screenList, Win.MonitorBoundsWidth, Win.MonitorBoundsHeight, Win.Primary);
            var result = Scr.ShowDialog();
            if (result == DialogResult.OK)
            {
                _screens.ScreenList = Scr.ReturnScreenList;
                _screens.Save(_settings.ActivePath);
            }
        }

        //**********************************************
        // OK
        //**********************************************
        private void butOK_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
            }

            bool newMode = chkPortableMode.Checked;
            if (newMode != _settings.PortableMode)
            {
                _settings.PortableMode = newMode;
                _settings.UpdateSettingsLocation();
            }

            _savedWindows.Save(_settings.ActivePath);
            _screens.Save(_settings.ActivePath);
            _settings.SaveToFile();

            RegisterListener();
            _currentWindows.ResetMoved();
            this.Hide();
        }

        //**********************************************
        // Exit
        //**********************************************
        private void butExit_Click(object sender, EventArgs e)
        {
            _closeWindow = true;
            this.Close();
        }

        //**********************************************
        // Cancel
        //**********************************************
        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //**********************************************
        // Apply
        //**********************************************
        private void butApply_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0 && listView1.SelectedItems.Count > 0)
            {
                SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
            }

            bool newMode = chkPortableMode.Checked;
            if (newMode != _settings.PortableMode)
            {
                _settings.PortableMode = newMode;
                _settings.UpdateSettingsLocation();
            }

            _savedWindows.Save(_settings.ActivePath);
            _screens.Save(_settings.ActivePath);
            _settings.SaveToFile();

            RegisterListener();
            _currentWindows.ResetMoved();
            PopulateListBox(); // Repopulate to reflect any changes
        }

        //**********************************************
        // Pause
        //**********************************************
        private void togglePause(object sender, EventArgs e)
        {
            if (isPausedUpdating == false)
            {
                isPausedUpdating = true;
                if (((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked)
                {
                    _settings.isPaused = false;
                    ((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked = false;
                    cbIsPaused.Checked = false;
                }
                else
                {
                    _settings.isPaused = true;
                    ((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked = true;
                    cbIsPaused.Checked = true;
                }
                _dirty = true;
                isPausedUpdating = false;
            }
        }

        private void cbIsPaused_CheckedChanged(object sender, EventArgs e)
        {
            if (isPausedUpdating == false)
            {
                isPausedUpdating = true;
                if (((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked)
                {
                    _settings.isPaused = false;
                    ((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked = false;
                    cbIsPaused.Checked = false;
                }
                else
                {
                    _settings.isPaused = true;
                    ((ToolStripMenuItem)notifyIcon1.ContextMenuStrip.Items[1]).Checked = true;
                    cbIsPaused.Checked = true;
                }
                _dirty = true;
                isPausedUpdating = false;
            }
        }

        private void tbLeft_TextChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbLeft.Text, out int number))
                _dirty = true;
        }

        private void tbTop_TextChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbTop.Text, out int number))
                _dirty = true;
        }

        private void tbWidth_TextChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbWidth.Text, out int number))
                _dirty = true;
        }

        private void tbHeight_TextChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbHeight.Text, out int number))
                _dirty = true;
        }

        private void cbCustomWidth_CheckedChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomWidth.Checked || cbFullScreen.Checked)
            {
                int screenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[Index]);
                tbWidth.Text = _screens.ScreenList[screenIndex].CustomWidth.ToString();
                tbLeft.Text = "0";
                tbWidth.Enabled = false;
                tbLeft.Enabled = false;
            }
            else
            {
                tbWidth.Text = _savedWindows.Props[Index].Width.ToString();
                tbLeft.Text = _savedWindows.Props[Index].Left.ToString();
                tbWidth.Enabled = true;
                tbLeft.Enabled = true;
            }
            _dirty = true;
        }

        private void cbCustomHeight_CheckedChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomHeight.Checked || cbFullScreen.Checked)
            {
                int screenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[Index]);
                tbHeight.Text = _screens.ScreenList[screenIndex].CustomHeight.ToString();
                tbTop.Text = "0";
                tbHeight.Enabled = false;
                tbTop.Enabled = false;
            }
            else
            {
                tbHeight.Text = _savedWindows.Props[Index].Height.ToString();
                tbTop.Text = _savedWindows.Props[Index].Top.ToString();
                tbHeight.Enabled = true;
                tbTop.Enabled = true;
            }
            _dirty = true;
        }

        private void cbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            // If no item is selected, do not proceed.
            if (listView1.SelectedItems.Count == 0) return;

            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            tbWidth.Enabled = !cbFullScreen.Checked;
            tbHeight.Enabled = !cbFullScreen.Checked;
            tbTop.Enabled = !cbFullScreen.Checked;
            tbLeft.Enabled = !cbFullScreen.Checked;
            cbCustomWidth.Enabled = !cbFullScreen.Checked;
            cbCustomHeight.Enabled = !cbFullScreen.Checked;
            if (cbFullScreen.Checked)
            {
                tbWidth.Text = "0";
                tbLeft.Text = "0";
                tbHeight.Text = "0";
                tbTop.Text = "0";
                tbWidth.Enabled = false;
                tbLeft.Enabled = false;
                tbHeight.Enabled = false;
                tbTop.Enabled = false;
            }
            else
            {
                tbWidth.Text = _savedWindows.Props[Index].Width.ToString();
                tbLeft.Text = _savedWindows.Props[Index].Left.ToString();
                tbHeight.Text = _savedWindows.Props[Index].Height.ToString();
                tbTop.Text = _savedWindows.Props[Index].Top.ToString();
                tbWidth.Enabled = true;
                tbLeft.Enabled = true;
                tbHeight.Enabled = true;
                tbTop.Enabled = true;
            }
            cbCustomWidth_CheckedChanged(this, EventArgs.Empty);
            cbCustomHeight_CheckedChanged(this, EventArgs.Empty);
            _dirty = true;
        }

        private void cbSearchTitleInclude_CheckedChanged(object sender, EventArgs e)
        {
            int Index;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            else
                Index = 0;
            tbTitleInclude.Enabled = cbSearchTitleInclude.Checked;
            radioFullInclude.Enabled = cbSearchTitleInclude.Checked;
            radioContainsInclude.Enabled = cbSearchTitleInclude.Checked;
            radioStartsWithInclude.Enabled = cbSearchTitleInclude.Checked;
            if (!cbSearchTitleInclude.Checked)
                cbSearchExe.Checked = true;
            _dirty = true;
        }

        private void cbSearchTitleExclude_CheckedChanged(object sender, EventArgs e)
        {
            int Index;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            else
                Index = 0;
            tbTitleExclude.Enabled = cbSearchTitleExclude.Checked;
            radioFullExclude.Enabled = cbSearchTitleExclude.Checked;
            radioContainsExclude.Enabled = cbSearchTitleExclude.Checked;
            radioStartsWithExclude.Enabled = cbSearchTitleExclude.Checked;
            if (!cbSearchTitleExclude.Checked)
                cbSearchExe.Checked = true;
            _dirty = true;
        }

        private void tbTitleInclude_TextChanged(object sender, EventArgs e)
        {
            radioContainsInclude.Checked = true;
        }

        private void tbTitleExclude_TextChanged(object sender, EventArgs e)
        {
            radioContainsExclude.Checked = true;
        }

        private void cbSearchExe_CheckedChanged(object sender, EventArgs e)
        {
            int Index;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            else
                Index = 0;
            //_savedWindows.Props[Index].SearchExe = cbSearchExe.Checked;
            tbExe.Enabled = cbSearchExe.Checked;
            if (!cbSearchExe.Checked)
                cbSearchTitleInclude.Checked = true;
            _dirty = true;
        }

        private void butResetMoved_Click(object sender, EventArgs e)
        {
            _currentWindows.ResetMoved();
        }

        private void butShow_Click(object sender, EventArgs e)
        {
            allowVisible = true;

            // 1. Ensure the form is visible and not minimized.
            this.Show();
            this.WindowState = FormWindowState.Normal;

            // 2. The "TopMost" trick to steal focus.
            this.TopMost = true;  // Temporarily bring it to the very top.
            this.TopMost = false; // Immediately set it back to a normal window.

            // 3. Explicitly activate and focus the form.
            this.Activate();
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                if (listView1.SelectedItems[0].Index > -1)
                {
                    _savedWindows.Props.RemoveAt(Index);
                    PopulateListBox(txtSearch.Text);
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[0].Selected = true;
                        //listView1_SelectedIndexChanged(this, EventArgs.Empty);
                    }
                    _dirty = true;
                }
            }
        }

        private void butDuplicate_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                _savedWindows.DuplicateWindow(Index);
                PopulateListBox(txtSearch.Text);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                butRemove_Click(sender, e);
            }
        }

        private void cbHotKeyLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbHotKeyRight.Items.Clear();
            cbHotKeyRight.Items.Add("None");
            if (cbHotKeyLeft.SelectedIndex == 0)
            {
                cbHotKeyRight.Items.Add("Ctrl");
                cbHotKeyRight.Items.Add("Shift");
            }
            if (cbHotKeyLeft.SelectedIndex == 1)
            {
                cbHotKeyRight.Items.Add("Alt");
                cbHotKeyRight.Items.Add("Shift");
            }
            if (cbHotKeyLeft.SelectedIndex == 2)
            {
                cbHotKeyRight.Items.Add("Alt");
                cbHotKeyRight.Items.Add("Ctrl");
            }
            _settings.HotKeyLeft = cbHotKeyLeft.SelectedItem.ToString();
            int Index = cbHotKeyRight.FindString(_settings.HotKeyRight);
            if (Index > -1)
                cbHotKeyRight.SelectedIndex = Index;
            else
                cbHotKeyRight.SelectedIndex = 0;
            _dirty = true;
        }

        private void cbHotKeyRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.HotKeyRight = cbHotKeyRight.SelectedItem.ToString();
            _dirty = true;
        }

        private void tbHotKeyCharacter_TextChanged(object sender, EventArgs e)
        {
            if (tbHotKeyCharacter.Text is string)
                _settings.HotKeyCharacter = tbHotKeyCharacter.Text;
            _dirty = true;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // 1. Ensure the form is visible and not minimized.
            this.Show();
            this.WindowState = FormWindowState.Normal;

            // 2. The "TopMost" trick to steal focus.
            this.TopMost = true;  // Temporarily bring it to the very top.
            this.TopMost = false; // Immediately set it back to a normal window.

            // 3. Explicitly activate and focus the form.
            this.Activate();
        }

        private void cbShowAllWindows_CheckedChanged(object sender, EventArgs e)
        {
            _settings.showAllWindows = cbShowAllWindows.Checked;
            PopulateListBox(txtSearch.Text);
        }

        private void cbResetIfNewScreen_CheckedChanged(object sender, EventArgs e)
        {
            _settings.resetIfNewScreen = cbResetIfNewScreen.Checked;
        }

        private void cbCanResize_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCanResize.Checked)
            {
                tbWidth.Enabled = true;
                tbHeight.Enabled = true;
                cbCustomWidth.Enabled = true;
                cbCustomHeight.Enabled = true;
            }
            else
            {
                tbWidth.Enabled = false;
                tbHeight.Enabled = false;
                cbCustomWidth.Enabled = false;
                cbCustomHeight.Enabled = false;
            }
        }

        private void cbWindowClass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWindowClass.Checked)
            {
                tbWindowClass.Enabled = true;
            }
            else
            {
                tbWindowClass.Enabled = false;
            }
        }

        //**********************************************
        // Form closing
        //**********************************************
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_closeWindow)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                // Stop listening when the application is closing
                UnhookWinEvent(_hhook);

                _savedWindows.Save(_settings.ActivePath);
                _screens.Save(_settings.ActivePath);
                _settings.SaveToFile();
            }
        }

        private void cbRunAtLogin_CheckedChanged(object sender, EventArgs e)
        {
            SetStartup();
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        private void checkBox_MouseHover(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name == "cbFullScreen")
                toolTip1.SetToolTip(cbFullScreen, "Size the selected window to fullscreen");
            if (((CheckBox)sender).Name == "cbCustomWidth")
                toolTip1.SetToolTip(cbCustomWidth, "Size the selected window to the custom width");
            if (((CheckBox)sender).Name == "cbCustomHeight")
                toolTip1.SetToolTip(cbCustomHeight, "Size the selected window to the custom height");
            if (((CheckBox)sender).Name == "cbResetIfNewScreen")
                toolTip1.SetToolTip(cbResetIfNewScreen, "Resize all windows if a new screen is added or removed");
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = listView1.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
                //return; // Add a return to prevent right-click from doing anything else.
            }

            // Get detailed hit-test information about where the user clicked.
            var hitTestInfo = listView1.HitTest(e.X, e.Y);
            //ClsDebug.LogNow($"[listView1_MouseClick] Mouse Click at X:{e.X}, Y:{e.Y}");

            // Check if the click was specifically on the state image area.
            if (hitTestInfo.Location == ListViewHitTestLocations.StateImage)
            {
                // Knowing for sure the user clicked the icon, so we can get the item.
                var lvi = hitTestInfo.Item;

                try
                {
                    int tag = (int)lvi.Tag;
                    int savedIndex = _savedWindows.GetWindowIndexByTag(tag);

                    if (savedIndex != -1)
                    {
                        // 1. Toggle the "Disabled" property in the data source.
                        _savedWindows.Props[savedIndex].Disabled = !_savedWindows.Props[savedIndex].Disabled;

                        // 2. Update the image to reflect the new state.
                        lvi.StateImageIndex = _savedWindows.Props[savedIndex].Disabled ? 1 : 0;

                        _dirty = true; // Mark that there are unsaved changes.
                    }
                }
                catch (Exception ex)
                {
                    ClsDebug.LogToEvent(ex, EventLogEntryType.Error, "Error in listView1_MouseClick");
                }
            }
        }

        //**********************************************
        // API calls
        //**********************************************

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern short VkKeyScan(char ch);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        // This event fires after any resize, but it has a way to know if it was a user.
        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            // Only act if the "authenticator" flag is true. This ignores all programmatic changes.
            if (_isUserDraggingColumn)
            {
                _userHasTakenControlOfColumns = true; // 1. The user is now permanently in control.
                SaveColumnLayout();  // 2. Immediately save the new layout.
                _isUserDraggingColumn = false; // 3. Reset the authenticator flag, ready for the next drag.
            }
        }

        private void chkResetOnMinimize_CheckedChanged(object sender, EventArgs e)
        {
            _settings.ResetOnMinimize = chkResetOnMinimize.Checked;
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            // Only auto-resize if the user has not yet taken control.
            if (!_userHasTakenControlOfColumns)
            {
                // Schedule the expansion of the 'Name' column to happen AFTER the resize is finished.
                // This gives the control time to settle and avoids the rendering bug.
                ResizeNameColumn();
            }
        }

        // This is the "Authenticator" event. It ONLY fires when a user is dragging.
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // This event ONLY fires from a user drag. Set the flag to true.
            _isUserDraggingColumn = true;
        }

        private void btnResetColumns_Click(object sender, EventArgs e)
        {
            // 1. Unsubscribe from events to make the operation atomic ---
            // This prevents the ListView from entering a chaotic state while we work.
            this.listView1.Resize -= new System.EventHandler(this.listView1_Resize);
            this.listView1.ColumnWidthChanged -= new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);

            // 2. Reset the saved settings data.
            // Check if the settings exist, clear the dictionary, and immediately save the file.
            // This ensures that on the next launch, the app will perform the "first run" setup.
            if (_settings.ListViewColumnWidths != null && _settings.ListViewColumnWidths.Count > 0)
            {
                _settings.ListViewColumnWidths.Clear();
                _settings.SaveToFile();
            }

            // 3. Immediately re-apply the initial "first run" visual layout.
            // This gives the user instant visual feedback.
            SetDefaultFixedColumnWidths();
            ResizeNameColumn();

            // 4. Reset the application's state flag.
            // This is crucial to re-enable the auto-resizing of the 'Name' column.
            _userHasTakenControlOfColumns = false;

            // Re-subscribe here, after everything is stable.
            this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
            this.listView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            // This event fires after the form is fully stable and visible.

            // --- Perform the full, stable layout process ---

            if (_userHasTakenControlOfColumns)
            {
                // If the user has saved preferences, apply them now.
                foreach (ColumnHeader col in listView1.Columns)
                {
                    if (_settings.ListViewColumnWidths.ContainsKey(col.Text))
                        col.Width = _settings.ListViewColumnWidths[col.Text];
                }
            }
            else
            {
                // If this is a first run, apply the default layout.
                SetDefaultFixedColumnWidths();
                ResizeNameColumn();
            }

            // --- Finally, re-subscribe to events now that the initial layout is complete ---
            this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
            this.listView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
            this.listView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Check if there is text in the search box.
            bool hasText = !string.IsNullOrWhiteSpace(txtSearch.Text);

            // Show the icon only if there is text to clear.
            // picClearSearch.Visible = hasText;

            // Change the icon color based on text.
            // We can just show/hide it, which is simpler and cleaner.
            picClearSearch.Image = hasText ? Properties.Resources.clear_red : Properties.Resources.clear_gray;

            // Re-populate the list with the new filter.
            PopulateListBox(txtSearch.Text);

            // --- Manually refresh the details panel ---
            // After the list has been rebuilt, we check the state of the selection.
            if (listView1.SelectedItems.Count > 0)
            {
                // If an item is still selected, we MUST re-run the LoadDetailsForItem method
                // for it. This will correctly apply (or remove) the highlighting based
                // on the NEW filter text (which may now be empty).
                LoadDetailsForItem(listView1.SelectedItems[0]);
            }
            else
            {
                // If the filter caused the previously selected item to disappear,
                // the list selection will be empty. In this case, reset the panel.
                ResetDetailsPanel();
            }
        }

        private void picClearSearch_Click(object sender, EventArgs e)
        {
            // Clear the text. This will automatically trigger txtSearch_TextChanged,
            // which will hide the icon and refresh the list.
            txtSearch.Clear();

            // Give focus back to the search box for a better user experience.
            txtSearch.Focus();
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            // This tells the dialog to open in its expanded state.
            colorDialog1.FullOpen = true;

            // Load the current highlight color directly from settings.
            colorDialog1.Color = Color.FromArgb(_settings.HighlightColorArgb);

            // Load any saved custom colors from settings into the dialog.
            if (_settings.CustomColors != null)
            {
                colorDialog1.CustomColors = _settings.CustomColors;
            }

            // Show the dialog. If the user clicks "OK"...
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // Save the chosen color and custom colors DIRECTLY to the settings object.
                _settings.HighlightColorArgb = colorDialog1.Color.ToArgb();
                _settings.CustomColors = colorDialog1.CustomColors;

                // Force a refresh of the details panel to show the new color instantly.
                if (listView1.SelectedItems.Count > 0)
                {
                    LoadDetailsForItem(listView1.SelectedItems[0]);
                }
            }
        }

        //**********************************************
        /// <summary>
        /// Recursively finds all controls of a specific type within a parent control.
        /// </summary>
        /// <typeparam name="T">The type of control to find (e.g., TextBox, CheckBox).</typeparam>
        /// <param name="parent">The control to start the search from.</param>
        /// <returns>An enumeration of all matching controls.</returns>
        //**********************************************
        public IEnumerable<T> GetAllControls<T>(Control parent) where T : Control
        {
            // Get all controls in the parent's immediate children.
            var controls = parent.Controls.OfType<T>();

            // For each immediate child, recursively call this method and add its results.
            foreach (var control in parent.Controls.OfType<Control>())
            {
                controls = controls.Concat(GetAllControls<T>(control));
            }

            return controls;
        }

        private void ResetDetailsPanel()
        {
            // 1. Disable the entire group box.
            groupBox2.Enabled = false;

            // 2. Handle the Label separately.
            tbSavedWindowIndex.Text = "";
            //ClsDebug.LogNow("[ResetDetailsPanel] Cleared tbSavedWindowIndex");

            // 3. Use the helper to find ALL TextBoxes, no matter where they are.
            var allTextBoxes = GetAllControls<TextBox>(groupBox2);
            foreach (var tb in allTextBoxes)
            {
                tb.Clear();
                tb.BackColor = SystemColors.Window; // Reset background color
                //ClsDebug.LogNow($"[ResetDetailsPanel] Cleared and reset background color of TextBox: {tb.Name}");
            }

            // 4. Use the helper to find ALL CheckBoxes.
            var allCheckBoxes = GetAllControls<CheckBox>(groupBox2);
            foreach (var cb in allCheckBoxes)
            {
                cb.Checked = false;
                //ClsDebug.LogNow($"[ResetDetailsPanel] Unchecked CheckBox: {cb.Name}");
            }

            // 5. Use the helper to find ALL RadioButtons.
            var allRadioButtons = GetAllControls<RadioButton>(groupBox2);
            foreach (var rb in allRadioButtons)
            {
                rb.Checked = false;
                //ClsDebug.LogNow($"[ResetDetailsPanel] Unchecked RadioButton: {rb.Name}");
            }
        }

        private void LoadDetailsForItem(ListViewItem selectedItem)
        {
            // First, get a list of all textboxes in the panel.
            var allTextBoxes = GetAllControls<TextBox>(groupBox2);
            // Before doing anything else, reset all their background colors to the default.
            foreach (var tb in allTextBoxes)
            {
                tb.BackColor = SystemColors.Window;
            }

            // --- 1. Get the Data Objects ---
            int Index = _savedWindows.GetWindowIndexByTag((int)selectedItem.Tag);
            if (Index == -1) return; // Safety check

            ClsWindowProps Win = _savedWindows.Props[Index];
            int screenIndex = _screens.GetScreenIndexForWindow(Win);

            // --- 2. Enable the UI Panel ---
            groupBox2.Enabled = true;

            // --- 3. Load Simple Text and CheckBox Values ---
            tbName.Text = Win.Name;
            tbWindowClass.Text = Win.WindowClass;
            tbExe.Text = Win.Exe;
            tbTitleInclude.Text = Win.TitleInclude ?? ""; // Use ?? "" to handle potential nulls
            tbTitleExclude.Text = Win.TitleExclude ?? "";

            if (ClsDebug.Debug)
                tbSavedWindowIndex.Text = Index.ToString();
            else
                tbSavedWindowIndex.Text = "";

            cbWindowClass.Checked = Win.ConsiderWindowClass;
            cbSearchTitleInclude.Checked = Win.SearchTitleInclude;
            cbSearchTitleExclude.Checked = Win.SearchTitleExclude;
            cbSearchExe.Checked = Win.SearchExe;
            cbIgnoreChildWindows.Checked = Win.IgnoreChildWindows;
            cbAlwaysMove.Checked = Win.AlwaysMove;
            cbCanResize.Checked = Win.CanResize;
            cbCustomWidth.Checked = Win.MaxWidth;
            cbCustomHeight.Checked = Win.MaxHeight;
            cbFullScreen.Checked = Win.FullScreen;

            // --- 4. Handle Screen-Dependent Geometry ---
            if (screenIndex > -1)
            {
                if (Win.MaxWidth)
                {
                    tbWidth.Text = _screens.ScreenList[screenIndex].CustomWidth.ToString();
                    tbLeft.Text = "0";
                }
                else
                {
                    tbWidth.Text = Win.Width.ToString();
                    tbLeft.Text = Win.Left.ToString();
                }

                if (Win.MaxHeight)
                {
                    tbHeight.Text = _screens.ScreenList[screenIndex].CustomHeight.ToString();
                    tbTop.Text = "0";
                }
                else
                {
                    tbHeight.Text = Win.Height.ToString();
                    tbTop.Text = Win.Top.ToString();
                }
            }
            else // Fallback for a disconnected monitor
            {
                tbWidth.Text = Win.Width.ToString();
                tbLeft.Text = Win.Left.ToString();
                tbHeight.Text = Win.Height.ToString();
                tbTop.Text = Win.Top.ToString();
            }

            // --- 5. Handle Radio Buttons ---
            switch (Win.SearchTypeInclude)
            {
                case ClsWindowProps.Full: radioFullInclude.Checked = true; break;
                case ClsWindowProps.Contains: radioContainsInclude.Checked = true; break;
                case ClsWindowProps.StartsWith: radioStartsWithInclude.Checked = true; break;
            }
            switch (Win.SearchTypeExclude)
            {
                case ClsWindowProps.Full: radioFullExclude.Checked = true; break;
                case ClsWindowProps.Contains: radioContainsExclude.Checked = true; break;
                case ClsWindowProps.StartsWith: radioStartsWithExclude.Checked = true; break;
            }

            // --- 6. Apply Search Highlighting ---
            string filter = txtSearch.Text;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var tb in allTextBoxes)
                {
                    // Reset color first
                    tb.BackColor = SystemColors.Window;
                    if (tb.Text.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        tb.BackColor = Color.FromArgb(_settings.HighlightColorArgb);
                    }
                }
            }

            // --- 7. Trigger UI Updates ---
            // This is crucial to ensure textboxes are enabled/disabled correctly based on the loaded checkbox values.
            cbWindowClass_CheckedChanged(this, EventArgs.Empty);
            cbSearchTitleInclude_CheckedChanged(this, EventArgs.Empty);
            cbSearchTitleExclude_CheckedChanged(this, EventArgs.Empty);
            cbSearchExe_CheckedChanged(this, EventArgs.Empty);
            cbCanResize_CheckedChanged(this, EventArgs.Empty);
            cbFullScreen_CheckedChanged(this, EventArgs.Empty); // This also handles custom width/height logic
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            // We only care about the primary (left) mouse button for deselection.
            if (e.Button == MouseButtons.Left)
            {
                var hitTestInfo = listView1.HitTest(e.X, e.Y);
                //ClsDebug.LogNow($"[listView1_MouseDown] Mouse Down at X:{e.X}, Y:{e.Y}");

                // If the HitTest shows the click was on an empty area...
                if (hitTestInfo.Item == null)
                {
                    // This is a confirmed left-click on the background.
                    // Explicitly clear any selected items to be safe.
                    listView1.SelectedItems.Clear();

                    // And reset the details panel.
                    ResetDetailsPanel();
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if the key being pressed is the Escape key.
            if (keyData == Keys.Escape)
            {
                // Check if our search textbox currently has focus.
                if (this.ActiveControl == txtSearch)
                {
                    // If it does, we perform our custom action: clear the text.
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        txtSearch.Clear();
                    }

                    // IMPORTANT: Return true to signal that we have handled this key.
                    // This stops the form from processing it further (i.e., it won't trigger the CancelButton).
                    return true;
                }
            }

            // For all other keys, or if the search box doesn't have focus,
            // let the form continue with its normal behavior.
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
