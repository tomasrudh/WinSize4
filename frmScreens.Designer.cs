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
            this.tbFullHeight = new System.Windows.Forms.TextBox();
            this.tbFullWidth = new System.Windows.Forms.TextBox();
            this.tbCustomTop = new System.Windows.Forms.TextBox();
            this.tbCustomLeft = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWorkingAreaHeight = new System.Windows.Forms.TextBox();
            this.tbWorkingAreaWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCustomHeight = new System.Windows.Forms.TextBox();
            this.tbCustomWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.width = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.height = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.primary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFullHeight
            // 
            this.tbFullHeight.Enabled = false;
            this.tbFullHeight.Location = new System.Drawing.Point(391, 30);
            this.tbFullHeight.Name = "tbFullHeight";
            this.tbFullHeight.Size = new System.Drawing.Size(113, 20);
            this.tbFullHeight.TabIndex = 34;
            // 
            // tbFullWidth
            // 
            this.tbFullWidth.Enabled = false;
            this.tbFullWidth.Location = new System.Drawing.Point(272, 30);
            this.tbFullWidth.Name = "tbFullWidth";
            this.tbFullWidth.Size = new System.Drawing.Size(113, 20);
            this.tbFullWidth.TabIndex = 32;
            // 
            // tbCustomTop
            // 
            this.tbCustomTop.Location = new System.Drawing.Point(198, 32);
            this.tbCustomTop.Name = "tbCustomTop";
            this.tbCustomTop.Size = new System.Drawing.Size(113, 20);
            this.tbCustomTop.TabIndex = 38;
            this.tbCustomTop.Visible = false;
            this.tbCustomTop.TextChanged += new System.EventHandler(this.tbCustomTop_TextChanged);
            // 
            // tbCustomLeft
            // 
            this.tbCustomLeft.Location = new System.Drawing.Point(79, 32);
            this.tbCustomLeft.Name = "tbCustomLeft";
            this.tbCustomLeft.Size = new System.Drawing.Size(113, 20);
            this.tbCustomLeft.TabIndex = 37;
            this.tbCustomLeft.Visible = false;
            this.tbCustomLeft.TextChanged += new System.EventHandler(this.tbCustomLeft_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(190, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Full area";
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(439, 191);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 41;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(358, 191);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 40;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(190, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Working area";
            // 
            // tbWorkingAreaHeight
            // 
            this.tbWorkingAreaHeight.Enabled = false;
            this.tbWorkingAreaHeight.Location = new System.Drawing.Point(391, 56);
            this.tbWorkingAreaHeight.Name = "tbWorkingAreaHeight";
            this.tbWorkingAreaHeight.Size = new System.Drawing.Size(113, 20);
            this.tbWorkingAreaHeight.TabIndex = 43;
            // 
            // tbWorkingAreaWidth
            // 
            this.tbWorkingAreaWidth.Enabled = false;
            this.tbWorkingAreaWidth.Location = new System.Drawing.Point(272, 56);
            this.tbWorkingAreaWidth.Name = "tbWorkingAreaWidth";
            this.tbWorkingAreaWidth.Size = new System.Drawing.Size(113, 20);
            this.tbWorkingAreaWidth.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Height";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbCustomHeight);
            this.groupBox1.Controls.Add(this.tbCustomWidth);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbCustomTop);
            this.groupBox1.Controls.Add(this.tbCustomLeft);
            this.groupBox1.Location = new System.Drawing.Point(193, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 103);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom working area";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 52;
            this.label6.Text = "Width";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Height";
            // 
            // tbCustomHeight
            // 
            this.tbCustomHeight.Location = new System.Drawing.Point(198, 71);
            this.tbCustomHeight.Name = "tbCustomHeight";
            this.tbCustomHeight.Size = new System.Drawing.Size(113, 20);
            this.tbCustomHeight.TabIndex = 51;
            this.tbCustomHeight.TextChanged += new System.EventHandler(this.tbCustomHeight_TextChanged_1);
            // 
            // tbCustomWidth
            // 
            this.tbCustomWidth.Location = new System.Drawing.Point(79, 71);
            this.tbCustomWidth.Name = "tbCustomWidth";
            this.tbCustomWidth.Size = new System.Drawing.Size(113, 20);
            this.tbCustomWidth.TabIndex = 50;
            this.tbCustomWidth.TextChanged += new System.EventHandler(this.tbCustomWidth_TextChanged_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Left";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 49;
            this.label5.Text = "Top";
            this.label5.Visible = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.width,
            this.height,
            this.primary});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(175, 202);
            this.listView1.TabIndex = 48;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // width
            // 
            this.width.Text = "Width";
            this.width.Width = 50;
            // 
            // height
            // 
            this.height.Text = "Height";
            this.height.Width = 50;
            // 
            // primary
            // 
            this.primary.Text = "Primary";
            this.primary.Width = 50;
            // 
            // frmScreens
            // 
            this.AcceptButton = this.butOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(526, 225);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWorkingAreaHeight);
            this.Controls.Add(this.tbWorkingAreaWidth);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.tbFullHeight);
            this.Controls.Add(this.tbFullWidth);
            this.Controls.Add(this.label10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScreens";
            this.Text = "Screens";
            this.Load += new System.EventHandler(this.Screens_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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