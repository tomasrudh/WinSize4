﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Windows.UI.Notifications;

namespace WinSize4
{
    public partial class frmMain : Form
    {
        public ClsSettings _settings = new ClsSettings();                    // Global settings
        public ClsCurrentWindows _currentWindows = new ClsCurrentWindows();  // Windows open right now
        public ClsSavedWindows _savedWindows = new ClsSavedWindows();        // Windows saved on file
        public ClsScreens _screens = new ClsScreens();
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

        //**********************************************
        // Form initialize
        //**********************************************
        public frmMain()
        {
            InitializeComponent();
            _settings.LoadFromFile();
            _savedWindows.Load(_settings.dataPath);
            _screens.Load(_settings.dataPath);
            _screens.AddNewScreens(_settings.dataPath);
            _screens.SetPresent(_settings.dataPath);
            _screens.Save(_settings.dataPath);
            //_savedWindows.Order();
            PopulateListBox();
            //txtVersion.Text = GetType().Assembly.GetName().Version.ToString();
            // Set version: Project -> WinSize4 Properties: Package - General - Assembly version / File version
            txtVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //ClsDebug.ClearLog(_settings.dataPath);
            ClsDebug.LogNow(_settings.dataPath, "\nScreens:");
            for (int i = 0; i < _screens.ScreenList.Count; i++)
            {
                ClsDebug.LogNow(_settings.dataPath, " Screen " + i + " " + _screens.ScreenList[i].BoundsWidth + " " + _screens.ScreenList[i].BoundsHeight +
                    " Primary: " + _screens.ScreenList[i].Primary + " Present: " + _screens.ScreenList[i].Present);
            }
            ClsDebug.LogNow(_settings.dataPath, "\nSaved windows:");
            for (int i = 0; i < _savedWindows.Props.Count; i++)
            {
                ClsDebug.LogNow(_settings.dataPath, " Window " + i + " " + _savedWindows.Props[i].Name + " " + _savedWindows.Props[i].Exe + " " + _savedWindows.Props[i].MonitorBoundsWidth +
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
            cbIsPaused.Checked = _settings.isPaused;
            ClsDebug.AddText("\nStarting");
            ClsDebug.LogText(_settings.dataPath);
            timer1.Interval = _settings.Interval;
            RegisterListener();
            //this.AddHandler(KeyDownEvent, new KeyEventHandler(KeyDown), true);
        }

        //**********************************************
        // Form load
        //**********************************************
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
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
                    bool Changed = _screens.AddNewScreens(_settings.dataPath);
                    if (_settings.resetIfNewScreen && Changed)
                        _currentWindows.ResetMoved(_settings.dataPath);

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
                        this.Invoke((MethodInvoker)delegate {
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
                ClsDebug._text = "";
                ClsDebug.AddText("Timer1_Tick starting");
                timer1.Stop();
                long hWnd = (long)GetForegroundWindow();
                if (_settings.isPaused)
                {
                    ClsDebug.AddText("WinSize4 is paused");
                }
                if (hWnd > 0 && _settings.isPaused == false)
                {
                    bool NewScreen = _screens.AddNewScreens(_settings.dataPath);
                    bool ChangedScreens = _screens.SetPresent(_settings.dataPath);
                    if (_settings.resetIfNewScreen && (NewScreen || ChangedScreens))
                    {
                        _currentWindows.ResetMoved(_settings.dataPath);
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
                        ClsDebug.LogNow(_settings.dataPath, "Checking window: " + Text);
                    }
                    if (targetSavedWindowsIndex > -1 &&
                        currentWindowProps.Title != "" &&
                        currentWindowProps.Title != _lastTitle &&
                        (_currentWindows.Windows[currentWindowsIndex].Moved == false || _savedWindows.Props[targetSavedWindowsIndex].AlwaysMove == true) &&
                        !_savedWindows.Props[targetSavedWindowsIndex].Disabled)
                    {
                        int targetScreenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[targetSavedWindowsIndex]);
                        //bool targetWindowHasParent = ((int)GetParent((IntPtr)hWnd) > 0);
                        bool targetWindowHasParent = ((int)GetWindow((IntPtr)hWnd, 4) > 0);
                        // Only continue if window is not a child window to a current window, or child windows should be considered
                        if (!_savedWindows.Props[targetSavedWindowsIndex].IgnoreChildWindows || !targetWindowHasParent)
                        {
                            ClsDebug.AddText("Moving window: " + Text);

                            _currentWindows.MoveCurrentWindow(currentWindowsIndex,
                                _savedWindows.Props[targetSavedWindowsIndex],
                                targetSavedWindowsIndex,
                                _screens.ScreenList[targetScreenIndex],
                                new ClsScreenList(),
                                _settings.dataPath);
                            _currentWindows.UpdateWindowProperties(currentWindowsIndex);
                            _currentWindows.Windows[currentWindowsIndex].Moved = true;
                            //Debug.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                            ClsDebug.AddText($"Execution Time: {watch.ElapsedMilliseconds} ms");
                            watch.Stop();
                            ClsDebug.LogText(_settings.dataPath);
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
        private void PopulateListBox()
        {
            try
            {
                int lastSelected = 0;
                if (listView1.SelectedItems.Count > 0)
                {
                    lastSelected = listView1.SelectedItems[0].Index;
                }
                listView1.Items.Clear();
                string[] row;
                for (int i = 0; i < _savedWindows.Props.Count; i++)
                {
                    string Primary = _savedWindows.Props[i].Primary ? "Yes" : "No";
                    row = new string[] { _savedWindows.Props[i].Name, _savedWindows.Props[i].MonitorBoundsWidth.ToString(), _savedWindows.Props[i].MonitorBoundsHeight.ToString(), Primary };
                    var listViewItem = new ListViewItem(row);
                    listViewItem.Tag = _savedWindows.Props[i].Tag;
                    listViewItem.StateImageIndex = _savedWindows.Props[i].Disabled ? 1 : 0;
                    if (_screens.GetScreenIndexForWindow(_savedWindows.Props[i]) == -1)
                    {
                        listView1.Items.Add(listViewItem);
                    }
                    if (!_screens.ScreenList[_screens.GetScreenIndexForWindow(_savedWindows.Props[i])].Present)
                    {
                        if (_settings.showAllWindows)
                        {
                            listViewItem.ForeColor = Color.Silver;
                            listViewItem.SubItems[1].ForeColor = Color.Silver;
                            listViewItem.SubItems[2].ForeColor = Color.Silver;
                            listViewItem.SubItems[3].ForeColor = Color.Silver;
                            listViewItem.UseItemStyleForSubItems = false;
                            listView1.Items.Add(listViewItem);
                        }
                    }
                    else
                    {
                        listView1.Items.Add(listViewItem);
                    }

                }
                if (listView1.Items.Count > 0 && lastSelected < listView1.Items.Count)
                {
                    listView1.Items[lastSelected].Selected = true;
                }
                listView1.Select();
                notifyIcon1.BalloonTipTitle = "WinSize4";
                notifyIcon1.BalloonTipText = _savedWindows.Props.Count.ToString() + " controlled windows";
            }
            catch
            (Exception ex)
            {
                ClsDebug.LogToEvent(ex, EventLogEntryType.Error, "");
            }
        }
        //**********************************************
        // Listbox selection changed
        //**********************************************
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_ListBoxDoEvents)
                {
                    //if (_lbLastSelectedIndex > -1)
                    //    SaveValuesForIndex(_lbLastSelectedIndex);
                    if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Tag != null)
                    {
                        int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                        int screenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[Index]);

                        groupBox2.Enabled = true;

                        ClsWindowProps Win = _savedWindows.Props[Index];
                        if (Win.TitleInclude != null)
                        {
                            tbTitleInclude.Text = Win.TitleInclude;
                        }
                        else
                        {
                            tbTitleExclude.Text = "<No title>";
                        }
                        if (Win.TitleExclude != null)
                        {
                            tbTitleExclude.Text = Win.TitleExclude;
                        }
                        else
                        {
                            tbTitleInclude.Text = "";
                        }
                        if (ClsDebug.Debug)
                            tbSavedWindowIndex.Text = Index.ToString();
                        else
                            tbSavedWindowIndex.Text = "";
                        tbName.Text = Win.Name;
                        tbWindowClass.Text = Win.WindowClass;
                        cbWindowClass.Checked = Win.ConsiderWindowClass;
                        cbSearchTitleInclude.Checked = Win.SearchTitleInclude;
                        cbSearchTitleExclude.Checked = Win.SearchTitleExclude;
                        tbExe.Text = Win.Exe;
                        cbSearchExe.Checked = Win.SearchExe;
                        cbIgnoreChildWindows.Checked = Win.IgnoreChildWindows;
                        cbAlwaysMove.Checked = Win.AlwaysMove;
                        cbCanResize.Checked = Win.CanResize;

                        cbCustomWidth.Checked = Win.MaxWidth;
                        if (cbCustomWidth.Checked)
                        {
                            tbWidth.Value = _screens.ScreenList[screenIndex].CustomWidth;
                            tbLeft.Value = 0;
                        }
                        else
                        {
                            tbWidth.Value = Win.Width;
                            tbLeft.Value = Win.Left;
                        }

                        cbCustomHeight.Checked = Win.MaxHeight;
                        if (cbCustomHeight.Checked)
                        {
                            tbHeight.Value = _screens.ScreenList[screenIndex].CustomHeight;
                            tbTop.Value = 0;
                        }
                        else
                        {
                            tbHeight.Value = Win.Height;
                            tbTop.Value = Win.Top;
                        }

                        cbFullScreen.Checked = Win.FullScreen;
                        switch (Win.SearchTypeInclude)
                        {
                            case ClsWindowProps.Full:
                                radioFullInclude.Checked = true;
                                break;
                            case ClsWindowProps.Contains:
                                radioContainsInclude.Checked = true;
                                break;
                            case ClsWindowProps.StartsWith:
                                radioStartsWithInclude.Checked = true;
                                break;
                        }
                        switch (Win.SearchTypeExclude)
                        {
                            case ClsWindowProps.Full:
                                radioFullExclude.Checked = true;
                                break;
                            case ClsWindowProps.Contains:
                                radioContainsExclude.Checked = true;
                                break;
                            case ClsWindowProps.StartsWith:
                                radioStartsWithExclude.Checked = true;
                                break;
                        }
                    }
                    else
                    // No item selected
                    {
                        tbName.Text = "";
                        cbWindowClass.Checked = false;
                        tbWindowClass.Text = "";
                        tbTitleInclude.Text = "";
                        cbSearchTitleInclude.Checked = false;
                        tbTitleExclude.Text = "";
                        cbSearchTitleExclude.Checked = false;
                        tbExe.Text = "";
                        cbSearchExe.Checked = false;
                        groupBox2.Enabled = false;
                    }
                }
            }
            catch
            (Exception ex)
            {
                ClsDebug.LogToEvent(ex, EventLogEntryType.Error, "");
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
                _savedWindows.Props[index].TitleInclude = tbTitleInclude.Text;
                _savedWindows.Props[index].TitleExclude = tbTitleExclude.Text;
                _savedWindows.Props[index].SearchTitleInclude = cbSearchTitleInclude.Checked;
                _savedWindows.Props[index].SearchTitleExclude = cbSearchTitleExclude.Checked;
                _savedWindows.Props[index].Exe = tbExe.Text;
                _savedWindows.Props[index].SearchExe = cbSearchExe.Checked;
                _savedWindows.Props[index].IgnoreChildWindows = cbIgnoreChildWindows.Checked;
                _savedWindows.Props[index].AlwaysMove = cbAlwaysMove.Checked;
                _savedWindows.Props[index].CanResize = cbCanResize.Checked;
                _savedWindows.Props[index].WindowClass = tbWindowClass.Text;
                if (!cbCustomWidth.Checked)
                {
                    _savedWindows.Props[index].Width = (int) tbWidth.Value;
                    _savedWindows.Props[index].Left = (int) tbLeft.Value;
                }
                if (!cbCustomHeight.Checked)
                {
                    _savedWindows.Props[index].Height = (int) tbHeight.Value;
                    _savedWindows.Props[index].Top = (int) tbTop.Value;
                }
                _savedWindows.Props[index].MaxWidth = cbCustomWidth.Checked;
                _savedWindows.Props[index].MaxHeight = cbCustomHeight.Checked;
                _savedWindows.Props[index].FullScreen = cbFullScreen.Checked;
                _savedWindows.Props[index].IgnoreChildWindows = cbIgnoreChildWindows.Checked;
                _savedWindows.Props[index].AlwaysMove = cbAlwaysMove.Checked;
                _savedWindows.Props[index].CanResize = cbCanResize.Checked;
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
                _screens.Save(_settings.dataPath);
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
            _savedWindows.Save(_settings.dataPath);
            _settings.SaveToFile();
            RegisterListener();
            _currentWindows.ResetMoved(_settings.dataPath);
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
            if (listView1.Items.Count > 0)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
                }
                PopulateListBox();
            }
            _savedWindows.Save(_settings.dataPath);
            _settings.SaveToFile();
            RegisterListener();
            _currentWindows.ResetMoved(_settings.dataPath);
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
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbLeft.Text, out int number))
                _dirty = true;
        }

        private void tbTop_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbTop.Text, out int number))
                _dirty = true;
        }

        private void tbWidth_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbWidth.Text, out int number))
                _dirty = true;
        }

        private void tbHeight_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbHeight.Text, out int number))
                _dirty = true;
        }

        private void cbCustomWidth_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomWidth.Checked || cbFullScreen.Checked)
            {
                int screenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[Index]);
                tbWidth.Value = _screens.ScreenList[screenIndex].CustomWidth;
                tbLeft.Value = 0;
                tbWidth.Enabled = false;
                tbLeft.Enabled = false;
            }
            else
            {
                tbWidth.Value = _savedWindows.Props[Index].Width;
                tbLeft.Value = _savedWindows.Props[Index].Left;
                tbWidth.Enabled = true;
                tbLeft.Enabled = true;
            }
            _dirty = true;
        }

        private void cbCustomHeight_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomHeight.Checked || cbFullScreen.Checked)
            {
                int screenIndex = _screens.GetScreenIndexForWindow(_savedWindows.Props[Index]);
                tbHeight.Value = _screens.ScreenList[screenIndex].CustomHeight;
                tbTop.Value = 0;
                tbHeight.Enabled = false;
                tbTop.Enabled = false;
            }
            else
            {
                tbHeight.Value = _savedWindows.Props[Index].Height;
                tbTop.Value = _savedWindows.Props[Index].Top;
                tbHeight.Enabled = true;
                tbTop.Enabled = true;
            }
            _dirty = true;
        }

        private void cbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            tbWidth.Enabled = !cbFullScreen.Checked;
            tbHeight.Enabled = !cbFullScreen.Checked;
            tbTop.Enabled = !cbFullScreen.Checked;
            tbLeft.Enabled = !cbFullScreen.Checked;
            cbCustomWidth.Enabled = !cbFullScreen.Checked;
            cbCustomHeight.Enabled = !cbFullScreen.Checked;
            if (cbFullScreen.Checked)
            {
                tbWidth.Value = 0;
                tbLeft.Value = 0;
                tbHeight.Value = 0;
                tbTop.Value = 0;
                tbWidth.Enabled = false;
                tbLeft.Enabled = false;
                tbHeight.Enabled = false;
                tbTop.Enabled = false;
            }
            else
            {
                tbWidth.Value = _savedWindows.Props[Index].Width;
                tbLeft.Value = _savedWindows.Props[Index].Left;
                tbHeight.Value = _savedWindows.Props[Index].Height;
                tbTop.Value = _savedWindows.Props[Index].Top;
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
            _currentWindows.ResetMoved(_settings.dataPath);
        }

        private void butShow_Click(object sender, EventArgs e)
        {
            allowVisible = true;
            Show();
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                if (listView1.SelectedItems[0].Index > -1)
                {
                    _savedWindows.Props.RemoveAt(Index);
                    PopulateListBox();
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
                PopulateListBox();
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
            Show();
            this.WindowState = FormWindowState.Normal;
            //notifyIcon1.Visible = false;
        }

        private void cbShowAllWindows_CheckedChanged(object sender, EventArgs e)
        {
            _settings.showAllWindows = cbShowAllWindows.Checked;
            PopulateListBox();
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
            }
            // Get detailed hit-test information about where the user clicked.
            var hitTestInfo = listView1.HitTest(e.X, e.Y);

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
                        //    Index 1 is the red cross, Index 0 is the green tick.
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
