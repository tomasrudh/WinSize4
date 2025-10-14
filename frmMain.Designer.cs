﻿namespace WinSize4
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
            label2 = new Label();
            tbExe = new TextBox();
            butOK = new Button();
            butApply = new Button();
            butExit = new Button();
            butRemove = new Button();
            label3 = new Label();
            label4 = new Label();
            tbTop = new NumericUpDown();
            label5 = new Label();
            tbWidth = new NumericUpDown();
            label6 = new Label();
            tbHeight = new NumericUpDown();
            cbCustomWidth = new CheckBox();
            cbCustomHeight = new CheckBox();
            cbFullScreen = new CheckBox();
            butCancel = new Button();
            butEditScreens = new Button();
            cbSearchExe = new CheckBox();
            groupBox1 = new GroupBox();
            tbHotKeyCharacter = new TextBox();
            cbHotKeyRight = new ComboBox();
            cbHotKeyLeft = new ComboBox();
            notifyIcon1 = new NotifyIcon(components);
            label7 = new Label();
            tbName = new TextBox();
            cbResetIfNewScreen = new CheckBox();
            groupBox2 = new GroupBox();
            tbLeft = new NumericUpDown();
            cbCanResize = new CheckBox();
            cbIgnoreChildWindows = new CheckBox();
            groupBox4 = new GroupBox();
            radioStartsWithInclude = new RadioButton();
            radioContainsInclude = new RadioButton();
            radioFullInclude = new RadioButton();
            tbTitleInclude = new TextBox();
            cbSearchTitleInclude = new CheckBox();
            groupBox3 = new GroupBox();
            radioStartsWithExclude = new RadioButton();
            radioContainsExclude = new RadioButton();
            radioFullExclude = new RadioButton();
            tbTitleExclude = new TextBox();
            cbSearchTitleExclude = new CheckBox();
            tbSavedWindowIndex = new Label();
            tbWindowClass = new TextBox();
            label8 = new Label();
            cbWindowClass = new CheckBox();
            cbAlwaysMove = new CheckBox();
            butDuplicate = new Button();
            butResetMoved = new Button();
            toolTip1 = new ToolTip(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            stateImageList = new ImageList(components);
            cbShowAllWindows = new CheckBox();
            cbRunAtLogin = new CheckBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            txtVersion = new TextBox();
            cbIsPaused = new CheckBox();
            tbTop = new NumericUpDown();
            tbWidth = new NumericUpDown();
            tbHeight = new NumericUpDown();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbLeft).BeginInit();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbTop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbHeight).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 287);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 4;
            label2.Text = "Executable";
            // 
            // tbExe
            // 
            tbExe.Location = new Point(168, 278);
            tbExe.Margin = new Padding(4, 3, 4, 3);
            tbExe.Name = "tbExe";
            tbExe.Size = new Size(305, 23);
            tbExe.TabIndex = 3;
            // 
            // butOK
            // 
            butOK.Location = new Point(540, 633);
            butOK.Margin = new Padding(4, 3, 4, 3);
            butOK.Name = "butOK";
            butOK.Size = new Size(124, 27);
            butOK.TabIndex = 5;
            butOK.Text = "Apply and Hide";
            butOK.UseVisualStyleBackColor = true;
            butOK.Click += butOK_Click;
            // 
            // butApply
            // 
            butApply.Location = new Point(765, 633);
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
            butExit.Location = new Point(859, 633);
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
            butRemove.Location = new Point(12, 428);
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
            label3.Location = new Point(14, 312);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(27, 15);
            label3.TabIndex = 10;
            label3.Text = "Left";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 342);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(27, 15);
            label4.TabIndex = 12;
            label4.Text = "Top";
            // 
            // tbTop
            // 
            tbTop.Location = new Point(167, 339);
            tbTop.Margin = new Padding(4, 3, 4, 3);
            tbTop.Name = "tbTop";
            tbTop.Size = new Size(131, 23);
            tbTop.TabIndex = 11;
            tbTop.TextChanged += tbTop_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 371);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 14;
            label5.Text = "Width";
            // 
            // tbWidth
            // 
            tbWidth.Location = new Point(168, 368);
            tbWidth.Margin = new Padding(4, 3, 4, 3);
            tbWidth.Name = "tbWidth";
            tbWidth.Size = new Size(131, 23);
            tbWidth.TabIndex = 13;
            tbWidth.TextChanged += tbWidth_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 401);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(43, 15);
            label6.TabIndex = 16;
            label6.Text = "Height";
            // 
            // tbHeight
            // 
            tbHeight.Location = new Point(168, 398);
            tbHeight.Margin = new Padding(4, 3, 4, 3);
            tbHeight.Name = "tbHeight";
            tbHeight.Size = new Size(131, 23);
            tbHeight.TabIndex = 15;
            tbHeight.TextChanged += tbHeight_TextChanged;
            // 
            // cbCustomWidth
            // 
            cbCustomWidth.AutoSize = true;
            cbCustomWidth.Location = new Point(330, 370);
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
            cbCustomHeight.Location = new Point(330, 400);
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
            cbFullScreen.Location = new Point(17, 461);
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
            butCancel.Location = new Point(670, 633);
            butCancel.Margin = new Padding(4, 3, 4, 3);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(88, 27);
            butCancel.TabIndex = 20;
            butCancel.Text = "Hide";
            butCancel.UseVisualStyleBackColor = true;
            butCancel.Click += butCancel_Click;
            // 
            // butEditScreens
            // 
            butEditScreens.Location = new Point(811, 563);
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
            cbSearchExe.Location = new Point(131, 288);
            cbSearchExe.Margin = new Padding(4, 3, 4, 3);
            cbSearchExe.Name = "cbSearchExe";
            cbSearchExe.Size = new Size(15, 14);
            cbSearchExe.TabIndex = 36;
            cbSearchExe.UseVisualStyleBackColor = true;
            cbSearchExe.CheckedChanged += cbSearchExe_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbHotKeyCharacter);
            groupBox1.Controls.Add(cbHotKeyRight);
            groupBox1.Controls.Add(cbHotKeyLeft);
            groupBox1.Location = new Point(463, 541);
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
            cbResetIfNewScreen.Location = new Point(480, 516);
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
            groupBox2.Controls.Add(tbLeft);
            groupBox2.Controls.Add(cbCanResize);
            groupBox2.Controls.Add(cbIgnoreChildWindows);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(tbSavedWindowIndex);
            groupBox2.Controls.Add(tbWindowClass);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(cbWindowClass);
            groupBox2.Controls.Add(cbAlwaysMove);
            groupBox2.Controls.Add(tbName);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(tbExe);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(butRemove);
            groupBox2.Controls.Add(cbSearchExe);
            groupBox2.Controls.Add(tbTop);
            groupBox2.Controls.Add(tbWidth);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(tbHeight);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(cbCustomWidth);
            groupBox2.Controls.Add(cbFullScreen);
            groupBox2.Controls.Add(cbCustomHeight);
            groupBox2.Controls.Add(butDuplicate);
            groupBox2.Location = new Point(463, 14);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(484, 492);
            groupBox2.TabIndex = 42;
            groupBox2.TabStop = false;
            groupBox2.Text = "Selected window";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // tbLeft
            // 
            tbLeft.Location = new Point(168, 310);
            tbLeft.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            tbLeft.Name = "tbLeft";
            tbLeft.Size = new Size(120, 23);
            tbLeft.TabIndex = 60;
            // 
            // cbCanResize
            // 
            cbCanResize.AutoSize = true;
            cbCanResize.Location = new Point(131, 387);
            cbCanResize.Margin = new Padding(4, 3, 4, 3);
            cbCanResize.Name = "cbCanResize";
            cbCanResize.Size = new Size(15, 14);
            cbCanResize.TabIndex = 59;
            cbCanResize.UseVisualStyleBackColor = true;
            cbCanResize.CheckedChanged += cbCanResize_CheckedChanged;
            // 
            // cbIgnoreChildWindows
            // 
            cbIgnoreChildWindows.AutoSize = true;
            cbIgnoreChildWindows.Location = new Point(152, 461);
            cbIgnoreChildWindows.Name = "cbIgnoreChildWindows";
            cbIgnoreChildWindows.Size = new Size(143, 19);
            cbIgnoreChildWindows.TabIndex = 58;
            cbIgnoreChildWindows.Text = "Ignore Child Windows";
            cbIgnoreChildWindows.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(radioStartsWithInclude);
            groupBox4.Controls.Add(radioContainsInclude);
            groupBox4.Controls.Add(radioFullInclude);
            groupBox4.Controls.Add(tbTitleInclude);
            groupBox4.Controls.Add(cbSearchTitleInclude);
            groupBox4.Location = new Point(10, 84);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(467, 92);
            groupBox4.TabIndex = 56;
            groupBox4.TabStop = false;
            groupBox4.Text = "Window Title Include";
            // 
            // radioStartsWithInclude
            // 
            radioStartsWithInclude.AutoSize = true;
            radioStartsWithInclude.Location = new Point(162, 60);
            radioStartsWithInclude.Margin = new Padding(4, 3, 4, 3);
            radioStartsWithInclude.Name = "radioStartsWithInclude";
            radioStartsWithInclude.Size = new Size(80, 19);
            radioStartsWithInclude.TabIndex = 49;
            radioStartsWithInclude.Text = "Starts with";
            radioStartsWithInclude.UseVisualStyleBackColor = true;
            // 
            // radioContainsInclude
            // 
            radioContainsInclude.AutoSize = true;
            radioContainsInclude.Location = new Point(82, 60);
            radioContainsInclude.Margin = new Padding(4, 3, 4, 3);
            radioContainsInclude.Name = "radioContainsInclude";
            radioContainsInclude.Size = new Size(72, 19);
            radioContainsInclude.TabIndex = 48;
            radioContainsInclude.Text = "Contains";
            radioContainsInclude.UseVisualStyleBackColor = true;
            // 
            // radioFullInclude
            // 
            radioFullInclude.AutoSize = true;
            radioFullInclude.Location = new Point(30, 60);
            radioFullInclude.Margin = new Padding(4, 3, 4, 3);
            radioFullInclude.Name = "radioFullInclude";
            radioFullInclude.Size = new Size(44, 19);
            radioFullInclude.TabIndex = 47;
            radioFullInclude.Text = "Full";
            radioFullInclude.UseVisualStyleBackColor = true;
            // 
            // tbTitleInclude
            // 
            tbTitleInclude.Location = new Point(30, 31);
            tbTitleInclude.Margin = new Padding(4, 3, 4, 3);
            tbTitleInclude.Name = "tbTitleInclude";
            tbTitleInclude.Size = new Size(433, 23);
            tbTitleInclude.TabIndex = 44;
            tbTitleInclude.TextChanged += tbTitleInclude_TextChanged;
            // 
            // cbSearchTitleInclude
            // 
            cbSearchTitleInclude.AutoSize = true;
            cbSearchTitleInclude.Location = new Point(7, 35);
            cbSearchTitleInclude.Margin = new Padding(4, 3, 4, 3);
            cbSearchTitleInclude.Name = "cbSearchTitleInclude";
            cbSearchTitleInclude.Size = new Size(15, 14);
            cbSearchTitleInclude.TabIndex = 46;
            cbSearchTitleInclude.UseVisualStyleBackColor = true;
            cbSearchTitleInclude.CheckedChanged += cbSearchTitleInclude_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioStartsWithExclude);
            groupBox3.Controls.Add(radioContainsExclude);
            groupBox3.Controls.Add(radioFullExclude);
            groupBox3.Controls.Add(tbTitleExclude);
            groupBox3.Controls.Add(cbSearchTitleExclude);
            groupBox3.Location = new Point(10, 182);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(467, 90);
            groupBox3.TabIndex = 55;
            groupBox3.TabStop = false;
            groupBox3.Text = "Window Title Exclude";
            // 
            // radioStartsWithExclude
            // 
            radioStartsWithExclude.AutoSize = true;
            radioStartsWithExclude.Location = new Point(162, 56);
            radioStartsWithExclude.Margin = new Padding(4, 3, 4, 3);
            radioStartsWithExclude.Name = "radioStartsWithExclude";
            radioStartsWithExclude.Size = new Size(80, 19);
            radioStartsWithExclude.TabIndex = 60;
            radioStartsWithExclude.Text = "Starts with";
            radioStartsWithExclude.UseVisualStyleBackColor = true;
            // 
            // radioContainsExclude
            // 
            radioContainsExclude.AutoSize = true;
            radioContainsExclude.Location = new Point(82, 56);
            radioContainsExclude.Margin = new Padding(4, 3, 4, 3);
            radioContainsExclude.Name = "radioContainsExclude";
            radioContainsExclude.Size = new Size(72, 19);
            radioContainsExclude.TabIndex = 59;
            radioContainsExclude.Text = "Contains";
            radioContainsExclude.UseVisualStyleBackColor = true;
            // 
            // radioFullExclude
            // 
            radioFullExclude.AutoSize = true;
            radioFullExclude.Location = new Point(30, 56);
            radioFullExclude.Margin = new Padding(4, 3, 4, 3);
            radioFullExclude.Name = "radioFullExclude";
            radioFullExclude.Size = new Size(44, 19);
            radioFullExclude.TabIndex = 58;
            radioFullExclude.Text = "Full";
            radioFullExclude.UseVisualStyleBackColor = true;
            // 
            // tbTitleExclude
            // 
            tbTitleExclude.Location = new Point(30, 27);
            tbTitleExclude.Margin = new Padding(4, 3, 4, 3);
            tbTitleExclude.Name = "tbTitleExclude";
            tbTitleExclude.Size = new Size(433, 23);
            tbTitleExclude.TabIndex = 55;
            tbTitleExclude.TextChanged += tbTitleExclude_TextChanged;
            // 
            // cbSearchTitleExclude
            // 
            cbSearchTitleExclude.AutoSize = true;
            cbSearchTitleExclude.Location = new Point(7, 31);
            cbSearchTitleExclude.Margin = new Padding(4, 3, 4, 3);
            cbSearchTitleExclude.Name = "cbSearchTitleExclude";
            cbSearchTitleExclude.Size = new Size(15, 14);
            cbSearchTitleExclude.TabIndex = 57;
            cbSearchTitleExclude.UseVisualStyleBackColor = true;
            cbSearchTitleExclude.CheckedChanged += cbSearchTitleExclude_CheckedChanged;
            // 
            // tbSavedWindowIndex
            // 
            tbSavedWindowIndex.AutoSize = true;
            tbSavedWindowIndex.Location = new Point(120, 30);
            tbSavedWindowIndex.Margin = new Padding(4, 0, 4, 0);
            tbSavedWindowIndex.Name = "tbSavedWindowIndex";
            tbSavedWindowIndex.Size = new Size(13, 15);
            tbSavedWindowIndex.TabIndex = 48;
            tbSavedWindowIndex.Text = "0";
            // 
            // tbWindowClass
            // 
            tbWindowClass.Location = new Point(164, 51);
            tbWindowClass.Margin = new Padding(4, 3, 4, 3);
            tbWindowClass.Name = "tbWindowClass";
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
            cbWindowClass.CheckedChanged += cbWindowClass_CheckedChanged;
            // 
            // cbAlwaysMove
            // 
            cbAlwaysMove.AutoSize = true;
            cbAlwaysMove.Location = new Point(330, 461);
            cbAlwaysMove.Margin = new Padding(4, 3, 4, 3);
            cbAlwaysMove.Name = "cbAlwaysMove";
            cbAlwaysMove.Size = new Size(96, 19);
            cbAlwaysMove.TabIndex = 44;
            cbAlwaysMove.Text = "Always move";
            cbAlwaysMove.UseVisualStyleBackColor = true;
            // 
            // butDuplicate
            // 
            butDuplicate.Location = new Point(164, 428);
            butDuplicate.Margin = new Padding(4, 3, 4, 3);
            butDuplicate.Name = "butDuplicate";
            butDuplicate.Size = new Size(136, 27);
            butDuplicate.TabIndex = 57;
            butDuplicate.Text = "Duplicate";
            butDuplicate.UseVisualStyleBackColor = true;
            butDuplicate.Click += butDuplicate_Click;
            // 
            // butResetMoved
            // 
            butResetMoved.Location = new Point(811, 516);
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
            listView1.Size = new Size(432, 613);
            listView1.Sorting = SortOrder.Ascending;
            listView1.StateImageList = stateImageList;
            listView1.TabIndex = 44;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.KeyDown += listView1_KeyDown;
            listView1.MouseClick += listView1_MouseClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 245;
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
            // stateImageList
            // 
            stateImageList.ColorDepth = ColorDepth.Depth8Bit;
            stateImageList.ImageStream = (ImageListStreamer)resources.GetObject("stateImageList.ImageStream");
            stateImageList.TransparentColor = Color.Transparent;
            stateImageList.Images.SetKeyName(0, "green-checkmark-icon-16.png");
            stateImageList.Images.SetKeyName(1, "red-x-icon-16.png");
            // 
            // cbShowAllWindows
            // 
            cbShowAllWindows.AutoSize = true;
            cbShowAllWindows.Location = new Point(14, 641);
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
            cbRunAtLogin.Location = new Point(469, 608);
            cbRunAtLogin.Margin = new Padding(4, 3, 4, 3);
            cbRunAtLogin.Name = "cbRunAtLogin";
            cbRunAtLogin.Size = new Size(90, 19);
            cbRunAtLogin.TabIndex = 46;
            cbRunAtLogin.Text = "Run at login";
            cbRunAtLogin.UseVisualStyleBackColor = true;
            cbRunAtLogin.CheckedChanged += cbRunAtLogin_CheckedChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // txtVersion
            // 
            txtVersion.BorderStyle = BorderStyle.None;
            txtVersion.ForeColor = Color.Black;
            txtVersion.Location = new Point(346, 633);
            txtVersion.Name = "txtVersion";
            txtVersion.ReadOnly = true;
            txtVersion.Size = new Size(100, 16);
            txtVersion.TabIndex = 47;
            txtVersion.TextAlign = HorizontalAlignment.Right;
            // 
            // cbIsPaused
            // 
            cbIsPaused.AutoSize = true;
            cbIsPaused.Location = new Point(594, 608);
            cbIsPaused.Name = "cbIsPaused";
            cbIsPaused.Size = new Size(57, 19);
            cbIsPaused.TabIndex = 48;
            cbIsPaused.Text = "Pause";
            cbIsPaused.UseVisualStyleBackColor = true;
            cbIsPaused.CheckedChanged += cbIsPaused_CheckedChanged;
            // 
            // tbTop
            // 
            tbTop.Location = new Point(168, 340);
            tbTop.Name = "tbTop";
            tbTop.Size = new Size(120, 23);
            tbTop.TabIndex = 61;
            // 
            // tbWidth
            // 
            tbWidth.Location = new Point(168, 369);
            tbWidth.Name = "tbWidth";
            tbWidth.Size = new Size(120, 23);
            tbWidth.TabIndex = 62;
            // 
            // tbHeight
            // 
            tbHeight.Location = new Point(168, 399);
            tbHeight.Name = "tbHeight";
            tbHeight.Size = new Size(120, 23);
            tbHeight.TabIndex = 63;
            // 
            // frmMain
            // 
            AcceptButton = butOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = butCancel;
            ClientSize = new Size(962, 674);
            Controls.Add(cbIsPaused);
            Controls.Add(txtVersion);
            Controls.Add(cbRunAtLogin);
            Controls.Add(cbShowAllWindows);
            Controls.Add(listView1);
            Controls.Add(butResetMoved);
            Controls.Add(cbResetIfNewScreen);
            Controls.Add(groupBox1);
            Controls.Add(butEditScreens);
            Controls.Add(butCancel);
            Controls.Add(butExit);
            Controls.Add(butApply);
            Controls.Add(butOK);
            Controls.Add(groupBox2);
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
            ((System.ComponentModel.ISupportInitialize)tbLeft).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbTop).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbWidth).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Label label2;
        private TextBox tbExe;
        private Button butOK;
        private Button butApply;
        private Button butExit;
        private Button butRemove;
        private Label label3;
        private Label label4;
        private NumericUpDown tbTop;
        private Label label5;
        private NumericUpDown tbWidth;
        private Label label6;
        private NumericUpDown tbHeight;
        private CheckBox cbCustomWidth;
        private CheckBox cbCustomHeight;
        private CheckBox cbFullScreen;
        private Button butCancel;
        private Button butEditScreens;
        private CheckBox cbSearchExe;
        private GroupBox groupBox1;
        private ComboBox cbHotKeyLeft;
        private TextBox tbHotKeyCharacter;
        private ComboBox cbHotKeyRight;
        private Label label7;
        private TextBox tbName;
        private CheckBox cbResetIfNewScreen;
        private GroupBox groupBox2;
        private Button butResetMoved;
        private ToolTip toolTip1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private CheckBox cbShowAllWindows;
        private CheckBox cbIgnoreChildWindows;
        private CheckBox cbAlwaysMove;
        private CheckBox cbRunAtLogin;
        private TextBox tbWindowClass;
        private Label label8;
        private CheckBox cbWindowClass;
        private Label tbSavedWindowIndex;
        private GroupBox groupBox4;
        private RadioButton radioStartsWithInclude;
        private RadioButton radioContainsInclude;
        private RadioButton radioFullInclude;
        private TextBox tbTitleInclude;
        private CheckBox cbSearchTitleInclude;
        private GroupBox groupBox3;
        private RadioButton radioStartsWithExclude;
        private RadioButton radioContainsExclude;
        private RadioButton radioFullExclude;
        private TextBox tbTitleExclude;
        private CheckBox cbSearchTitleExclude;
        private TextBox txtVersion;
        private Button butDuplicate;
        private CheckBox cbCanResize;
        public NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private CheckBox cbIsPaused;
        private ImageList stateImageList;
        private NumericUpDown tbLeft;
    }
}

