using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinSize4
{
    public partial class frmScreens : Form
    {
        public List<ClsScreenList> _screenList = new List<ClsScreenList>();
        public List<ClsScreenList> _returnScreenList { get; set; }
        int _boundsWidth;
        int _boundsHeight;
        bool _primary;
        public frmScreens(List<ClsScreenList> Form1ScreenList, int _boundsWidth, int _boundsHeight, bool _primary)
        {
            InitializeComponent();
            this._screenList = Form1ScreenList;
            this._boundsWidth = _boundsWidth;
            this._boundsHeight = _boundsHeight;
            this._primary = _primary;
        }

        private void Screens_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string[] row;
            int Index = 0;
            foreach (ClsScreenList Scr in this._screenList)
            {
                if (Scr.Primary)
                    row = new string[] { Scr.BoundsWidth.ToString(), Scr.BoundsHeight.ToString(), "Yes" };
                else
                    row = new string[] { Scr.BoundsWidth.ToString(), Scr.BoundsHeight.ToString(), "No" };
                var listViewItem = new ListViewItem(row);
                if (! Scr.Present)
                {
                    listViewItem.ForeColor = System.Drawing.Color.Silver;
                    listViewItem.SubItems[1].ForeColor = Color.Silver;
                    listViewItem.SubItems[2].ForeColor = Color.Silver;
                    listViewItem.UseItemStyleForSubItems = false;
                }
                listView1.Items.Add(listViewItem);
                if (Scr.BoundsWidth == _boundsWidth && Scr.BoundsHeight == _boundsHeight && Scr.Primary == _primary)
                    Index = listView1.Items.Count - 1;
            }
            listView1.Items[Index].Selected = true;
            listView1.Select();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                tbFullWidth.Text = this._screenList[listView1.SelectedItems[0].Index].BoundsWidth.ToString();
                tbFullHeight.Text = this._screenList[listView1.SelectedItems[0].Index].BoundsHeight.ToString();
                tbWorkingAreaWidth.Text = this._screenList[listView1.SelectedItems[0].Index].WorkingAreaWidth.ToString();
                tbWorkingAreaHeight.Text = this._screenList[listView1.SelectedItems[0].Index].WorkingAreaHeight.ToString();
                tbCustomLeft.Text = this._screenList[listView1.SelectedItems[0].Index].CustomLeft.ToString();
                tbCustomTop.Text = this._screenList[listView1.SelectedItems[0].Index].CustomTop.ToString();
                tbCustomWidth.Text = this._screenList[listView1.SelectedItems[0].Index].CustomWidth.ToString();
                tbCustomHeight.Text = this._screenList[listView1.SelectedItems[0].Index].CustomHeight.ToString();
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this._returnScreenList = this._screenList;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void tbCustomLeft_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbCustomLeft.Text, out int number))
                this._screenList[listView1.SelectedItems[0].Index].CustomLeft = number;
        }

        private void tbCustomTop_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbCustomTop.Text, out int number))
                this._screenList[listView1.SelectedItems[0].Index].CustomTop = number;
        }

        private void tbCustomWidth_TextChanged_1(object sender, EventArgs e)
        {
            if (int.TryParse(tbCustomWidth.Text, out int number))
                this._screenList[listView1.SelectedItems[0].Index].CustomWidth = number;
        }

        private void tbCustomHeight_TextChanged_1(object sender, EventArgs e)
        {
            if (int.TryParse(tbCustomHeight.Text, out int number))
                this._screenList[listView1.SelectedItems[0].Index].CustomHeight = number;
        }
    }
}
