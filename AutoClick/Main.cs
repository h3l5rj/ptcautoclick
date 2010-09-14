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
            { "http://www.neodollar.com/index.php?view=login", "http://www.neodollar.com/index.php?view=click", "http://www.neodollar.com/gpt.php",
                "NeoDollar : Log In", "NeoDollar : My Account Panel - Tran Vinh Truong" , "NeoDollar : Get Paid To Click", "Viewing Ad @ NeoDollar", "30000"},
            { "http://www.tendollarclick.com/index.php?view=login", "http://www.tendollarclick.com/index.php?view=click", "http://www.tendollarclick.com/gpt.php",
                "Ten Dollar Click : Log In", "Ten Dollar Click : My Account Panel - Tran Vinh Truong" , "Ten Dollar Click : Get Paid To Click", "Viewing Ad @ Ten Dollar Click", "60000"},
            { "http://www.ptcsense.com/index.php?view=login", "http://www.ptcsense.com/index.php?view=click", "http://www.ptcsense.com/gpt.php",
                "PTC Sense : Log In", "PTC Sense : My Account Panel - Tran Vinh Truong" , "PTC Sense : Get Paid To Click", "Viewing Ad @ PTC Sense", "30000"},
            { "http://www.richptc.com/index.php?view=login", "http://www.richptc.com/index.php?view=click", "http://www.richptc.com/gpt.php",
                "Rich PTC : Log In", "Rich PTC : My Account Panel - Tran Vinh Truong" , "Rich PTC : Get Paid To Click", "Viewing Ad @ Rich PTC", "30000"},
            { "http://www.bigmoneyptc.com/index.php?view=login", "http://www.bigmoneyptc.com/index.php?view=click", "http://www.bigmoneyptc.com/gpt.php",
                "Big Money PTC : Log In", "Big Money PTC : My Account Panel - Tran Vinh Truong" , "Big Money PTC : Get Paid To Click", "Viewing Ad @ Big Money PTC", "30000"},
            { "http://www.grandptc.com/index.php?view=login", "http://www.grandptc.com/index.php?view=click", "http://www.grandptc.com/gpt.php",
                "Grand PTC : Log In", "Grand PTC : My Account Panel - Tran Vinh Truong" , "Grand PTC : Get Paid To Click", "Viewing Ad @ Grand PTC", "30000"},
            { "http://www.ptcbiz.com/index.php?view=login", "http://www.ptcbiz.com/index.php?view=click", "http://www.ptcbiz.com/gpt.php",
                "PTC Biz : Log In", "PTC Biz : My Account Panel - Tran Vinh Truong" , "PTC Biz : Get Paid To Click", "Viewing Ad @ PTC Biz", "30000"},
            { "http://www.ptcwallet.com/index.php?view=login", "http://www.ptcwallet.com/index.php?view=click", "http://www.ptcwallet.com/gpt.php",
                "PTC Wallet : Log In", "PTC Wallet : My Account Panel - Tran Vinh Truong" , "PTC Wallet : Get Paid To Click", "Viewing Ad @ PTC Wallet", "10000"},
            { "http://www.buxinc.com/index.php?view=login", "http://www.buxinc.com/index.php?view=click", "http://www.buxinc.com/gpt.php",
                "Bux Inc : Log In", "Bux Inc : My Account Panel - Tran Vinh Truong" , "Bux Inc : Get Paid To Click", "Viewing Ad @ Bux Inc", "30000"},
            { "http://www.fineptc.com/index.php?view=login", "http://www.fineptc.com/index.php?view=click", "http://www.fineptc.com/gpt.php",
                "Fine PTC : Log In", "Fine PTC : My Account Panel - Tran Vinh Truong" , "Fine PTC : Get Paid To Click", "Viewing Ad @ Fine PTC", "60000"},
            { "http://mysteryptc.com/index.php?view=login", "http://mysteryptc.com/index.php?view=click", "http://mysteryptc.com/gpt.php",
                "Mystery PTC Site : Log In", "Mystery PTC Site : My Account Panel - Tran Vinh Truong" , "Mystery PTC Site : Get Paid To Click", "Viewing Ad @ Mystery PTC Site", "10000"},
            { "http://mysteryclickers.com/index.php?view=login", "http://mysteryclickers.com/index.php?view=click", "http://mysteryclickers.com/gpt.php",
                "Mystery Clickers PTC Site : Log In", "Mystery Clickers PTC Site : My Account Panel - Tran Vinh Truong" , "Mystery Clickers PTC Site : Get Paid To Click", "Viewing Ad @ Mystery Clickers PTC Site", "10000"},
            { "http://beachptc.com/index.php?view=login", "http://beachptc.com/index.php?view=click", "http://beachptc.com/gpt.php",
                "Beach PTC Site : Log In", "Beach PTC Site : My Account Panel - Tran Vinh Truong" , "Beach PTC Site : Get Paid To Click", "Viewing Ad @ Beach PTC Site", "25000"},
            { "http://www.billionaireptc.com/index.php?view=login", "http://www.billionaireptc.com/index.php?view=click", "http://www.billionaireptc.com/gpt.php",
                "Billionaire PTC Site : Log In", "Billionaire PTC Site : My Account Panel - Tran Vinh Truong" , "Billionaire PTC Site : Get Paid To Click", "Viewing Ad @ Billionaire PTC Site", "25000"},
            { "http://www.clickforabuck.com/index.php?view=login", "http://www.clickforabuck.com/index.php?view=click", "http://www.clickforabuck.com/gpt.php",
                "Click For A Buck PTC Site : Log In", "Click For A Buck PTC Site : My Account Panel - Tran Vinh Truong" , "Click For A Buck PTC Site : Get Paid To Click", "Viewing Ad @ Click For A Buck PTC Site", "25000"}/*,
            { "http://www.onedollarptc.com/index.php?view=login", "http://www.onedollarptc.com/index.php?view=click", "http://www.onedollarptc.com/gpt.php",
                "Bible Company : Log In", "Bible Company : My Account Panel - Tran Vinh Truong" , "Bible Company : Get Paid To Click", "Viewing Ad @ Bible Company", "25000"},
            { "http://bestdollarclicks.com/index.php?view=login", "http://bestdollarclicks.com/index.php?view=click", "http://bestdollarclicks.com/gpt.php",
                "Bible Company : Log In", "Bible Company : Purchase Advertising" , "Bible Company : Get Paid To Click", "Viewing Ad @ Bible Company", "60000"}*/
        };
        private const string USERNAME = "tranvinhtruong";
        private const string PASSWORD = "tctlT1005";

        private Match matchObj;
        private int i;
        private int max;
        private HtmlElement link;
        private bool needStartWaitForClickTimer = false;
        private HtmlDocument countdownFrame;

        public Main()
        {
            InitializeComponent();

            startSurf();
        }

        private void startSurf()
        {
            writeLog(index + ": " + ptcSites[index, 1]);
            wbBrowser.Navigate(ptcSites[index, 1]); // view ads page
        }

        private void wbBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // always start autoRefresh timer on wbBrowser_Navigating
            if (autoRefresh.Enabled)
            {
                autoRefresh.Stop();
            }
            autoRefresh.Interval = (int)(int.Parse(ptcSites[index, 7]) * 2.5);
            autoRefresh.Start();
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wbBrowser.DocumentTitle == ptcSites[index, 3]) // log in page
                {
                    if (index == 10) // Mystery PTC
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
                    if (index == 0) // NeoDollar
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(718, 614)).InvokeMember("click");
                    }
                    if (index == 1) // Ten Dollar Click
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(538, 407)).InvokeMember("click");
                        wbBrowser.Document.GetElementFromPoint(new Point(538, 606)).InvokeMember("click");
                    }
                    else if (index == 2) // PTC Sense
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(703, 392)).InvokeMember("click");
                    }
                    else if (index == 3) // Rich PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(401, 430)).InvokeMember("click");
                    }
                    else if (index == 4) // Big Money PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(499, 408)).InvokeMember("click");
                        wbBrowser.Document.GetElementFromPoint(new Point(505, 321)).InvokeMember("click");
                    }
                    else if (index == 5) // Grand PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(572, 391)).InvokeMember("click");
                        wbBrowser.Document.GetElementFromPoint(new Point(699, 549)).InvokeMember("click");
                    }
                    else if (index == 6) // PTC Biz
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(858, 335)).InvokeMember("click");
                    }
                    else if (index == 7) // PTC Wallet
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(508, 454)).InvokeMember("click");
                    }
                    else if (index == 8) // Bux Inc
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(901, 330)).InvokeMember("click");
                    }
                    else if (index == 9) // Fine PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(571, 412)).InvokeMember("click");
                    }
                    else if (index == 10) // Mystery PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(715, 491)).InvokeMember("click");
                    }
                    else if (index == 11) // Mystery Clickers PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(916, 106)).InvokeMember("click");
                    }
                    else if (index == 12) // Beach PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(600, 648)).InvokeMember("click");
                    }
                    else if (index == 13) // Billionaire PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(600, 298)).InvokeMember("click");
                    }
                    else if (index == 14) // Click For A Buck PTC
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(599, 337)).InvokeMember("click");
                    }/*
                    else if (index == 15) // onedollarptc
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(586, 341)).InvokeMember("click");
                    }
                    else if (index == 16) // bestdollarclicks
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(594, 467)).InvokeMember("click");
                    }*/
                    writeLog("Login ...");
                }
                else if (wbBrowser.DocumentTitle == ptcSites[index, 5])    // view ads page
                {
                    if (wbBrowser.DocumentText.Contains("login"))    // not logged in
                    {
                        wbBrowser.Navigate(ptcSites[index, 0]);    // open log in page
                    }
                    else
                    {
                        matchObj = Regex.Match(wbBrowser.DocumentText, "(?<=a href=\"gpt.php)[^\"]*");
                        writeLog("link available to click? - " + matchObj.Success);
                        if (matchObj.Success)
                        {
                            max = wbBrowser.Document.Links.Count;
                            for (i = 0; i < max; i++)
                            {
                                link = wbBrowser.Document.Links[i];
                                if (link.GetAttribute("href").StartsWith(ptcSites[index, 2]))
                                {
                                    // skip these ads because they are having error
                                    if (!(link.InnerHtml.Equals("New Ptc!! Rapidobux!!") || link.InnerHtml.Equals("**the Power Behind Ebusiness**") || link.InnerHtml.Equals("Surf These Links")
                                        || link.InnerHtml.Equals("18 Carats") || (link.InnerHtml.Equals("Auto Traffic Avalanche") && index == 1)))
                                    {
                                        needStartWaitForClickTimer = true;
                                        wbBrowser.Navigate(link.GetAttribute("href"));
                                        break;
                                    }
                                }
                            }
                            if (i >= max)
                            {
                                writeLog("LINK AVAILABLE TO CLICK BUT SKIPPED!!!");
                                surfNextSite();
                            }
                        }
                        else
                        {
                            surfNextSite();
                        }
                    }
                }
                else if (wbBrowser.Url.ToString().StartsWith(ptcSites[index, 2]) && needStartWaitForClickTimer)    // count down page
                {
                    if (!waitForClick.Enabled)
                    {
                        writeLog("waitForClick timer - Start");
                        waitForClick.Interval = (int)(int.Parse(ptcSites[index, 7]) * 1.2);
                        waitForClick.Start();
                    }
                }
                else
                {
                    startSurf();
                }
            }
            catch (Exception ex)
            {
                writeLog("Exception on wbBrowser_DocumentCompleted: " + ex.Message + "\r\n" + ex.StackTrace);
                startSurf();
            }
        }

        private void surfNextSite()
        {
            index++;
            if (index >= ptcSites.GetLength(0))   // reset
            {
                index = 0;
            }
            startSurf();    // surf next site
        }

        private void waitForClick_Tick(object sender, EventArgs e)
        {
            try
            {
                if (wbBrowser.Document.Window.Frames.Count > 0)
                {
                    // find image and autoclick
                    countdownFrame = wbBrowser.Document.Window.Frames[0].Document;
                    if (countdownFrame.GetElementById("timer") != null)
                    {
                        if (countdownFrame.GetElementById("timer").InnerHtml.Contains("Click"))
                        {
                            string key = countdownFrame.GetElementById("timer").InnerHtml.Substring(6);
                            foreach (HtmlElement link in countdownFrame.Links)
                            {
                                if (link.InnerHtml.Contains("clickimages/" + key + "."))
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
            catch (Exception ex)
            {
                writeLog("Exception on waitForClick_Tick: " + ex.Message + "\r\n" + ex.StackTrace);
                startSurf();
            }
        }

        private void stopWaitForClickTimer()
        {
            if (waitForClick.Enabled)
            {
                writeLog("waitForClick timer - Stop");
                waitForClick.Stop();

                needStartWaitForClickTimer = false;
            }
        }

        private void autoRefresh_Tick(object sender, EventArgs e)
        {
            stopWaitForClickTimer();
            stopAutoFreshTimer();

            // surf next site if program is stopped
            writeLog("WARNING: Program is stopped => surf next site.");
            surfNextSite();    // surf next site
        }

        private void stopAutoFreshTimer()
        {
            if (autoRefresh.Enabled)
            {
                autoRefresh.Stop();
            }
        }

        private void autoClosePopup_Tick(object sender, EventArgs e)
        {
            IntPtr thisHandle = IntPtr.Zero;
            thisHandle = (FindWindow(null, "Windows Internet Explorer") != IntPtr.Zero) ? FindWindow(null, "Windows Internet Explorer") :
                 ((FindWindow(null, "Web Browser") != IntPtr.Zero) ? FindWindow(null, "Web Browser") : FindWindow(null, "Message from webpage"));
            if (thisHandle != IntPtr.Zero)
            {
                writeLog("Popup id: " + thisHandle);
                SetForegroundWindow(thisHandle);
                SendKeys.Send("{ENTER}");   // press ENTER for closing popup
            }
        }

        private void writeLog(string logContent)
        {
            if (logToFile)
            {
                File.AppendAllText("AutoClick_" + DateTime.Now.ToString("yyyyMMdd") + ".log", "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + logContent + "\r\n");
            }
            else
            {
                Console.WriteLine(logContent);
            }
        }
    }
}
