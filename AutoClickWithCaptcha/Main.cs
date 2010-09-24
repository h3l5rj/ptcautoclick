using System;
using System.Windows.Forms;

namespace AutoClickWithCaptcha
{
    public partial class Main : Form
    {
        private uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "nycebux", "http://www.nycebux.com/pages/login", "http://www.nycebux.com/pages/clickads", "20000"}
            , { "mintptc", "http://www.mintptc.com/pages/login", "http://www.mintptc.com/pages/clickads", "30000"}
            , { "newwavebux", "http://www.newwavebux.com/pages/login", "http://www.newwavebux.com/pages/clickads", "30000"}
            , { "buxreflink", "http://www.buxreflink.com/pages/login", "http://www.buxreflink.com/pages/clickads", "30000"}
            , { "tviptc", "http://www.tviptc.com/pages/login", "http://www.tviptc.com/pages/clickads", "10000"}
            , { "hiddenbux", "http://www.hiddenbux.com/pages/login", "http://www.hiddenbux.com/pages/clickads", "20000"}
            , { "roubux", "http://www.roubux.com/pages/login", "http://www.roubux.com/pages/clickads", "30000"}
            , { "thebuxgroup", "http://www.thebuxgroup.com/pages/login", "http://www.thebuxgroup.com/pages/clickads", "30000"}
            , { "freebirdbux", "http://www.freebirdbux.com/pages/login", "http://www.freebirdbux.com/pages/clickads", "30000"}
            , { "yourprofitbux", "http://www.yourprofitbux.com/pages/login", "http://www.yourprofitbux.com/pages/clickads", "30000"}
            , { "allstarbux", "http://www.allstarbux.com/pages/login", "http://www.allstarbux.com/pages/clickads", "60000"}
            , { "buxtobux", "http://www.buxtobux.com/pages/login", "http://www.buxtobux.com/pages/clickads", "60000"}
            , { "breakoutbux", "http://www.breakoutbux.com/pages/login", "http://www.breakoutbux.com/pages/clickads", "60000"}
            , { "quickmoneybux", "http://www.quickmoneybux.com/pages/login", "http://www.quickmoneybux.com/pages/clickads", "60000"}
            , { "downunderbux", "http://www.downunderbux.com/pages/login", "http://www.downunderbux.com/pages/clickads", "15000"}
            , { "buxclan", "http://www.buxclan.com/pages/login", "http://www.buxclan.com/pages/clickads", "30000"}
        };

        private ToolStripButton btnPtcSite;
        private Form[] childForms;
        private frmBrowser frmBrowser;

        public Main()
        {
            InitializeComponent();

            // fill toolbar
            for (int i = 0; i < ptcSites.GetLength(0); i++)
            {
                btnPtcSite = new ToolStripButton(ptcSites[i, 0], null, null, i.ToString());
                btnPtcSite.Click += new System.EventHandler(this.toolStripButton_Click);
                toolbar.Items.Add(btnPtcSite);
            }
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            btnPtcSite = sender as ToolStripButton;
            int index = int.Parse(btnPtcSite.Name);
            childForms = this.MdiChildren;
            if (childForms.Length > 0)
            {
                int i;
                for (i = 0; i < childForms.Length; i++)
                {
                    frmBrowser = childForms[i] as frmBrowser;
                    if (frmBrowser.Name == btnPtcSite.Name)
                        break;
                }
                if (i < childForms.Length)
                {
                    frmBrowser.BringToFront();
                }
                else
                {
                    createNewBrowser(index);
                }
            }
            else
            {
                createNewBrowser(index);
            }
        }

        private void createNewBrowser(int i)
        {
            frmBrowser = new frmBrowser(ptcSites[i, 0], ptcSites[i, 1], ptcSites[i, 2], int.Parse(ptcSites[i, 3]));
            frmBrowser.Name = i.ToString();
            frmBrowser.MdiParent = this;
            frmBrowser.Show();
        }

        private void switcher_Tick(object sender, EventArgs e)
        {
            childForms = this.MdiChildren;
            if (childForms.Length > 0)
            {
                index++;
                if (index >= childForms.Length)
                {
                    index = 0;
                }
                frmBrowser = childForms[index] as frmBrowser;
                frmBrowser.BringToFront();
            }
        }
    }
}
