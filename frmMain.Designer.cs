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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbExe = new System.Windows.Forms.TextBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butApply = new System.Windows.Forms.Button();
            this.butExit = new System.Windows.Forms.Button();
            this.butRemove = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLeft = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTop = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.cbCustomWidth = new System.Windows.Forms.CheckBox();
            this.cbCustomHeight = new System.Windows.Forms.CheckBox();
            this.cbFullScreen = new System.Windows.Forms.CheckBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.butEditScreens = new System.Windows.Forms.Button();
            this.cbSearchExe = new System.Windows.Forms.CheckBox();
            this.cbSearchTitle = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbHotKeyCharacter = new System.Windows.Forms.TextBox();
            this.cbHotKeyRight = new System.Windows.Forms.ComboBox();
            this.cbHotKeyLeft = new System.Windows.Forms.ComboBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbResetIfNewScreen = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioStartsWith = new System.Windows.Forms.RadioButton();
            this.radioContains = new System.Windows.Forms.RadioButton();
            this.radioFull = new System.Windows.Forms.RadioButton();
            this.butResetMoved = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbShowAllWindows = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(147, 45);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(262, 20);
            this.tbTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Window Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Executable";
            // 
            // tbExe
            // 
            this.tbExe.Location = new System.Drawing.Point(148, 94);
            this.tbExe.Name = "tbExe";
            this.tbExe.Size = new System.Drawing.Size(262, 20);
            this.tbExe.TabIndex = 3;
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(496, 444);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 5;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butApply
            // 
            this.butApply.Location = new System.Drawing.Point(658, 444);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(75, 23);
            this.butApply.TabIndex = 6;
            this.butApply.Text = "Apply";
            this.butApply.UseVisualStyleBackColor = true;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // butExit
            // 
            this.butExit.Location = new System.Drawing.Point(739, 444);
            this.butExit.Name = "butExit";
            this.butExit.Size = new System.Drawing.Size(75, 23);
            this.butExit.TabIndex = 7;
            this.butExit.Text = "Exit";
            this.butExit.UseVisualStyleBackColor = true;
            this.butExit.Click += new System.EventHandler(this.butExit_Click);
            // 
            // butRemove
            // 
            this.butRemove.Location = new System.Drawing.Point(8, 224);
            this.butRemove.Name = "butRemove";
            this.butRemove.Size = new System.Drawing.Size(117, 23);
            this.butRemove.TabIndex = 8;
            this.butRemove.Text = "Remove";
            this.butRemove.UseVisualStyleBackColor = true;
            this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Left";
            // 
            // tbLeft
            // 
            this.tbLeft.Location = new System.Drawing.Point(148, 120);
            this.tbLeft.Name = "tbLeft";
            this.tbLeft.Size = new System.Drawing.Size(113, 20);
            this.tbLeft.TabIndex = 9;
            this.tbLeft.TextChanged += new System.EventHandler(this.tbLeft_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Top";
            // 
            // tbTop
            // 
            this.tbTop.Location = new System.Drawing.Point(148, 146);
            this.tbTop.Name = "tbTop";
            this.tbTop.Size = new System.Drawing.Size(113, 20);
            this.tbTop.TabIndex = 11;
            this.tbTop.TextChanged += new System.EventHandler(this.tbTop_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Width";
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(148, 172);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(113, 20);
            this.tbWidth.TabIndex = 13;
            this.tbWidth.TextChanged += new System.EventHandler(this.tbWidth_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Height";
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(148, 198);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(113, 20);
            this.tbHeight.TabIndex = 15;
            this.tbHeight.TextChanged += new System.EventHandler(this.tbHeight_TextChanged);
            // 
            // cbCustomWidth
            // 
            this.cbCustomWidth.AutoSize = true;
            this.cbCustomWidth.Location = new System.Drawing.Point(303, 174);
            this.cbCustomWidth.Name = "cbCustomWidth";
            this.cbCustomWidth.Size = new System.Drawing.Size(61, 17);
            this.cbCustomWidth.TabIndex = 17;
            this.cbCustomWidth.Text = "Custom";
            this.cbCustomWidth.UseVisualStyleBackColor = true;
            this.cbCustomWidth.CheckedChanged += new System.EventHandler(this.cbCustomWidth_CheckedChanged);
            this.cbCustomWidth.MouseHover += new System.EventHandler(this.checkBox_MouseHover);
            // 
            // cbCustomHeight
            // 
            this.cbCustomHeight.AutoSize = true;
            this.cbCustomHeight.Location = new System.Drawing.Point(303, 201);
            this.cbCustomHeight.Name = "cbCustomHeight";
            this.cbCustomHeight.Size = new System.Drawing.Size(61, 17);
            this.cbCustomHeight.TabIndex = 18;
            this.cbCustomHeight.Text = "Custom";
            this.cbCustomHeight.UseVisualStyleBackColor = true;
            this.cbCustomHeight.CheckedChanged += new System.EventHandler(this.cbCustomHeight_CheckedChanged);
            this.cbCustomHeight.MouseHover += new System.EventHandler(this.checkBox_MouseHover);
            // 
            // cbFullScreen
            // 
            this.cbFullScreen.AutoSize = true;
            this.cbFullScreen.Location = new System.Drawing.Point(303, 228);
            this.cbFullScreen.Name = "cbFullScreen";
            this.cbFullScreen.Size = new System.Drawing.Size(77, 17);
            this.cbFullScreen.TabIndex = 19;
            this.cbFullScreen.Text = "Full screen";
            this.cbFullScreen.UseVisualStyleBackColor = true;
            this.cbFullScreen.CheckedChanged += new System.EventHandler(this.cbFullScreen_CheckedChanged);
            this.cbFullScreen.MouseHover += new System.EventHandler(this.checkBox_MouseHover);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(577, 444);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 20;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butEditScreens
            // 
            this.butEditScreens.Location = new System.Drawing.Point(687, 301);
            this.butEditScreens.Name = "butEditScreens";
            this.butEditScreens.Size = new System.Drawing.Size(117, 23);
            this.butEditScreens.TabIndex = 33;
            this.butEditScreens.Text = "Edit screens";
            this.butEditScreens.UseVisualStyleBackColor = true;
            this.butEditScreens.Click += new System.EventHandler(this.butEditScreens_Click);
            // 
            // cbSearchExe
            // 
            this.cbSearchExe.AutoSize = true;
            this.cbSearchExe.Location = new System.Drawing.Point(118, 97);
            this.cbSearchExe.Name = "cbSearchExe";
            this.cbSearchExe.Size = new System.Drawing.Size(15, 14);
            this.cbSearchExe.TabIndex = 36;
            this.cbSearchExe.UseVisualStyleBackColor = true;
            this.cbSearchExe.CheckedChanged += new System.EventHandler(this.cbSearchExe_CheckedChanged);
            // 
            // cbSearchTitle
            // 
            this.cbSearchTitle.AutoSize = true;
            this.cbSearchTitle.Location = new System.Drawing.Point(117, 48);
            this.cbSearchTitle.Name = "cbSearchTitle";
            this.cbSearchTitle.Size = new System.Drawing.Size(15, 14);
            this.cbSearchTitle.TabIndex = 37;
            this.cbSearchTitle.UseVisualStyleBackColor = true;
            this.cbSearchTitle.CheckedChanged += new System.EventHandler(this.cbSearchTitle_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbHotKeyCharacter);
            this.groupBox1.Controls.Add(this.cbHotKeyRight);
            this.groupBox1.Controls.Add(this.cbHotKeyLeft);
            this.groupBox1.Location = new System.Drawing.Point(388, 339);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 53);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hotkey";
            // 
            // tbHotKeyCharacter
            // 
            this.tbHotKeyCharacter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbHotKeyCharacter.Location = new System.Drawing.Point(196, 19);
            this.tbHotKeyCharacter.MaxLength = 1;
            this.tbHotKeyCharacter.Name = "tbHotKeyCharacter";
            this.tbHotKeyCharacter.Size = new System.Drawing.Size(46, 20);
            this.tbHotKeyCharacter.TabIndex = 2;
            this.tbHotKeyCharacter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbHotKeyCharacter.TextChanged += new System.EventHandler(this.tbHotKeyCharacter_TextChanged);
            // 
            // cbHotKeyRight
            // 
            this.cbHotKeyRight.FormattingEnabled = true;
            this.cbHotKeyRight.Items.AddRange(new object[] {
            "None"});
            this.cbHotKeyRight.Location = new System.Drawing.Point(101, 19);
            this.cbHotKeyRight.Name = "cbHotKeyRight";
            this.cbHotKeyRight.Size = new System.Drawing.Size(89, 21);
            this.cbHotKeyRight.TabIndex = 1;
            this.cbHotKeyRight.SelectedIndexChanged += new System.EventHandler(this.cbHotKeyRight_SelectedIndexChanged);
            // 
            // cbHotKeyLeft
            // 
            this.cbHotKeyLeft.FormattingEnabled = true;
            this.cbHotKeyLeft.Items.AddRange(new object[] {
            "Alt",
            "Ctrl",
            "Shift"});
            this.cbHotKeyLeft.Location = new System.Drawing.Point(6, 19);
            this.cbHotKeyLeft.Name = "cbHotKeyLeft";
            this.cbHotKeyLeft.Size = new System.Drawing.Size(89, 21);
            this.cbHotKeyLeft.TabIndex = 0;
            this.cbHotKeyLeft.SelectedIndexChanged += new System.EventHandler(this.cbHotKeyLeft_SelectedIndexChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipTitle = "WinSize4";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(147, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(262, 20);
            this.tbName.TabIndex = 39;
            // 
            // cbResetIfNewScreen
            // 
            this.cbResetIfNewScreen.AutoSize = true;
            this.cbResetIfNewScreen.Location = new System.Drawing.Point(405, 278);
            this.cbResetIfNewScreen.Name = "cbResetIfNewScreen";
            this.cbResetIfNewScreen.Size = new System.Drawing.Size(155, 17);
            this.cbResetIfNewScreen.TabIndex = 41;
            this.cbResetIfNewScreen.Text = "Reset moved if new screen";
            this.cbResetIfNewScreen.UseVisualStyleBackColor = true;
            this.cbResetIfNewScreen.CheckedChanged += new System.EventHandler(this.cbResetIfNewScreen_CheckedChanged);
            this.cbResetIfNewScreen.MouseHover += new System.EventHandler(this.checkBox_MouseHover);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioStartsWith);
            this.groupBox2.Controls.Add(this.radioContains);
            this.groupBox2.Controls.Add(this.radioFull);
            this.groupBox2.Controls.Add(this.tbName);
            this.groupBox2.Controls.Add(this.tbTitle);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbExe);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cbSearchTitle);
            this.groupBox2.Controls.Add(this.butRemove);
            this.groupBox2.Controls.Add(this.tbLeft);
            this.groupBox2.Controls.Add(this.cbSearchExe);
            this.groupBox2.Controls.Add(this.tbTop);
            this.groupBox2.Controls.Add(this.tbWidth);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbHeight);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cbCustomWidth);
            this.groupBox2.Controls.Add(this.cbFullScreen);
            this.groupBox2.Controls.Add(this.cbCustomHeight);
            this.groupBox2.Location = new System.Drawing.Point(397, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 260);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected window";
            // 
            // radioStartsWith
            // 
            this.radioStartsWith.AutoSize = true;
            this.radioStartsWith.Location = new System.Drawing.Point(302, 71);
            this.radioStartsWith.Name = "radioStartsWith";
            this.radioStartsWith.Size = new System.Drawing.Size(74, 17);
            this.radioStartsWith.TabIndex = 43;
            this.radioStartsWith.Text = "Starts with";
            this.radioStartsWith.UseVisualStyleBackColor = true;
            // 
            // radioContains
            // 
            this.radioContains.AutoSize = true;
            this.radioContains.Location = new System.Drawing.Point(230, 71);
            this.radioContains.Name = "radioContains";
            this.radioContains.Size = new System.Drawing.Size(66, 17);
            this.radioContains.TabIndex = 42;
            this.radioContains.Text = "Contains";
            this.radioContains.UseVisualStyleBackColor = true;
            // 
            // radioFull
            // 
            this.radioFull.AutoSize = true;
            this.radioFull.Location = new System.Drawing.Point(183, 71);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(41, 17);
            this.radioFull.TabIndex = 41;
            this.radioFull.Text = "Full";
            this.radioFull.UseVisualStyleBackColor = true;
            // 
            // butResetMoved
            // 
            this.butResetMoved.Location = new System.Drawing.Point(394, 301);
            this.butResetMoved.Name = "butResetMoved";
            this.butResetMoved.Size = new System.Drawing.Size(117, 23);
            this.butResetMoved.TabIndex = 43;
            this.butResetMoved.Text = "Reset moved";
            this.butResetMoved.UseVisualStyleBackColor = true;
            this.butResetMoved.Click += new System.EventHandler(this.butResetMoved_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(371, 432);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 44;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 165;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Width";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Height";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Primary";
            // 
            // cbShowAllWindows
            // 
            this.cbShowAllWindows.AutoSize = true;
            this.cbShowAllWindows.Location = new System.Drawing.Point(12, 450);
            this.cbShowAllWindows.Name = "cbShowAllWindows";
            this.cbShowAllWindows.Size = new System.Drawing.Size(142, 17);
            this.cbShowAllWindows.TabIndex = 45;
            this.cbShowAllWindows.Text = "Show all saved windows";
            this.cbShowAllWindows.UseVisualStyleBackColor = true;
            this.cbShowAllWindows.CheckedChanged += new System.EventHandler(this.cbShowAllWindows_CheckedChanged);
            // 
            // frmMain
            // 
            this.AcceptButton = this.butOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(825, 477);
            this.Controls.Add(this.cbShowAllWindows);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.butResetMoved);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbResetIfNewScreen);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.butEditScreens);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butExit);
            this.Controls.Add(this.butApply);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "WinSize4";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

