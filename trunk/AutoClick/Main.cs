using System;
using System.Windows.Forms;

namespace AutoClick
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmBrowser frmBrowser = new frmBrowser();
            frmBrowser.MdiParent = this;
            frmBrowser.Show();
        }
    }
}
