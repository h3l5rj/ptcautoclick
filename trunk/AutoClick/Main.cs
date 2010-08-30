using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClick
{
    public partial class Main : Form
    {
        private Boolean logToFile = true;

        private static uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "http://www.tendollarclick.com/index.php?view=login", "http://www.tendollarclick.com/index.php?view=click", "http://www.tendollarclick.com/gpt.php",
                "Ten Dollar Click : Log In", "Ten Dollar Click : My Account Panel - Tran Vinh Truong" , "Ten Dollar Click : Get Paid To Click", "Viewing Ad @ Ten Dollar Click", "61000"},
            { "http://www.ptcsense.com/index.php?view=login", "http://www.ptcsense.com/index.php?view=click", "http://www.ptcsense.com/gpt.php",
                "PTC Sense : Log In", "PTC Sense : My Account Panel - Tran Vinh Truong" , "PTC Sense : Get Paid To Click", "Viewing Ad @ PTC Sense", "31000"},
            { "http://www.richptc.com/index.php?view=login", "http://www.richptc.com/index.php?view=account&ac=click", "http://www.richptc.com/gpt.php",
                "Rich PTC : Log In", "Rich PTC : My Account Panel - Tran Vinh Truong" , "Rich PTC : Get Paid To Click", "Viewing Ad @ Rich PTC", "31000"},
            { "http://www.bigmoneyptc.com/index.php?view=login", "http://www.bigmoneyptc.com/index.php?view=account&ac=click", "http://www.bigmoneyptc.com/gpt.php",
                "Big Money PTC : Log In", "Big Money PTC : My Account Panel - Tran Vinh Truong" , "Big Money PTC : Get Paid To Click", "Viewing Ad @ Big Money PTC", "31000"},
            { "http://www.grandptc.com/index.php?view=login", "http://www.grandptc.com/index.php?view=click", "http://www.grandptc.com/gpt.php",
                "Grand PTC : Log In", "Grand PTC : My Account Panel - Tran Vinh Truong" , "Grand PTC : Get Paid To Click", "Viewing Ad @ Grand PTC", "31000"},
            { "http://www.ptcbiz.com/index.php?view=login", "http://www.ptcbiz.com/index.php?view=click", "http://www.ptcbiz.com/gpt.php",
                "PTC Biz : Log In", "PTC Biz : My Account Panel - Tran Vinh Truong" , "PTC Biz : Get Paid To Click", "Viewing Ad @ PTC Biz", "31000"},
            { "http://www.ptcwallet.com/index.php?view=login", "http://www.ptcwallet.com/index.php?view=click", "http://www.ptcwallet.com/gpt.php",
                "PTC Wallet : Log In", "PTC Wallet : My Account Panel - Tran Vinh Truong" , "PTC Wallet : Get Paid To Click", "Viewing Ad @ PTC Wallet", "11000"},
            { "http://www.buxinc.com/index.php?view=login", "http://www.buxinc.com/index.php?view=click", "http://www.buxinc.com/gpt.php",
                "Bux Inc : Log In", "Bux Inc : My Account Panel - Tran Vinh Truong" , "Bux Inc : Get Paid To Click", "Viewing Ad @ Bux Inc", "31000"},
            { "http://www.fineptc.com/index.php?view=login", "http://www.fineptc.com/index.php?view=click", "http://www.fineptc.com/gpt.php",
                "Fine PTC : Log In", "Fine PTC : My Account Panel - Tran Vinh Truong" , "Fine PTC : Get Paid To Click", "Viewing Ad @ Fine PTC", "61000"}
        };
        private const string USERNAME = "tranvinhtruong";
        private const string PASSWORD = "tctlT1005";

        private Match matchObj;

        public Main()
        {
            InitializeComponent();

            startSurf();
        }

        private void startSurf()
        {
            writeLog(index + ": " + ptcSites[index, 1]);
            wbBrowser.Navigate(ptcSites[index, 1]);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            writeLog(wbBrowser.DocumentTitle);
            if (wbBrowser.DocumentTitle == ptcSites[index, 3]) // log in page
            {
                wbBrowser.Document.GetElementById("form_user").SetAttribute("value", USERNAME);
                wbBrowser.Document.GetElementById("form_pwd").SetAttribute("value", PASSWORD);

                // click on "Access Account"
                if (index == 0) // Ten Dollar Click
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(538, 407)).InvokeMember("click");
                    wbBrowser.Document.GetElementFromPoint(new Point(538, 606)).InvokeMember("click");
                }
                else if (index == 1) // PTC Sense
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(703, 392)).InvokeMember("click");
                }
                else if (index == 2) // Rich PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(401, 430)).InvokeMember("click");
                }
                else if (index == 3) // Big Money PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(499, 408)).InvokeMember("click");
                    wbBrowser.Document.GetElementFromPoint(new Point(505, 321)).InvokeMember("click");
                }
                else if (index == 4) // Grand PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(572, 391)).InvokeMember("click");
                    wbBrowser.Document.GetElementFromPoint(new Point(699, 549)).InvokeMember("click");
                }
                else if (index == 5) // PTC Biz
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(858, 335)).InvokeMember("click");
                }
                else if (index == 6) // PTC Wallet
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(508, 454)).InvokeMember("click");
                }
                else if (index == 7) // Bux Inc
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(901, 330)).InvokeMember("click");
                }
                else if (index == 8) // Fine PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(571, 412)).InvokeMember("click");
                }
            }
            else if (wbBrowser.DocumentTitle == ptcSites[index, 4])    // account page
            {
                wbBrowser.Navigate(ptcSites[index, 1]); // open view ads page
            }
            else if (wbBrowser.DocumentTitle == ptcSites[index, 5])    // view ads page
            {
                if (wbBrowser.DocumentText.Contains("login"))    // not log in
                {
                    wbBrowser.Navigate(ptcSites[index, 0]);    // open log in page
                }
                else
                {
                    try
                    {
                        matchObj = Regex.Match(wbBrowser.DocumentText, "(?<=a href=\"gpt.php)[^\"]*");
                        writeLog("link available to click? - " + matchObj.Success);
                        if (matchObj.Success)
                        {
                            wbBrowser.Navigate(ptcSites[index, 2] + matchObj.Value);
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
                    catch (FileLoadException)
                    {
                        writeLog("Cannot get DocumentText!");
                        startSurf();    // retry
                    }
                }
            }
            else if (wbBrowser.DocumentTitle == ptcSites[index, 6])    // count down page
            {
                if (!waitForClick.Enabled)
                {
                    writeLog("waitForClick timer - Start");
                    waitForClick.Interval = int.Parse(ptcSites[index, 7]);
                    waitForClick.Start();

                    if (!autoRefresh.Enabled)
                    {
                        writeLog("autoRefresh timer - Start");
                        autoRefresh.Start();
                    }
                }
            }
            else
            {
                wbBrowser.Navigate(ptcSites[index, 1]); // back to view ads page
            }
        }

        private void waitForClick_Tick(object sender, EventArgs e)
        {
            if (wbBrowser.Document.Window.Frames.Count > 0)
            {
                // find image and autoclick
                HtmlDocument countdownFrame = wbBrowser.Document.Window.Frames[0].Document;
                if (countdownFrame.GetElementById("timer") != null)
                {
                    if (countdownFrame.GetElementById("timer").InnerHtml.Contains("Click"))
                    {
                        string key = countdownFrame.GetElementById("timer").InnerHtml.Substring(6);
                        writeLog("key = " + key);
                        foreach (HtmlElement link in countdownFrame.Links)
                        {
                            writeLog(link.InnerHtml);
                            if (link.InnerHtml.Contains(key))
                            {
                                link.InvokeMember("click");
                                writeLog("---CLICK---");
                                stopWaitForClickTimer();
                                stopAutoFreshTimer();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void stopWaitForClickTimer()
        {
            writeLog("waitForClick timer - Stop");
            waitForClick.Stop();
        }

        private void autoRefresh_Tick(object sender, EventArgs e)
        {
            // auto refresh if program is stopped
            writeLog("Program is stopped => auto refresh");
            stopWaitForClickTimer();
            stopAutoFreshTimer();
            wbBrowser.Refresh();
        }

        private void stopAutoFreshTimer()
        {
            writeLog("autoRefresh timer - Stop");
            autoRefresh.Stop();
        }

        private void writeLog(string logContent)
        {
            if (logToFile)
            {
                File.AppendAllText("AutoClick.log", "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FF") + "] " + logContent + "\n");
            }
            else
            {
                Console.WriteLine(logContent);
            }
        }
    }
}
