namespace WinSize4
{
    partial class frmScreens
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScreens));
            tbFullHeight = new TextBox();
            tbFullWidth = new TextBox();
            tbCustomTop = new TextBox();
            tbCustomLeft = new TextBox();
            label10 = new Label();
            butCancel = new Button();
            butOK = new Button();
            label1 = new Label();
            tbWorkingAreaHeight = new TextBox();
            tbWorkingAreaWidth = new TextBox();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            label7 = new Label();
            tbCustomHeight = new TextBox();
            tbCustomWidth = new TextBox();
            label4 = new Label();
            label5 = new Label();
            listView1 = new ListView();
            width = new ColumnHeader();
            height = new ColumnHeader();
            primary = new ColumnHeader();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tbFullHeight
            // 
            tbFullHeight.Enabled = false;
            tbFullHeight.Location = new Point(652, 58);
            tbFullHeight.Margin = new Padding(5, 6, 5, 6);
            tbFullHeight.Name = "tbFullHeight";
            tbFullHeight.Size = new Size(186, 31);
            tbFullHeight.TabIndex = 34;
            // 
            // tbFullWidth
            // 
            tbFullWidth.Enabled = false;
            tbFullWidth.Location = new Point(453, 58);
            tbFullWidth.Margin = new Padding(5, 6, 5, 6);
            tbFullWidth.Name = "tbFullWidth";
            tbFullWidth.Size = new Size(186, 31);
            tbFullWidth.TabIndex = 32;
            // 
            // tbCustomTop
            // 
            tbCustomTop.Location = new Point(330, 62);
            tbCustomTop.Margin = new Padding(5, 6, 5, 6);
            tbCustomTop.Name = "tbCustomTop";
            tbCustomTop.Size = new Size(186, 31);
            tbCustomTop.TabIndex = 38;
            tbCustomTop.Visible = false;
            tbCustomTop.TextChanged += tbCustomTop_TextChanged;
            // 
            // tbCustomLeft
            // 
            tbCustomLeft.Location = new Point(132, 62);
            tbCustomLeft.Margin = new Padding(5, 6, 5, 6);
            tbCustomLeft.Name = "tbCustomLeft";
            tbCustomLeft.Size = new Size(186, 31);
            tbCustomLeft.TabIndex = 37;
            tbCustomLeft.Visible = false;
            tbCustomLeft.TextChanged += tbCustomLeft_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(317, 63);
            label10.Margin = new Padding(5, 0, 5, 0);
            label10.Name = "label10";
            label10.Size = new Size(77, 25);
            label10.TabIndex = 36;
            label10.Text = "Full area";
            // 
            // butCancel
            // 
            butCancel.DialogResult = DialogResult.Cancel;
            butCancel.Location = new Point(732, 367);
            butCancel.Margin = new Padding(5, 6, 5, 6);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(125, 44);
            butCancel.TabIndex = 41;
            butCancel.Text = "Cancel";
            butCancel.UseVisualStyleBackColor = true;
            // 
            // butOK
            // 
            butOK.Location = new Point(597, 367);
            butOK.Margin = new Padding(5, 6, 5, 6);
            butOK.Name = "butOK";
            butOK.Size = new Size(125, 44);
            butOK.TabIndex = 40;
            butOK.Text = "OK";
            butOK.UseVisualStyleBackColor = true;
            butOK.Click += butOK_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(317, 113);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(117, 25);
            label1.TabIndex = 44;
            label1.Text = "Working area";
            // 
            // tbWorkingAreaHeight
            // 
            tbWorkingAreaHeight.Enabled = false;
            tbWorkingAreaHeight.Location = new Point(652, 108);
            tbWorkingAreaHeight.Margin = new Padding(5, 6, 5, 6);
            tbWorkingAreaHeight.Name = "tbWorkingAreaHeight";
            tbWorkingAreaHeight.Size = new Size(186, 31);
            tbWorkingAreaHeight.TabIndex = 43;
            // 
            // tbWorkingAreaWidth
            // 
            tbWorkingAreaWidth.Enabled = false;
            tbWorkingAreaWidth.Location = new Point(453, 108);
            tbWorkingAreaWidth.Margin = new Padding(5, 6, 5, 6);
            tbWorkingAreaWidth.Name = "tbWorkingAreaWidth";
            tbWorkingAreaWidth.Size = new Size(186, 31);
            tbWorkingAreaWidth.TabIndex = 42;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(448, 17);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 25);
            label2.TabIndex = 45;
            label2.Text = "Width";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(647, 17);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(65, 25);
            label3.TabIndex = 46;
            label3.Text = "Height";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(tbCustomHeight);
            groupBox1.Controls.Add(tbCustomWidth);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(tbCustomTop);
            groupBox1.Controls.Add(tbCustomLeft);
            groupBox1.Location = new Point(322, 158);
            groupBox1.Margin = new Padding(5, 6, 5, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(5, 6, 5, 6);
            groupBox1.Size = new Size(535, 198);
            groupBox1.TabIndex = 47;
            groupBox1.TabStop = false;
            groupBox1.Text = "Custom working area";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(127, 106);
            label6.Margin = new Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new Size(60, 25);
            label6.TabIndex = 52;
            label6.Text = "Width";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(325, 106);
            label7.Margin = new Padding(5, 0, 5, 0);
            label7.Name = "label7";
            label7.Size = new Size(65, 25);
            label7.TabIndex = 53;
            label7.Text = "Height";
            // 
            // tbCustomHeight
            // 
            tbCustomHeight.Location = new Point(330, 137);
            tbCustomHeight.Margin = new Padding(5, 6, 5, 6);
            tbCustomHeight.Name = "tbCustomHeight";
            tbCustomHeight.Size = new Size(186, 31);
            tbCustomHeight.TabIndex = 51;
            tbCustomHeight.TextChanged += tbCustomHeight_TextChanged_1;
            // 
            // tbCustomWidth
            // 
            tbCustomWidth.Location = new Point(132, 137);
            tbCustomWidth.Margin = new Padding(5, 6, 5, 6);
            tbCustomWidth.Name = "tbCustomWidth";
            tbCustomWidth.Size = new Size(186, 31);
            tbCustomWidth.TabIndex = 50;
            tbCustomWidth.TextChanged += tbCustomWidth_TextChanged_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(127, 31);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(41, 25);
            label4.TabIndex = 48;
            label4.Text = "Left";
            label4.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(325, 31);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(41, 25);
            label5.TabIndex = 49;
            label5.Text = "Top";
            label5.Visible = false;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { width, height, primary });
            listView1.Location = new Point(20, 23);
            listView1.Margin = new Padding(5, 6, 5, 6);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(289, 385);
            listView1.TabIndex = 48;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // width
            // 
            width.Text = "Width";
            width.Width = 50;
            // 
            // height
            // 
            height.Text = "Height";
            height.Width = 50;
            // 
            // primary
            // 
            primary.Text = "Primary";
            primary.Width = 50;
            // 
            // frmScreens
            // 
            AcceptButton = butOK;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = butCancel;
            ClientSize = new Size(877, 433);
            Controls.Add(listView1);
            Controls.Add(groupBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbWorkingAreaHeight);
            Controls.Add(tbWorkingAreaWidth);
            Controls.Add(butCancel);
            Controls.Add(butOK);
            Controls.Add(tbFullHeight);
            Controls.Add(tbFullWidth);
            Controls.Add(label10);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5, 6, 5, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmScreens";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Screens";
            Load += Screens_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbFullHeight;
        private System.Windows.Forms.TextBox tbFullWidth;
        private System.Windows.Forms.TextBox tbCustomTop;
        private System.Windows.Forms.TextBox tbCustomLeft;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWorkingAreaHeight;
        private System.Windows.Forms.TextBox tbWorkingAreaWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCustomHeight;
        private System.Windows.Forms.TextBox tbCustomWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader width;
        private System.Windows.Forms.ColumnHeader height;
        private System.Windows.Forms.ColumnHeader primary;
    }
}