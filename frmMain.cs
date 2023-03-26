using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

namespace WinSize4
{
    public partial class frmMain : Form
    {
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

        //**********************************************
        // Form initialize
        //**********************************************
        public frmMain()
        {
            InitializeComponent();
            _settings.LoadFromFile();
            _savedWindows.Load();
            _screens.Load();
            _screens.GetScreens();
            _screens.SetPresent();
            _screens.Save();
            //_savedWindows.Order();
            PopulateListBox();
            notifyIcon1.Text = _savedWindows.Props.Count.ToString() + " controlled windows";
            notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Reset moved", null, this.butResetMoved_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, this.butExit_Click);
            cbHotKeyLeft.SelectedIndex = cbHotKeyLeft.FindString(_settings.HotKeyLeft);
            cbHotKeyLeft_SelectedIndexChanged(this, EventArgs.Empty);
            cbSearchTitle_CheckedChanged(this, EventArgs.Empty);
            cbHotKeyRight.SelectedIndex = cbHotKeyRight.FindString(_settings.HotKeyRight);
            tbHotKeyCharacter.Text = _settings.HotKeyCharacter;
            cbShowAllWindows.Checked = _settings.showAllWindows;
            cbResetIfNewScreen.Checked = _settings.resetIfNewScreen;
            cbRunAtLogin.Checked = _settings.runAtLogin;
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
            // Stop the timer while we handle a hotkey click
            timer1.Stop();
            bool newWindow = false;
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == _hotKeyID)
            {
                long hWnd = (long)GetForegroundWindow();
                int focusIndex = _currentWindows.GetIndexForhWnd(hWnd);
                if (focusIndex == -1)
                {
                    // AddWindow the window to _currentWindows
                    _screens.GetScreens();
                    _currentWindows.Add(hWnd);
                    _dirty = true;
                    //_currentWindows.SetMoved(hWnd);
                    focusIndex = _currentWindows.Windows.Count - 1;
                }
                else
                {
                    // Update the existing window in _currentWindows
                    _currentWindows.UpdateWindowProperties(focusIndex);
                    _currentWindows.Windows[focusIndex].Moved = false;
                }
                int Tag;
                int screenIndex = _screens.GetIndexForWindow(_currentWindows.Windows[focusIndex].Props);
                if (!_savedWindows.UpdateWindowProperties(_currentWindows.Windows[focusIndex].Props, _screens.ScreenList, screenIndex))
                {
                    newWindow = true;
                    Tag = _savedWindows.AddWindow(_currentWindows.Windows[focusIndex].Props);
                }
                PopulateListBox();
                int savedWindowsIndex = _savedWindows.GetIndex(_currentWindows.Windows[focusIndex].Props, _screens.ScreenList, screenIndex);
                listView1.Items[GetListViewIndexByTag(_savedWindows.Props[savedWindowsIndex].Tag)].Selected = true;
                listView1.Select();

                ToastNotificationManagerCompat.OnActivated += toastArgs =>
                {
                    _toastClicked = true;
                };
                string newWindowText = newWindow == true ? "Adding as a new window" : "Updating an existing window";
                new ToastContentBuilder()
                    //.AddHeader("WinSize4", "WinSize4", "action=openConversation&id=WinSize4")
                    .AddText("WinSize4 controls a window")
                    .AddText(newWindowText)
                    .AddText(_savedWindows.Props[savedWindowsIndex].Title)
                    .SetBackgroundActivation()
                    .Show();
            }
            base.WndProc(ref m);
            timer1.Start();
        }

        //**********************************************
        // timer1 click
        //**********************************************
        private void timer1_Tick(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            timer1.Stop();
            int screenIndex;

            if (_toastClicked)
                this.Show();

            bool IsChild = false;
            bool IgnoreChildWindow = true;
            int savedWindowsIndex;

            //if (_screens.CleanScreenList() && _settings.resetIfNewScreen)
            //    _currentWindows.ResetMoved();

            long hWnd = (long)GetForegroundWindow();
            if (hWnd > 0)
            {
                _screens.SetPresent();
                _currentWindows.CleanWindowsList();
                GetWindowThreadProcessId((IntPtr)hWnd, out int Pid);
                IsChild = (_currentWindows.GetIndexForPid(Pid, hWnd) > -1);

                // Check if the window exists in _currentWindows, add it if not
                int currentWindowsIndex = _currentWindows.GetIndexForhWnd(hWnd);
                if (currentWindowsIndex == -1)
                {
                    _currentWindows.Add(hWnd);
                    currentWindowsIndex = _currentWindows.Windows.Count - 1;
                    ClsWindowProps CurrentWindowProps = _currentWindows.GetWindowProperties(hWnd);
                    screenIndex = _screens.GetIndexForWindow(_currentWindows.Windows[currentWindowsIndex].Props);
                    savedWindowsIndex = _savedWindows.GetIndex(CurrentWindowProps, _screens.ScreenList, screenIndex);
                    if (savedWindowsIndex > -1)
                    {
                        IgnoreChildWindow = _savedWindows.Props[savedWindowsIndex].IgnoreChildWindows;
                    }
                }
                // Only continue if window is not a child window to a current window, or child windows should be considered
                if (!IsChild || !IgnoreChildWindow)
                {
                    // If the window has no title, don't bother, it's probably a preview
                    if (_currentWindows.Windows[currentWindowsIndex].Props.Title != "")
                    {
                        // Does window exists in _savedWindows?
                        screenIndex = _screens.GetIndexForWindow(_currentWindows.Windows[currentWindowsIndex].Props);
                        savedWindowsIndex = _savedWindows.GetIndex(_currentWindows.Windows[currentWindowsIndex].Props, _screens.ScreenList, screenIndex);
                        if (savedWindowsIndex > -1)
                        {
                            // Is the window already moved?
                            if (!_currentWindows.Windows[currentWindowsIndex].Moved)
                            {
                                // Find primary screen and that screen in _currentScreens
                                int priScreenIndex;
                                Screen PriScr;
                                foreach (Screen s in Screen.AllScreens)
                                {
                                    if (s.Primary)
                                    {
                                        PriScr = s;
                                        for (int i = 0; i < _screens.ScreenList.Count; i++)
                                        {
                                            if (_screens.ScreenList[i].BoundsWidth == PriScr.Bounds.Width &&
                                                _screens.ScreenList[i].BoundsHeight == PriScr.Bounds.Height &&
                                                _screens.ScreenList[i].Primary == PriScr.Primary)
                                            {
                                                priScreenIndex = i;
                                                break;
                                            }
                                        }
                                    }
                                }

                                // Find index in _currentScreens for the window, or -1
                                Screen Scr = Screen.FromHandle((IntPtr)hWnd);
                                screenIndex = _screens.GetIndexForWindow(_currentWindows.Windows[currentWindowsIndex].Props);
                                if (screenIndex == -1)
                                {
                                    ClsScreenList NewScr = new ClsScreenList();
                                    NewScr.BoundsWidth = Scr.Bounds.Width;
                                    NewScr.BoundsHeight = Scr.Bounds.Height;
                                    NewScr.WorkingAreaWidth = Scr.WorkingArea.Width;
                                    NewScr.WorkingAreaHeight = Scr.WorkingArea.Height;
                                    NewScr.Primary = Scr.Primary;
                                    _screens.Add(NewScr);
                                    if (_settings.resetIfNewScreen)
                                        _currentWindows.ResetMoved();
                                    screenIndex = _screens.ScreenList.Count - 1;
                                }

                                // Find screen in _screens
                                int screenSavedIndex = _screens.GetIndexForScreen(_screens.ScreenList[screenIndex]);

                                // Move window
                                if (screenSavedIndex > -1)
                                {
                                    string Text = _currentWindows.Windows[currentWindowsIndex].hWnd + " " +
                                        _currentWindows.Windows[currentWindowsIndex].Pid + " " +
                                        _currentWindows.Windows[currentWindowsIndex].Props.Name + " " +
                                        _currentWindows.Windows[currentWindowsIndex].Props.Name + " " +
                                        _currentWindows.Windows[currentWindowsIndex].Props.WindowClass;
                                    Debug.WriteLine($"Moving window: " + Text);
                                    _currentWindows.MoveCurrentWindow(currentWindowsIndex,
                                        _savedWindows.Props[savedWindowsIndex],
                                        _screens.ScreenList[screenSavedIndex],
                                        _screens.ScreenList[screenIndex]);
                                    _currentWindows.UpdateWindowProperties(currentWindowsIndex);
                                    //_currentWindows.SetMoved(_currentWindows.Windows[currentWindowsIndex].hWnd);
                                    _currentWindows.Windows[currentWindowsIndex].Moved = true;
                                }
                            }
                        }
                    }
                }
                //_currentWindows.Windows[currentWindowsIndex].Moved = true;
            }

            timer1.Start();
            _toastClicked = false;
            watch.Stop();
            Debug.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        //**********************************************
        // Populate listbox
        //**********************************************
        private void PopulateListBox()
        {
            int lastSelected = 0;
            if (listView1.SelectedItems.Count > 0)
            {
                lastSelected = (int)listView1.SelectedItems[0].Index;
            }
            listView1.Items.Clear();
            string[] row;
            for (int i = 0; i < _savedWindows.Props.Count; i++)
            {
                string Primary = _savedWindows.Props[i].Primary ? "Yes" : "No";
                row = new string[] { _savedWindows.Props[i].Name, _savedWindows.Props[i].MonitorBoundsWidth.ToString(), _savedWindows.Props[i].MonitorBoundsHeight.ToString(), Primary };
                var listViewItem = new ListViewItem(row);
                listViewItem.Tag = _savedWindows.Props[i].Tag;
                if (!_screens.ScreenList[_screens.GetIndexForWindow(_savedWindows.Props[i])].Present)
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

        //**********************************************
        // Listbox selection changed
        //**********************************************
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ListBoxDoEvents)
            {
                //if (_lbLastSelectedIndex > -1)
                //    SaveValuesForIndex(_lbLastSelectedIndex);
                if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Tag != null)
                {
                    int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                    int screenIndex = _screens.GetIndexForWindow(_savedWindows.Props[Index]);

                    ClsWindowProps Win = _savedWindows.Props[Index];
                    if (Win.Title != null)
                    {
                        tbTitle.Text = Win.Title;
                    }
                    else
                    {
                        tbTitle.Text = "<No title>";
                    }
                    tbName.Text = Win.Name;
                    tbWindowClass.Text = Win.WindowClass;
                    cbWindowClass.Checked = Win.ConsiderWindowClass;
                    cbSearchTitle.Checked = Win.SearchTitle;
                    tbExe.Text = Win.Exe;
                    cbSearchExe.Checked = Win.SearchExe;

                    cbCustomWidth.Checked = Win.MaxWidth;
                    if (cbCustomWidth.Checked)
                    {
                        tbWidth.Text = _screens.ScreenList[screenIndex].CustomWidth.ToString();
                        tbLeft.Text = "0";
                    }
                    else
                    {
                        tbWidth.Text = Win.Width.ToString();
                        tbLeft.Text = Win.Left.ToString();
                    }

                    cbCustomHeight.Checked = Win.MaxHeight;
                    if (cbCustomHeight.Checked)
                    {
                        tbHeight.Text = _screens.ScreenList[screenIndex].CustomHeight.ToString();
                        tbTop.Text = "0";
                    }
                    else
                    {
                        tbHeight.Text = Win.Height.ToString();
                        tbTop.Text = Win.Top.ToString();
                    }

                    cbFullScreen.Checked = Win.FullScreen;
                    cbIgnoreChildWindows.Checked = Win.IgnoreChildWindows;
                    switch (Win.SearchType)
                    {
                        case ClsWindowProps.Full:
                            radioFull.Checked = true;
                            break;
                        case ClsWindowProps.Contains:
                            radioContains.Checked = true;
                            break;
                        case ClsWindowProps.StartsWith:
                            radioStartsWith.Checked = true;
                            break;
                    }
                }
            }
            //_lbLastSelectedIndex = listView1.SelectedItems[0].Index;
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
                _savedWindows.Props[index].Title = tbTitle.Text;
                _savedWindows.Props[index].SearchTitle = cbSearchTitle.Checked;
                _savedWindows.Props[index].Exe = tbExe.Text;
                _savedWindows.Props[index].SearchExe = cbSearchExe.Checked;
                if (!cbCustomWidth.Checked)
                {
                    _savedWindows.Props[index].Width = int.Parse(tbWidth.Text);
                    _savedWindows.Props[index].Left = int.Parse(tbLeft.Text);
                }
                if (!cbCustomHeight.Checked)
                {
                    _savedWindows.Props[index].Height = int.Parse(tbHeight.Text);
                    _savedWindows.Props[index].Top = int.Parse(tbTop.Text);
                }
                _savedWindows.Props[index].MaxWidth = cbCustomWidth.Checked;
                _savedWindows.Props[index].MaxHeight = cbCustomHeight.Checked;
                _savedWindows.Props[index].FullScreen = cbFullScreen.Checked;
                _savedWindows.Props[index].IgnoreChildWindows = cbIgnoreChildWindows.Checked;
                if (radioFull.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.Full;
                if (radioContains.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.Contains;
                if (radioStartsWith.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.StartsWith;
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
                    //.AddText(_savedWindows.Props[_savedWindows.Props.Count - 1].Title)
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
                _screens.ScreenList = Scr._returnScreenList;
                _screens.Save();
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
            _savedWindows.Save();
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
            if (listView1.Items.Count > 0)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
                }
                PopulateListBox();
            }
            _savedWindows.Save();
            _settings.SaveToFile();
            RegisterListener();
            _currentWindows.ResetMoved();
        }

        //private void 

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
                int screenIndex = _screens.GetIndexForWindow(_savedWindows.Props[Index]);
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
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomHeight.Checked || cbFullScreen.Checked)
            {
                int screenIndex = _screens.GetIndexForWindow(_savedWindows.Props[Index]);
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

        private void cbSearchTitle_CheckedChanged(object sender, EventArgs e)
        {
            int Index;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            else
                Index = 0;
            tbTitle.Enabled = cbSearchTitle.Checked;
            radioFull.Enabled = cbSearchTitle.Checked;
            radioContains.Enabled = cbSearchTitle.Checked;
            radioStartsWith.Enabled = cbSearchTitle.Checked;
            if (!cbSearchTitle.Checked)
                cbSearchExe.Checked = true;
            _dirty = true;
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
                cbSearchTitle.Checked = true;
            _dirty = true;
        }

        private void butResetMoved_Click(object sender, EventArgs e)
        {
            _currentWindows.ResetMoved();
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

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            switch (((System.Windows.Forms.RadioButton)sender).Name)
            {
                case "radioFull":
                    //_savedWindows.Props[Index].SearchType = ClsWindowProps.Full;
                    break;
                case "radioContains":
                    //_savedWindows.Props[Index].SearchType = ClsWindowProps.Contains;
                    break;
                case "radioStartsWith":
                    //_savedWindows.Props[Index].SearchType = ClsWindowProps.StartsWith;
                    break;
            }
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
    }
}
