namespace WinSize4
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            timer1 = new System.Windows.Forms.Timer(components);
            tbTitle = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tbExe = new TextBox();
            butOK = new Button();
            butApply = new Button();
            butExit = new Button();
            butRemove = new Button();
            label3 = new Label();
            tbLeft = new TextBox();
            label4 = new Label();
            tbTop = new TextBox();
            label5 = new Label();
            tbWidth = new TextBox();
            label6 = new Label();
            tbHeight = new TextBox();
            cbCustomWidth = new CheckBox();
            cbCustomHeight = new CheckBox();
            cbFullScreen = new CheckBox();
            butCancel = new Button();
            butEditScreens = new Button();
            cbSearchExe = new CheckBox();
            cbSearchTitle = new CheckBox();
            groupBox1 = new GroupBox();
            tbHotKeyCharacter = new TextBox();
            cbHotKeyRight = new ComboBox();
            cbHotKeyLeft = new ComboBox();
            notifyIcon1 = new NotifyIcon(components);
            label7 = new Label();
            tbName = new TextBox();
            cbResetIfNewScreen = new CheckBox();
            groupBox2 = new GroupBox();
            tbWindowClass = new TextBox();
            label8 = new Label();
            cbWindowClass = new CheckBox();
            cbIgnoreChildWindows = new CheckBox();
            radioStartsWith = new RadioButton();
            radioContains = new RadioButton();
            radioFull = new RadioButton();
            butResetMoved = new Button();
            toolTip1 = new ToolTip(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            cbShowAllWindows = new CheckBox();
            cbRunAtLogin = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // tbTitle
            // 
            tbTitle.Location = new Point(164, 80);
            tbTitle.Margin = new Padding(4, 3, 4, 3);
            tbTitle.Name = "tbTitle";
            tbTitle.Size = new Size(305, 23);
            tbTitle.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 88);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 2;
            label1.Text = "Window Title";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 145);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 4;
            label2.Text = "Executable";
            // 
            // tbExe
            // 
            tbExe.Location = new Point(165, 136);
            tbExe.Margin = new Padding(4, 3, 4, 3);
            tbExe.Name = "tbExe";
            tbExe.Size = new Size(305, 23);
            tbExe.TabIndex = 3;
            // 
            // butOK
            // 
            butOK.Location = new Point(543, 512);
            butOK.Margin = new Padding(4, 3, 4, 3);
            butOK.Name = "butOK";
            butOK.Size = new Size(124, 27);
            butOK.TabIndex = 5;
            butOK.Text = "Apply and Minimize";
            butOK.UseVisualStyleBackColor = true;
            butOK.Click += butOK_Click;
            // 
            // butApply
            // 
            butApply.Location = new Point(768, 512);
            butApply.Margin = new Padding(4, 3, 4, 3);
            butApply.Name = "butApply";
            butApply.Size = new Size(88, 27);
            butApply.TabIndex = 6;
            butApply.Text = "Apply";
            butApply.UseVisualStyleBackColor = true;
            butApply.Click += butApply_Click;
            // 
            // butExit
            // 
            butExit.Location = new Point(862, 512);
            butExit.Margin = new Padding(4, 3, 4, 3);
            butExit.Name = "butExit";
            butExit.Size = new Size(88, 27);
            butExit.TabIndex = 7;
            butExit.Text = "Exit";
            butExit.UseVisualStyleBackColor = true;
            butExit.Click += butExit_Click;
            // 
            // butRemove
            // 
            butRemove.Location = new Point(9, 286);
            butRemove.Margin = new Padding(4, 3, 4, 3);
            butRemove.Name = "butRemove";
            butRemove.Size = new Size(136, 27);
            butRemove.TabIndex = 8;
            butRemove.Text = "Remove";
            butRemove.UseVisualStyleBackColor = true;
            butRemove.Click += butRemove_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 170);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(27, 15);
            label3.TabIndex = 10;
            label3.Text = "Left";
            // 
            // tbLeft
            // 
            tbLeft.Location = new Point(165, 166);
            tbLeft.Margin = new Padding(4, 3, 4, 3);
            tbLeft.Name = "tbLeft";
            tbLeft.Size = new Size(131, 23);
            tbLeft.TabIndex = 9;
            tbLeft.TextChanged += tbLeft_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 200);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(26, 15);
            label4.TabIndex = 12;
            label4.Text = "Top";
            // 
            // tbTop
            // 
            tbTop.Location = new Point(165, 196);
            tbTop.Margin = new Padding(4, 3, 4, 3);
            tbTop.Name = "tbTop";
            tbTop.Size = new Size(131, 23);
            tbTop.TabIndex = 11;
            tbTop.TextChanged += tbTop_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 235);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 14;
            label5.Text = "Width";
            // 
            // tbWidth
            // 
            tbWidth.Location = new Point(165, 226);
            tbWidth.Margin = new Padding(4, 3, 4, 3);
            tbWidth.Name = "tbWidth";
            tbWidth.Size = new Size(131, 23);
            tbWidth.TabIndex = 13;
            tbWidth.TextChanged += tbWidth_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(11, 265);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(43, 15);
            label6.TabIndex = 16;
            label6.Text = "Height";
            // 
            // tbHeight
            // 
            tbHeight.Location = new Point(165, 256);
            tbHeight.Margin = new Padding(4, 3, 4, 3);
            tbHeight.Name = "tbHeight";
            tbHeight.Size = new Size(131, 23);
            tbHeight.TabIndex = 15;
            tbHeight.TextChanged += tbHeight_TextChanged;
            // 
            // cbCustomWidth
            // 
            cbCustomWidth.AutoSize = true;
            cbCustomWidth.Location = new Point(346, 229);
            cbCustomWidth.Margin = new Padding(4, 3, 4, 3);
            cbCustomWidth.Name = "cbCustomWidth";
            cbCustomWidth.Size = new Size(68, 19);
            cbCustomWidth.TabIndex = 17;
            cbCustomWidth.Text = "Custom";
            cbCustomWidth.UseVisualStyleBackColor = true;
            cbCustomWidth.CheckedChanged += cbCustomWidth_CheckedChanged;
            cbCustomWidth.MouseHover += checkBox_MouseHover;
            // 
            // cbCustomHeight
            // 
            cbCustomHeight.AutoSize = true;
            cbCustomHeight.Location = new Point(346, 260);
            cbCustomHeight.Margin = new Padding(4, 3, 4, 3);
            cbCustomHeight.Name = "cbCustomHeight";
            cbCustomHeight.Size = new Size(68, 19);
            cbCustomHeight.TabIndex = 18;
            cbCustomHeight.Text = "Custom";
            cbCustomHeight.UseVisualStyleBackColor = true;
            cbCustomHeight.CheckedChanged += cbCustomHeight_CheckedChanged;
            cbCustomHeight.MouseHover += checkBox_MouseHover;
            // 
            // cbFullScreen
            // 
            cbFullScreen.AutoSize = true;
            cbFullScreen.Location = new Point(346, 291);
            cbFullScreen.Margin = new Padding(4, 3, 4, 3);
            cbFullScreen.Name = "cbFullScreen";
            cbFullScreen.Size = new Size(82, 19);
            cbFullScreen.TabIndex = 19;
            cbFullScreen.Text = "Full screen";
            cbFullScreen.UseVisualStyleBackColor = true;
            cbFullScreen.CheckedChanged += cbFullScreen_CheckedChanged;
            cbFullScreen.MouseHover += checkBox_MouseHover;
            // 
            // butCancel
            // 
            butCancel.DialogResult = DialogResult.Cancel;
            butCancel.Location = new Point(673, 512);
            butCancel.Margin = new Padding(4, 3, 4, 3);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(88, 27);
            butCancel.TabIndex = 20;
            butCancel.Text = "Minimize";
            butCancel.UseVisualStyleBackColor = true;
            butCancel.Click += butCancel_Click;
            // 
            // butEditScreens
            // 
            butEditScreens.Location = new Point(802, 379);
            butEditScreens.Margin = new Padding(4, 3, 4, 3);
            butEditScreens.Name = "butEditScreens";
            butEditScreens.Size = new Size(136, 27);
            butEditScreens.TabIndex = 33;
            butEditScreens.Text = "Edit screens";
            butEditScreens.UseVisualStyleBackColor = true;
            butEditScreens.Click += butEditScreens_Click;
            // 
            // cbSearchExe
            // 
            cbSearchExe.AutoSize = true;
            cbSearchExe.Location = new Point(128, 146);
            cbSearchExe.Margin = new Padding(4, 3, 4, 3);
            cbSearchExe.Name = "cbSearchExe";
            cbSearchExe.Size = new Size(15, 14);
            cbSearchExe.TabIndex = 36;
            cbSearchExe.UseVisualStyleBackColor = true;
            cbSearchExe.CheckedChanged += cbSearchExe_CheckedChanged;
            // 
            // cbSearchTitle
            // 
            cbSearchTitle.AutoSize = true;
            cbSearchTitle.Location = new Point(128, 89);
            cbSearchTitle.Margin = new Padding(4, 3, 4, 3);
            cbSearchTitle.Name = "cbSearchTitle";
            cbSearchTitle.Size = new Size(15, 14);
            cbSearchTitle.TabIndex = 37;
            cbSearchTitle.UseVisualStyleBackColor = true;
            cbSearchTitle.CheckedChanged += cbSearchTitle_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbHotKeyCharacter);
            groupBox1.Controls.Add(cbHotKeyRight);
            groupBox1.Controls.Add(cbHotKeyLeft);
            groupBox1.Location = new Point(463, 423);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(298, 61);
            groupBox1.TabIndex = 38;
            groupBox1.TabStop = false;
            groupBox1.Text = "Hotkey";
            // 
            // tbHotKeyCharacter
            // 
            tbHotKeyCharacter.CharacterCasing = CharacterCasing.Upper;
            tbHotKeyCharacter.Location = new Point(231, 22);
            tbHotKeyCharacter.Margin = new Padding(4, 3, 4, 3);
            tbHotKeyCharacter.MaxLength = 1;
            tbHotKeyCharacter.Name = "tbHotKeyCharacter";
            tbHotKeyCharacter.Size = new Size(53, 23);
            tbHotKeyCharacter.TabIndex = 2;
            tbHotKeyCharacter.TextAlign = HorizontalAlignment.Center;
            tbHotKeyCharacter.TextChanged += tbHotKeyCharacter_TextChanged;
            // 
            // cbHotKeyRight
            // 
            cbHotKeyRight.FormattingEnabled = true;
            cbHotKeyRight.Items.AddRange(new object[] { "None" });
            cbHotKeyRight.Location = new Point(120, 22);
            cbHotKeyRight.Margin = new Padding(4, 3, 4, 3);
            cbHotKeyRight.Name = "cbHotKeyRight";
            cbHotKeyRight.Size = new Size(103, 23);
            cbHotKeyRight.TabIndex = 1;
            cbHotKeyRight.SelectedIndexChanged += cbHotKeyRight_SelectedIndexChanged;
            // 
            // cbHotKeyLeft
            // 
            cbHotKeyLeft.FormattingEnabled = true;
            cbHotKeyLeft.Items.AddRange(new object[] { "Alt", "Ctrl", "Shift" });
            cbHotKeyLeft.Location = new Point(9, 22);
            cbHotKeyLeft.Margin = new Padding(4, 3, 4, 3);
            cbHotKeyLeft.Name = "cbHotKeyLeft";
            cbHotKeyLeft.Size = new Size(103, 23);
            cbHotKeyLeft.TabIndex = 0;
            cbHotKeyLeft.SelectedIndexChanged += cbHotKeyLeft_SelectedIndexChanged;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "WinSize4";
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(10, 30);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(39, 15);
            label7.TabIndex = 40;
            label7.Text = "Name";
            // 
            // tbName
            // 
            tbName.Location = new Point(164, 22);
            tbName.Margin = new Padding(4, 3, 4, 3);
            tbName.Name = "tbName";
            tbName.Size = new Size(305, 23);
            tbName.TabIndex = 39;
            // 
            // cbResetIfNewScreen
            // 
            cbResetIfNewScreen.AutoSize = true;
            cbResetIfNewScreen.Location = new Point(472, 353);
            cbResetIfNewScreen.Margin = new Padding(4, 3, 4, 3);
            cbResetIfNewScreen.Name = "cbResetIfNewScreen";
            cbResetIfNewScreen.Size = new Size(226, 19);
            cbResetIfNewScreen.TabIndex = 41;
            cbResetIfNewScreen.Text = "Reset moved if new screen is detected";
            cbResetIfNewScreen.UseVisualStyleBackColor = true;
            cbResetIfNewScreen.CheckedChanged += cbResetIfNewScreen_CheckedChanged;
            cbResetIfNewScreen.MouseHover += checkBox_MouseHover;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tbWindowClass);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(cbWindowClass);
            groupBox2.Controls.Add(cbIgnoreChildWindows);
            groupBox2.Controls.Add(radioStartsWith);
            groupBox2.Controls.Add(radioContains);
            groupBox2.Controls.Add(radioFull);
            groupBox2.Controls.Add(tbName);
            groupBox2.Controls.Add(tbTitle);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(tbExe);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(cbSearchTitle);
            groupBox2.Controls.Add(butRemove);
            groupBox2.Controls.Add(tbLeft);
            groupBox2.Controls.Add(cbSearchExe);
            groupBox2.Controls.Add(tbTop);
            groupBox2.Controls.Add(tbWidth);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(tbHeight);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(cbCustomWidth);
            groupBox2.Controls.Add(cbFullScreen);
            groupBox2.Controls.Add(cbCustomHeight);
            groupBox2.Location = new Point(463, 14);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(484, 333);
            groupBox2.TabIndex = 42;
            groupBox2.TabStop = false;
            groupBox2.Text = "Selected window";
            // 
            // tbWindowClass
            // 
            tbWindowClass.Location = new Point(164, 51);
            tbWindowClass.Margin = new Padding(4, 3, 4, 3);
            tbWindowClass.Name = "tbWindowClass";
            tbWindowClass.ReadOnly = true;
            tbWindowClass.Size = new Size(305, 23);
            tbWindowClass.TabIndex = 45;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(10, 59);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(81, 15);
            label8.TabIndex = 46;
            label8.Text = "Window Class";
            // 
            // cbWindowClass
            // 
            cbWindowClass.AutoSize = true;
            cbWindowClass.Location = new Point(128, 60);
            cbWindowClass.Margin = new Padding(4, 3, 4, 3);
            cbWindowClass.Name = "cbWindowClass";
            cbWindowClass.Size = new Size(15, 14);
            cbWindowClass.TabIndex = 47;
            cbWindowClass.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreChildWindows
            // 
            cbIgnoreChildWindows.AutoSize = true;
            cbIgnoreChildWindows.Location = new Point(165, 291);
            cbIgnoreChildWindows.Margin = new Padding(4, 3, 4, 3);
            cbIgnoreChildWindows.Name = "cbIgnoreChildWindows";
            cbIgnoreChildWindows.Size = new Size(143, 19);
            cbIgnoreChildWindows.TabIndex = 44;
            cbIgnoreChildWindows.Text = "Ignore Child Windows";
            cbIgnoreChildWindows.UseVisualStyleBackColor = true;
            // 
            // radioStartsWith
            // 
            radioStartsWith.AutoSize = true;
            radioStartsWith.Location = new Point(344, 110);
            radioStartsWith.Margin = new Padding(4, 3, 4, 3);
            radioStartsWith.Name = "radioStartsWith";
            radioStartsWith.Size = new Size(80, 19);
            radioStartsWith.TabIndex = 43;
            radioStartsWith.Text = "Starts with";
            radioStartsWith.UseVisualStyleBackColor = true;
            // 
            // radioContains
            // 
            radioContains.AutoSize = true;
            radioContains.Location = new Point(260, 110);
            radioContains.Margin = new Padding(4, 3, 4, 3);
            radioContains.Name = "radioContains";
            radioContains.Size = new Size(72, 19);
            radioContains.TabIndex = 42;
            radioContains.Text = "Contains";
            radioContains.UseVisualStyleBackColor = true;
            // 
            // radioFull
            // 
            radioFull.AutoSize = true;
            radioFull.Location = new Point(206, 110);
            radioFull.Margin = new Padding(4, 3, 4, 3);
            radioFull.Name = "radioFull";
            radioFull.Size = new Size(44, 19);
            radioFull.TabIndex = 41;
            radioFull.Text = "Full";
            radioFull.UseVisualStyleBackColor = true;
            // 
            // butResetMoved
            // 
            butResetMoved.Location = new Point(460, 379);
            butResetMoved.Margin = new Padding(4, 3, 4, 3);
            butResetMoved.Name = "butResetMoved";
            butResetMoved.Size = new Size(136, 27);
            butResetMoved.TabIndex = 43;
            butResetMoved.Text = "Reset moved";
            butResetMoved.UseVisualStyleBackColor = true;
            butResetMoved.Click += butResetMoved_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listView1.Location = new Point(14, 14);
            listView1.Margin = new Padding(4, 3, 4, 3);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(432, 498);
            listView1.Sorting = SortOrder.Ascending;
            listView1.TabIndex = 44;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.KeyDown += listView1_KeyDown;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 165;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Width";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Height";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Primary";
            // 
            // cbShowAllWindows
            // 
            cbShowAllWindows.AutoSize = true;
            cbShowAllWindows.Location = new Point(14, 519);
            cbShowAllWindows.Margin = new Padding(4, 3, 4, 3);
            cbShowAllWindows.Name = "cbShowAllWindows";
            cbShowAllWindows.Size = new Size(153, 19);
            cbShowAllWindows.TabIndex = 45;
            cbShowAllWindows.Text = "Show all saved windows";
            cbShowAllWindows.UseVisualStyleBackColor = true;
            cbShowAllWindows.CheckedChanged += cbShowAllWindows_CheckedChanged;
            // 
            // cbRunAtLogin
            // 
            cbRunAtLogin.AutoSize = true;
            cbRunAtLogin.Location = new Point(472, 487);
            cbRunAtLogin.Margin = new Padding(4, 3, 4, 3);
            cbRunAtLogin.Name = "cbRunAtLogin";
            cbRunAtLogin.Size = new Size(90, 19);
            cbRunAtLogin.TabIndex = 46;
            cbRunAtLogin.Text = "Run at login";
            cbRunAtLogin.UseVisualStyleBackColor = true;
            cbRunAtLogin.CheckedChanged += cbRunAtLogin_CheckedChanged;
            // 
            // frmMain
            // 
            AcceptButton = butOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = butCancel;
            ClientSize = new Size(962, 550);
            Controls.Add(cbRunAtLogin);
            Controls.Add(cbShowAllWindows);
            Controls.Add(listView1);
            Controls.Add(butResetMoved);
            Controls.Add(groupBox2);
            Controls.Add(cbResetIfNewScreen);
            Controls.Add(groupBox1);
            Controls.Add(butEditScreens);
            Controls.Add(butCancel);
            Controls.Add(butExit);
            Controls.Add(butApply);
            Controls.Add(butOK);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmMain";
            Text = "WinSize4";
            FormClosing += frmMain_FormClosing;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbExe;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butApply;
        private System.Windows.Forms.Button butExit;
        private System.Windows.Forms.Button butRemove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLeft;
        private System.Windows.Forms.TextBox tbTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.CheckBox cbCustomWidth;
        private System.Windows.Forms.CheckBox cbCustomHeight;
        private System.Windows.Forms.CheckBox cbFullScreen;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butEditScreens;
        private System.Windows.Forms.CheckBox cbSearchExe;
        private System.Windows.Forms.CheckBox cbSearchTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbHotKeyLeft;
        private System.Windows.Forms.TextBox tbHotKeyCharacter;
        private System.Windows.Forms.ComboBox cbHotKeyRight;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cbResetIfNewScreen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butResetMoved;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.RadioButton radioStartsWith;
        private System.Windows.Forms.RadioButton radioContains;
        private System.Windows.Forms.RadioButton radioFull;
        private System.Windows.Forms.CheckBox cbShowAllWindows;
        private CheckBox cbIgnoreChildWindows;
        private CheckBox cbRunAtLogin;
        private TextBox tbWindowClass;
        private Label label8;
        private CheckBox cbWindowClass;
    }
}

