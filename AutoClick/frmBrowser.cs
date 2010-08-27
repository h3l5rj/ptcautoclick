using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClick
{
    public partial class frmBrowser : Form
    {
        private static uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "http://www.tendollarclick.com/index.php?view=login", "http://www.tendollarclick.com/index.php?view=click", "http://www.tendollarclick.com/gpt.php",
                "Ten Dollar Click : Log In", "Ten Dollar Click : My Account Panel - Tran Vinh Truong" , "Ten Dollar Click : Get Paid To Click", "Viewing Ad @ Ten Dollar Click", "65000"},
            { "http://www.ptcsense.com/index.php?view=login", "http://www.ptcsense.com/index.php?view=click", "http://www.ptcsense.com/gpt.php",
                "PTC Sense : Log In", "PTC Sense : My Account Panel - Tran Vinh Truong" , "PTC Sense : Get Paid To Click", "Viewing Ad @ PTC Sense", "35000"},
            { "http://www.richptc.com/index.php?view=login", "http://www.richptc.com/index.php?view=account&ac=click", "http://www.richptc.com/gpt.php",
                "Rich PTC : Log In", "Rich PTC : My Account Panel - Tran Vinh Truong" , "Rich PTC : Get Paid To Click", "Viewing Ad @ Rich PTC", "35000"},
            { "http://www.bigmoneyptc.com/index.php?view=login", "http://www.bigmoneyptc.com/index.php?view=account&ac=click", "http://www.bigmoneyptc.com/gpt.php",
                "Big Money PTC : Log In", "Big Money PTC : My Account Panel - Tran Vinh Truong" , "Big Money PTC : Get Paid To Click", "Viewing Ad @ Big Money PTC", "35000"},
            { "http://www.grandptc.com/index.php?view=login", "http://www.grandptc.com/index.php?view=click", "http://www.grandptc.com/gpt.php",
                "Grand PTC : Log In", "Grand PTC : My Account Panel - Tran Vinh Truong" , "Grand PTC : Get Paid To Click", "Viewing Ad @ Grand PTC", "35000"}
        };
        private const string USERNAME = "tranvinhtruong";
        private const string PASSWORD = "tctlT1005";

        private Match matchObj;

        public frmBrowser()
        {
            InitializeComponent();
        }

        private void startSurf()
        {
            wbMain.Navigate(ptcSites[index, 0]);
        }

        private void frmBrowser_Load(object sender, EventArgs e)
        {
            startSurf();
        }

        private void wbMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Console.WriteLine(wbMain.DocumentTitle);
            if (wbMain.DocumentTitle == ptcSites[index, 3]) // log in page
            {
                wbMain.Document.GetElementById("form_user").SetAttribute("value", USERNAME);
                wbMain.Document.GetElementById("form_pwd").SetAttribute("value", PASSWORD);

                // click on "Access Account"
                if (index == 0) // Ten Dollar Click
                {
                    wbMain.Document.GetElementFromPoint(new Point(538, 407)).InvokeMember("click"); 
                    wbMain.Document.GetElementFromPoint(new Point(538, 606)).InvokeMember("click");
                }
                else if (index == 1) // PTC Sense
                {
                    wbMain.Document.GetElementFromPoint(new Point(703, 392)).InvokeMember("click"); 
                }
                else if (index == 2) // Rich PTC
                {
                    wbMain.Document.GetElementFromPoint(new Point(401, 430)).InvokeMember("click"); 
                }
                else if (index == 3) // Big Money PTC
                {
                    wbMain.Document.GetElementFromPoint(new Point(499, 408)).InvokeMember("click"); 
                }
                else if (index == 4) // Grand PTC
                {
                    wbMain.Document.GetElementFromPoint(new Point(699, 549)).InvokeMember("click"); 
                }
            }
            else if (wbMain.DocumentTitle == ptcSites[index, 4])    // account page
            {
                wbMain.Navigate(ptcSites[index, 1]); // open view ads page
            }
            else if (wbMain.DocumentTitle == ptcSites[index, 5])    // view ads page
            {
                matchObj = Regex.Match(wbMain.DocumentText, "(?<=a href=\"gpt.php)[^\"]*");
                if (matchObj.Success)
                {
                    wbMain.Navigate(ptcSites[index, 2] + matchObj.Value);
                }
                else
                {
                    index++;
                    if (index >= ptcSites.GetLength(0))   // re-surf
                    {
                        index = 0;
                    }
                    startSurf();    // surf next site
                }
            }
            else if (wbMain.DocumentTitle == ptcSites[index, 6])    // count down page
            {
                if (!waitForClick.Enabled)
                {
                    Console.WriteLine("waitForClick timer - Start");
                    waitForClick.Interval = int.Parse(ptcSites[index, 7]);
                    waitForClick.Start();
                }
            }
            else
            {
                wbMain.Navigate(ptcSites[index, 1]); // back to view ads page
            }
        }

        private void waitForClick_Tick(object sender, EventArgs e)
        {
            if (wbMain.Document.Window.Frames.Count > 0)
            {
                // find image and autoclick
                HtmlDocument countdownFrame = wbMain.Document.Window.Frames[0].Document;
                if (countdownFrame.GetElementById("timer").InnerHtml.Contains("Click"))
                {
                    string key = countdownFrame.GetElementById("timer").InnerHtml.Substring(6);
                    Console.WriteLine("key = " + key);
                    foreach (HtmlElement link in countdownFrame.Links)
                    {
                        Console.WriteLine(link.InnerHtml);
                        if (link.InnerHtml.Contains(key))
                        {
                            link.InvokeMember("click");
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("waitForClick timer - Stop");
                waitForClick.Stop();
            }
        }
    }
}
