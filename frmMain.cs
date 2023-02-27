using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WinSize4
{
    public partial class frmMain : Form
    {
        public ClsCurrentWindows _currentWindows = new ClsCurrentWindows();  // Windows open right now
        public ClsSavedWindows _savedWindows = new ClsSavedWindows();        // Windows saved on file
        public ClsCurrentScreens _currentScreens = new ClsCurrentScreens();  // Monitors active right now
        public ClsSavedScreens _savedScreens = new ClsSavedScreens();        // Screens saved on file
        public ClsSettings _settings = new ClsSettings();                    // Global settings
        const int _hotKeyID = 1;
        //const int _hotKeyModifierLeft = 2;    //Alt = 1, Ctrl = 2, Shift = 4, Win(Windows key for opening the start menu) = 8
        //const int _hotKeyModifierRight = 4;   //Alt = 1, Ctrl = 2, Shift = 4, Win(Windows key for opening the start menu) = 8
        int _lbLastSelectedIndex = -1;        // The last selected item
        bool _ListBoxDoEvents = true;
        bool _closeWindow = false;
        bool _dirty = false;

        //**********************************************
        // Form initialize
        //**********************************************
        public frmMain()
        {
            InitializeComponent();
            _settings.LoadFromFile();
            _savedWindows.Load();
            _savedScreens.Load();
            _currentScreens.GetScreens();
            _savedScreens.Save();
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
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == _hotKeyID)
            {
                long hWnd = (long)GetForegroundWindow();
                int focusIndex = _currentWindows.GetIndexForhWnd(hWnd);
                if (focusIndex == -1)
                {
                    // Add the window to _currentWindows
                    _currentScreens.GetScreens();
                    _currentWindows.Add(hWnd);
                    _dirty = true;
                    //_currentWindows.SetMoved(hWnd);
                    focusIndex = _currentWindows.Windows.Count - 1;
                }
                else
                {
                    // Update the existing window in _currentWindows
                    _currentWindows.UpdateWindowProperties(focusIndex);
                }
                if (!_savedWindows.UpdateWindowProperties(_currentWindows.Windows[focusIndex].Props)) {
                    _savedWindows.Add(_currentWindows.Windows[focusIndex].Props, hWnd);
                }
                PopulateListBox();
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("WinSize4 controls a window")
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
            // Get foreground window, add it to _currentWindows, if not already there
            long hWnd = (long)GetForegroundWindow();
            int focusIndex = _currentWindows.GetIndexForhWnd(hWnd);
            // Check if the window exists in _currentWindows, add it if not
            if (focusIndex == -1)
            {
                _currentWindows.Add((long)GetForegroundWindow());
                focusIndex = _currentWindows.Windows.Count - 1;
            }

            //if (_currentScreens.CleanScreenList() && _settings.resetIfNewScreen)
            //    _currentWindows.ResetMoved();

            // If the window has no title, don't bother, it's probably a preview
            if (_currentWindows.Windows[focusIndex].Props.Title != "")
            {
                // Does window exists in _savedWindows?
                int savedWindowsIndex = _savedWindows.GetIndex(_currentWindows.Windows[focusIndex].Props);
                if (savedWindowsIndex > -1)
                {
                    // Is the window already moved?
                    if (!_currentWindows.Windows[focusIndex].Moved)
                    {
                        // Find primary screen and that screen in _currentScreens
                        int priScreenIndex;
                        Screen PriScr;
                        foreach (Screen s in Screen.AllScreens)
                        {
                            if (s.Primary)
                            {
                                PriScr = s;
                                for (int i = 0; i < _currentScreens.ScreenList.Count; i++)
                                {
                                    if (_currentScreens.ScreenList[i].BoundsWidth == PriScr.Bounds.Width &&
                                        _currentScreens.ScreenList[i].BoundsHeight == PriScr.Bounds.Height &&
                                        _currentScreens.ScreenList[i].Primary == PriScr.Primary)
                                    {
                                        priScreenIndex = i;
                                        break;
                                    }
                                }
                            }
                        }

                        // Find index in _currentScreens for the window, or -1
                        Screen Scr = Screen.FromHandle((IntPtr)hWnd);
                        int screenCurrentIndex = -1;
                        for (int i = 0; i < _currentScreens.ScreenList.Count; i++)
                        {
                            if (_currentScreens.ScreenList[i].BoundsWidth == Scr.Bounds.Width &&
                                _currentScreens.ScreenList[i].BoundsHeight == Scr.Bounds.Height &&
                                _currentScreens.ScreenList[i].Primary == Scr.Primary)
                            {
                                screenCurrentIndex = i;
                                break;
                            }
                        }
                        if (screenCurrentIndex == -1)
                        {
                            ClsScreenList NewScr = new ClsScreenList();
                            NewScr.BoundsWidth = Scr.Bounds.Width;
                            NewScr.BoundsHeight = Scr.Bounds.Height;
                            NewScr.WorkingAreaWidth = Scr.WorkingArea.Width;
                            NewScr.WorkingAreaHeight = Scr.WorkingArea.Height;
                            NewScr.Primary = Scr.Primary;
                            _currentScreens.Add(NewScr);
                            if (_settings.resetIfNewScreen)
                                _currentWindows.ResetMoved();
                            screenCurrentIndex = _currentScreens.ScreenList.Count - 1;
                        }

                        // Find screen in _savedScreens
                        int screenSavedIndex = _savedScreens.GetIndexForScreen(_currentScreens.ScreenList[screenCurrentIndex]);

                        // Move window
                        _currentWindows.MoveCurrentWindow(focusIndex,
                            _savedWindows.Props[savedWindowsIndex],
                            _savedScreens.ScreenList[screenSavedIndex],
                            _currentScreens.ScreenList[screenCurrentIndex]);
                        _currentWindows.UpdateWindowProperties(focusIndex);
                        //_currentWindows.SetMoved(_currentWindows.Windows[focusIndex].hWnd);
                        _currentWindows.Windows[focusIndex].Moved = true;
                        _currentWindows.CleanWindowsList();
                    }
                }
            }
        }

        //**********************************************
        // Populate listbox
        //**********************************************
        private void PopulateListBox()
        {
            listView1.Items.Clear();
            string[] row;
            for (int i = 0; i < _savedWindows.Props.Count; i++)
            {
                string Primary = _savedWindows.Props[i].Primary ? "Yes" : "No";
                row = new string[] { _savedWindows.Props[i].Name, _savedWindows.Props[i].MonitorBoundsWidth.ToString(), _savedWindows.Props[i].MonitorBoundsHeight.ToString(), Primary };
                var listViewItem = new System.Windows.Forms.ListViewItem(row);
                listViewItem.Tag = _savedWindows.Props[i].Tag;
                if (_currentScreens.GetIndex(_savedWindows.Props[i]) < 0)
                {
                    if (_settings.showAllWindows)
                    {
                        listViewItem.ForeColor = System.Drawing.Color.Silver;
                        listView1.Items.Add(listViewItem);
                    }
                }
                else
                {
                    listView1.Items.Add(listViewItem);
                }

            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                _lbLastSelectedIndex = 0;
            }
            listView1.Select();
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
                    ClsWindowProps Win = _savedWindows.GetWindowByTag((int)listView1.SelectedItems[0].Tag);
                    if (Win.Title != null)
                    {
                        tbTitle.Text = Win.Title;
                    }
                    else
                    {
                        tbTitle.Text = "<No title>";
                    }
                    tbName.Text = Win.Name;
                    cbSearchTitle.Checked = Win.SearchTitle;
                    tbExe.Text = Win.Exe;
                    cbSearchExe.Checked = Win.SearchExe;
                    tbLeft.Text = Win.Left.ToString();
                    tbTop.Text = Win.Top.ToString();
                    tbWidth.Text = Win.Width.ToString();
                    tbHeight.Text = Win.Height.ToString();
                    cbCustomWidth.Checked = Win.MaxWidth;
                    cbCustomHeight.Checked = Win.MaxHeight;
                    cbFullScreen.Checked = Win.FullScreen;
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
        // Save values
        //**********************************************
        private void SaveValuesForIndex(int index)
        {
            if (index > -1)
            {
                _savedWindows.Props[index].Name = tbName.Text;
                _savedWindows.Props[index].Title = tbTitle.Text;
                _savedWindows.Props[index].SearchTitle = cbSearchTitle.Checked;
                _savedWindows.Props[index].Exe = tbExe.Text;
                _savedWindows.Props[index].SearchExe = cbSearchExe.Checked;
                _savedWindows.Props[index].Left = int.Parse(tbLeft.Text);
                _savedWindows.Props[index].Top = int.Parse(tbTop.Text);
                _savedWindows.Props[index].Width = int.Parse(tbWidth.Text);
                _savedWindows.Props[index].Height = int.Parse(tbHeight.Text);
                _savedWindows.Props[index].MaxWidth = cbCustomWidth.Checked;
                _savedWindows.Props[index].MaxHeight = cbCustomHeight.Checked;
                _savedWindows.Props[index].FullScreen = cbFullScreen.Checked;
                if (radioFull.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.Full;
                if (radioContains.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.Contains;
                if (radioStartsWith.Checked)
                    _savedWindows.Props[index].SearchType = ClsWindowProps.StartsWith;
                _ListBoxDoEvents = false;
                //listView1.Items[index] = tbName.Text;
                _ListBoxDoEvents = true;
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
        // Event handlers
        //**********************************************

        //**********************************************
        // Edit screens
        //**********************************************
        private void butEditScreens_Click(object sender, EventArgs e)
        {
            List<ClsScreenList> screenList = new List<ClsScreenList>(_savedScreens.ScreenList.Count);
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            _savedScreens.ScreenList.ForEach((item) =>
            {
                screenList.Add((ClsScreenList)item.Clone());
            });
            ClsWindowProps Win = _savedWindows.Props[Index];
            frmScreens Scr = new frmScreens(screenList,
                Win.MonitorBoundsWidth,
                Win.MonitorBoundsHeight,
                Win.Primary);
            var result = Scr.ShowDialog();
            if (result == DialogResult.OK)
            {
                _savedScreens.ScreenList = Scr._returnScreenList;
                _savedScreens.Save();
            }
        }

        //**********************************************
        // OK
        //**********************************************
        private void butOK_Click(object sender, EventArgs e)
        {
            SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
            _savedWindows.Save();
            _settings.SaveToFile();
            RegisterListener();
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
                SaveValuesForIndex(_savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag));
                PopulateListBox();
            }
            _savedWindows.Save();
            _settings.SaveToFile();
            //RegisterListener();
        }

        //private void 

        private void tbLeft_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbLeft.Text, out int number))
                _savedWindows.Props[Index].Left = number;
            _dirty = true;
        }

        private void tbTop_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbTop.Text, out int number))
                _savedWindows.Props[Index].Top = number;
            _dirty = true;
        }

        private void tbWidth_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbWidth.Text, out int number))
                _savedWindows.Props[Index].Width = number;
            _dirty = true;
        }

        private void tbHeight_TextChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (int.TryParse(tbHeight.Text, out int number))
                _savedWindows.Props[Index].Height = number;
            _dirty = true;
        }

        private void cbCustomWidth_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomWidth.Checked)
            {
                int screenIndex = _savedScreens.GetIndexForWindow(_savedWindows.Props[Index]);
                tbWidth.Text = _savedScreens.ScreenList[screenIndex].CustomWidth.ToString();
                tbLeft.Text = "0";
            }
            else
            {
                tbWidth.Text = _savedWindows.Props[Index].Width.ToString();
            }
            _savedWindows.Props[listView1.SelectedItems[0].Index].MaxWidth = cbCustomWidth.Checked;
            tbWidth.Enabled = !cbCustomWidth.Checked;
            tbLeft.Enabled = !cbCustomWidth.Checked;
            _dirty = true;
        }

        private void cbCustomHeight_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            if (cbCustomHeight.Checked)
            {
                int screenIndex = _savedScreens.GetIndexForWindow(_savedWindows.Props[Index]);
                tbHeight.Text = _savedScreens.ScreenList[screenIndex].CustomHeight.ToString();
                tbTop.Text = "0";
            }
            else
            {
                tbHeight.Text = _savedWindows.Props[Index].Height.ToString();
            }
            _savedWindows.Props[listView1.SelectedItems[0].Index].MaxHeight = cbCustomHeight.Checked;
            tbHeight.Enabled = !cbCustomHeight.Checked;
            tbTop.Enabled = !cbCustomHeight.Checked;
            _dirty = true;
        }

        private void cbSearchTitle_CheckedChanged(object sender, EventArgs e)
        {
            int Index;
            if (listView1.SelectedItems.Count > 0)
                Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            else
                Index = 0;
            _savedWindows.Props[Index].SearchTitle = cbSearchTitle.Checked;
            tbTitle.Enabled = cbSearchTitle.Checked;
            radioFull.Enabled = cbSearchTitle.Checked;
            radioContains.Enabled = cbSearchTitle.Checked;
            radioStartsWith.Enabled = cbSearchTitle.Checked;
            _dirty = true;
        }

        private void cbSearchExe_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            _savedWindows.Props[Index].SearchExe = cbSearchExe.Checked;
            tbExe.Enabled = cbSearchExe.Checked;
            _dirty = true;
        }

        private void butResetMoved_Click(object sender, EventArgs e)
        {
            _currentWindows.ResetMoved();
        }

        private void cbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
            _savedWindows.Props[Index].FullScreen = cbFullScreen.Checked;
            tbWidth.Enabled = !cbFullScreen.Checked;
            tbHeight.Enabled = !cbFullScreen.Checked;
            tbTop.Enabled = !cbFullScreen.Checked;
            tbLeft.Enabled = !cbFullScreen.Checked;
            cbCustomWidth.Enabled = !cbFullScreen.Checked;
            cbCustomHeight.Enabled = !cbFullScreen.Checked;
            cbCustomWidth_CheckedChanged(this, EventArgs.Empty);
            cbCustomHeight_CheckedChanged(this, EventArgs.Empty);
            _dirty = true;
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int Index = _savedWindows.GetWindowIndexByTag((int)listView1.SelectedItems[0].Tag);
                if (listView1.SelectedItems[0].Index > -1)
                {
                    _savedWindows.Props.RemoveAt(Index);
                    _lbLastSelectedIndex = -1;
                    PopulateListBox();
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[0].Selected = true;
                        listView1_SelectedIndexChanged(this, EventArgs.Empty);
                    }
                    _dirty = true;
                }
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
                    _savedWindows.Props[Index].SearchType = ClsWindowProps.Full;
                    break;
                case "radioContains":
                    _savedWindows.Props[Index].SearchType = ClsWindowProps.Contains;
                    break;
                case "radioStartsWith":
                    _savedWindows.Props[Index].SearchType = ClsWindowProps.StartsWith;
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
    }
}
