using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClick
{
    public partial class Main : Form
    {
        // Get a handle to an application window.
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private Boolean logToFile = true;

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
                "Grand PTC : Log In", "Grand PTC : My Account Panel - Tran Vinh Truong" , "Grand PTC : Get Paid To Click", "Viewing Ad @ Grand PTC", "35000"},
            { "http://www.ptcbiz.com/index.php?view=login", "http://www.ptcbiz.com/index.php?view=click", "http://www.ptcbiz.com/gpt.php",
                "PTC Biz : Log In", "PTC Biz : My Account Panel - Tran Vinh Truong" , "PTC Biz : Get Paid To Click", "Viewing Ad @ PTC Biz", "35000"},
            { "http://www.ptcwallet.com/index.php?view=login", "http://www.ptcwallet.com/index.php?view=click", "http://www.ptcwallet.com/gpt.php",
                "PTC Wallet : Log In", "PTC Wallet : My Account Panel - Tran Vinh Truong" , "PTC Wallet : Get Paid To Click", "Viewing Ad @ PTC Wallet", "15000"},
            { "http://www.buxinc.com/index.php?view=login", "http://www.buxinc.com/index.php?view=click", "http://www.buxinc.com/gpt.php",
                "Bux Inc : Log In", "Bux Inc : My Account Panel - Tran Vinh Truong" , "Bux Inc : Get Paid To Click", "Viewing Ad @ Bux Inc", "35000"},
            { "http://www.fineptc.com/index.php?view=login", "http://www.fineptc.com/index.php?view=click", "http://www.fineptc.com/gpt.php",
                "Fine PTC : Log In", "Fine PTC : My Account Panel - Tran Vinh Truong" , "Fine PTC : Get Paid To Click", "Viewing Ad @ Fine PTC", "65000"},
            { "http://mysteryptc.com/index.php?view=login", "http://mysteryptc.com/index.php?view=click", "http://mysteryptc.com/gpt.php",
                "Mystery PTC Site : Log In", "Mystery PTC Site : My Account Panel - Tran Vinh Truong" , "Mystery PTC Site : Get Paid To Click", "Viewing Ad @ Mystery PTC Site", "15000"},
            { "http://mysteryclickers.com/index.php?view=login", "http://mysteryclickers.com/index.php?view=click", "http://mysteryclickers.com/gpt.php",
                "Mystery Clickers PTC Site : Log In", "Mystery Clickers PTC Site : My Account Panel - Tran Vinh Truong" , "Mystery Clickers PTC Site : Get Paid To Click", "Viewing Ad @ Mystery Clickers PTC Site", "15000"},
            { "http://beachptc.com/index.php?view=login", "http://beachptc.com/index.php?view=click", "http://beachptc.com/gpt.php",
                "Beach PTC Site : Log In", "Beach PTC Site : My Account Panel - Tran Vinh Truong" , "Beach PTC Site : Get Paid To Click", "Viewing Ad @ Beach PTC Site", "15000"},
            { "http://www.billionaireptc.com/index.php?view=login", "http://www.billionaireptc.com/index.php?view=click", "http://www.billionaireptc.com/gpt.php",
                "Billionaire PTC Site : Log In", "Billionaire PTC Site : My Account Panel - Tran Vinh Truong" , "Billionaire PTC Site : Get Paid To Click", "Viewing Ad @ Billionaire PTC Site", "30000"},
            { "http://www.clickforabuck.com/index.php?view=login", "http://www.clickforabuck.com/index.php?view=click", "http://www.clickforabuck.com/gpt.php",
                "Click For A Buck PTC Site : Log In", "Click For A Buck PTC Site : My Account Panel - Tran Vinh Truong" , "Click For A Buck PTC Site : Get Paid To Click", "Viewing Ad @ Click For A Buck PTC Site", "30000"}/*,
            { "http://www.onedollarptc.com/index.php?view=login", "http://www.onedollarptc.com/index.php?view=click", "http://www.onedollarptc.com/gpt.php",
                "Bible Company : Log In", "Bible Company : My Account Panel - Tran Vinh Truong" , "Bible Company : Get Paid To Click", "Viewing Ad @ Bible Company", "30000"},
            { "http://bestdollarclicks.com/index.php?view=login", "http://bestdollarclicks.com/index.php?view=click", "http://bestdollarclicks.com/gpt.php",
                "Bible Company : Log In", "Bible Company : Purchase Advertising" , "Bible Company : Get Paid To Click", "Viewing Ad @ Bible Company", "65000"}*/
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
            try
            {
                wbBrowser.Navigate(ptcSites[index, 1]);
            }
            catch (Exception)
            {
                writeLog("Exception on navigate because there's a popup is opening.");
                IntPtr thisHandle = FindWindow(null, this.Text);
                SetForegroundWindow(thisHandle);
                SendKeys.Send("{ENTER}");   // press ENTER for closing popup
            }
        }

        private void wbBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!autoRefresh.Enabled)
            {
                writeLog("autoRefresh timer - Start");
                autoRefresh.Interval = 2 * int.Parse(ptcSites[index, 7]) + 5000;
                autoRefresh.Start();
            }
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            writeLog(wbBrowser.DocumentTitle);

            if (wbBrowser.DocumentTitle == ptcSites[index, 3]) // log in page
            {
                if (index == 9) // Mystery PTC
                {
                    wbBrowser.Document.GetElementById("uUsername").SetAttribute("value", USERNAME);
                    wbBrowser.Document.GetElementById("uPassword").SetAttribute("value", PASSWORD);
                }
                else
                {
                    wbBrowser.Document.GetElementById("form_user").SetAttribute("value", USERNAME);
                    wbBrowser.Document.GetElementById("form_pwd").SetAttribute("value", PASSWORD);
                }

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
                else if (index == 9) // Mystery PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(715, 491)).InvokeMember("click");
                }
                else if (index == 10) // Mystery Clickers PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(916, 106)).InvokeMember("click");
                }
                else if (index == 11) // Beach PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(600, 648)).InvokeMember("click");
                }
                else if (index == 12) // Billionaire PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(600, 298)).InvokeMember("click");
                }
                else if (index == 13) // Click For A Buck PTC
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(599, 337)).InvokeMember("click");
                }/*
                else if (index == 14) // onedollarptc
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(586, 341)).InvokeMember("click");
                }
                else if (index == 15) // bestdollarclicks
                {
                    wbBrowser.Document.GetElementFromPoint(new Point(594, 467)).InvokeMember("click");
                }*/
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
                            foreach (HtmlElement link in wbBrowser.Document.Links)
                            {
                                if (link.GetAttribute("href").Contains(matchObj.Value) && link.InnerHtml.Contains("Rapidobux"))
                                {
                                    matchObj = matchObj.NextMatch();
                                }
                            }
                            if (matchObj.Success)
                            {
                                wbBrowser.Navigate(ptcSites[index, 2] + matchObj.Value);
                            }
                            else
                            {
                                index++;
                                if (index >= ptcSites.GetLength(0))   // reset
                                {
                                    index = 0;
                                }
                                startSurf();    // surf next site
                            }
                        }
                        else
                        {
                            index++;
                            if (index >= ptcSites.GetLength(0))   // reset
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
                            if (link.InnerHtml.Contains("/" + key + "."))
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
            if (waitForClick.Enabled)
            {
                writeLog("waitForClick timer - Stop");
                waitForClick.Stop();
            }
        }

        private void autoRefresh_Tick(object sender, EventArgs e)
        {
            stopWaitForClickTimer();
            stopAutoFreshTimer();

            // refresh site if program is stopped
            writeLog("Program is stopped => refresh site <= ################");
            startSurf();    // refresh site
        }

        private void stopAutoFreshTimer()
        {
            if (autoRefresh.Enabled)
            {
                writeLog("autoRefresh timer - Stop");
                autoRefresh.Stop();
            }
        }

        private void writeLog(string logContent)
        {
            if (logToFile)
            {
                File.AppendAllText("AutoClick.log", "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + logContent + "\r\n");
            }
            else
            {
                Console.WriteLine(logContent);
            }
        }
    }
}
