using System;
using System.Windows.Forms;

namespace AutoClickWithCaptcha
{
    public partial class Main : Form
    {        
        private string[,] ptcSites = new string[,] {
            { "nycebux", "http://www.nycebux.com/pages/login", "http://www.nycebux.com/pages/clickads", "Click Ads - NYCE Bux", "35000"},
            { "14bux", "http://www.14bux.com/pages/login", "http://www.14bux.com/pages/clickads", "Click Ads - 14bux", "20000"},
            { "mintptc", "http://www.mintptc.com/pages/login", "http://www.mintptc.com/pages/clickads", "Click Ads - mintptc.com", "35000"},
            { "newwavebux", "http://www.newwavebux.com/pages/login", "http://www.newwavebux.com/pages/clickads", "Click Ads - NewWaveBux.com", "35000"},
            { "buxreflink", "http://www.buxreflink.com/pages/login", "http://www.buxreflink.com/pages/clickads", "Click Ads - BuxRefLink.com", "35000"},
            { "tviptc", "http://www.tviptc.com/pages/login", "http://www.tviptc.com/pages/clickads", "Click Ads - TVIptc.com", "15000"},
            { "hiddenbux", "http://www.hiddenbux.com/pages/login", "http://www.hiddenbux.com/pages/clickads", "Click Ads - HiddenBux", "25000"},
            { "roubux", "http://www.roubux.com/pages/login", "http://www.roubux.com/pages/clickads", "Click Ads - ROUbux", "35000"},
            { "thebuxgroup", "http://www.thebuxgroup.com/pages/login", "http://www.thebuxgroup.com/pages/clickads", "Click Ads - TheBuxGroup.com", "35000"},
            { "freebirdbux", "http://www.freebirdbux.com/pages/login", "http://www.freebirdbux.com/pages/clickads", "Click Ads - FreeBirdBux", "35000"},
            { "memberbux", "http://www.memberbux.com/pages/login", "http://www.memberbux.com/pages/clickads", "Click Ads - MemberBux.com", "35000"},
            { "yourprofitbux", "http://www.yourprofitbux.com/pages/login", "http://www.yourprofitbux.com/pages/clickads", "Click Ads - YourProfitBux.com", "35000"},
            { "infinitybux", "http://www.infinitybux.com/pages/login", "http://www.infinitybux.com/pages/clickads", "Click Ads - InfinityBux", "35000"},
            { "buxincomecafe", "http://www.buxincomecafe.com/pages/login", "http://www.buxincomecafe.com/pages/clickads", "Click Ads - BuxIncomeCafe.com", "25000"},
            { "allstarbux", "http://www.allstarbux.com/pages/login", "http://www.allstarbux.com/pages/clickads", "Click Ads - allstarbux.com", "65000"},
            { "buxtobux", "http://www.buxtobux.com/pages/login", "http://www.buxtobux.com/pages/clickads", "Click Ads - buxtobux.com", "65000"},
            { "breakoutbux", "http://www.breakoutbux.com/pages/login", "http://www.breakoutbux.com/pages/clickads", "Click Ads - breakoutbux.com", "65000"},
            { "quickmoneybux", "http://www.quickmoneybux.com/pages/login", "http://www.quickmoneybux.com/pages/clickads", "Click Ads - QuickMoneyBux.com", "65000"},
            { "buxbillionaire", "http://www.buxbillionaire.com/pages/login", "http://www.buxbillionaire.com/pages/clickads", "Click Ads - buxbillionaire.com", "65000"},
            { "5dollarclick", "http://www.5dollarclick.com/pages/login", "http://www.5dollarclick.com/pages/clickads", "Click Ads - 5dollarclick.com", "65000"},
            { "downunderbux", "http://www.downunderbux.com/pages/login", "http://www.downunderbux.com/pages/clickads", "Click Ads - downunderbux.com", "65000"},
            { "buxclan", "http://www.buxclan.com/pages/login", "http://www.buxclan.com/pages/clickads", "Click Ads - BuxClan.com", "35000"},
            { "redwingptc", "http://www.redwingptc.com/pages/login", "http://www.redwingptc.com/pages/clickads", "Click Ads - redwingptc", "65000"}
        };

        private ToolStripButton btnPtcSite;
        private Form[] childForms;
        private frmBrowser frmBrowser;

        public Main()
        {
            InitializeComponent();

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
                    Util.writeLog("Bring to front: " + btnPtcSite.Text);
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
            Util.writeLog("Opening " + ptcSites[i, 0] + " ...");
            frmBrowser = new frmBrowser(ptcSites[i, 1], ptcSites[i, 2], ptcSites[i, 3], int.Parse(ptcSites[i, 4]));
            frmBrowser.Name = i.ToString();
            frmBrowser.MdiParent = this;
            frmBrowser.Show();
        }
    }
}
