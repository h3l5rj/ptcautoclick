using mshtml;
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
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(int hWnd);

        private Boolean logToFile = true;

        private static uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "http://www.neodollar.com/index.php?view=login", "http://www.neodollar.com/index.php?view=click", "http://www.neodollar.com/gpt.php",
                "NeoDollar : Log In", "NeoDollar : My Account Panel - Tran Vinh Truong" , "NeoDollar : Get Paid To Click", "Viewing Ad @ NeoDollar", "30000"},
            { "http://www.tendollarclick.com/index.php?view=login", "http://www.tendollarclick.com/index.php?view=click", "http://www.tendollarclick.com/gpt.php",
                "Ten Dollar Click : Log In", "Ten Dollar Click : My Account Panel - Tran Vinh Truong" , "Ten Dollar Click : Get Paid To Click", "Viewing Ad @ Ten Dollar Click", "60000"},
            { "http://www.ptcsense.com/index.php?view=login", "http://www.ptcsense.com/index.php?view=click", "http://www.ptcsense.com/gpt.php",
                "PTC Sense : Log In", "PTC Sense : My Account Panel - Tran Vinh Truong" , "PTC Sense : Get Paid To Click", "Viewing Ad @ PTC Sense", "30000"},
            { "http://www.richptc.com/index.php?view=login", "http://www.richptc.com/index.php?view=account&ac=click", "http://www.richptc.com/gpt.php",
                "Rich PTC : Log In", "Rich PTC : My Account Panel - Tran Vinh Truong" , "Rich PTC : Get Paid To Click", "Viewing Ad @ Rich PTC", "30000"},
            { "http://www.bigmoneyptc.com/index.php?view=login", "http://www.bigmoneyptc.com/index.php?view=account&ac=click", "http://www.bigmoneyptc.com/gpt.php",
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

        private HTMLDocument doc;
        private string html;
        private Match matchObj;
        private bool needStartWaitForClickTimer = false;

        private int IFlags = 0;
        private object obj = 0;
        private HTMLWindow2 surftopframe;
        private HTMLDocument countdownFrame;

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

        private void wbBrowser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
        {
            startAutoFreshTimer();
        }

        private void wbBrowser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
        {
            try
            {
                doc = (HTMLDocument)wbBrowser.Document;
                html = doc.documentElement.innerHTML;

                if (doc.title == ptcSites[index, 5])    // view ads page
                {
                    if (html.Contains("login"))    // not logged in
                    {
                        wbBrowser.Navigate(ptcSites[index, 0]);    // open log in page
                    }
                    else
                    {
                        matchObj = Regex.Match(html, "(?<=href=\"gpt.php)[^\"]*");
                        if (matchObj.Success)
                        {
                            foreach (IHTMLElement link in doc.links)
                            {
                                if (link.getAttribute("href", IFlags).ToString().Contains(matchObj.Value.Replace("&amp;", "&")))
                                {
                                    // skip these ads because they are having error
                                    if (link.innerHTML.Equals("New Ptc!! Rapidobux!!") || link.innerHTML.Equals("**the Power Behind Ebusiness**") || link.innerHTML.Equals("Adult Help For Online Success"))
                                    {
                                        writeLog("\"" + link.innerHTML + "\" is having error ==> Skip.");
                                        matchObj = matchObj.NextMatch();
                                        break;
                                    }
                                }
                            }
                            if (matchObj.Success)
                            {
                                needStartWaitForClickTimer = true;
                                wbBrowser.Navigate(ptcSites[index, 2] + matchObj.Value.Replace("&amp;", "&"));
                            }
                            else
                            {
                                surfNextSite();
                            }
                        }
                        else
                        {
                            surfNextSite();
                        }
                    }
                }
                else if (doc.title == ptcSites[index, 3]) // log in page
                {
                    if (index == 10) // Mystery PTC
                    {
                        doc.getElementById("uUsername").setAttribute("value", USERNAME, IFlags);
                        doc.getElementById("uPassword").setAttribute("value", PASSWORD, IFlags);
                    }
                    else
                    {
                        doc.getElementById("form_user").setAttribute("value", USERNAME, IFlags);
                        doc.getElementById("form_pwd").setAttribute("value", PASSWORD, IFlags);
                    }

                    // click on "Access Account"
                    if (index == 0) // NeoDollar
                    {
                        doc.elementFromPoint(718, 614).click();
                    }
                    if (index == 1) // Ten Dollar Click
                    {
                        doc.elementFromPoint(538, 407).click();
                        doc.elementFromPoint(538, 606).click();
                    }
                    else if (index == 2) // PTC Sense
                    {
                        doc.elementFromPoint(703, 392).click();
                    }
                    else if (index == 3) // Rich PTC
                    {
                        doc.elementFromPoint(401, 430).click();
                    }
                    else if (index == 4) // Big Money PTC
                    {
                        doc.elementFromPoint(499, 408).click();
                        doc.elementFromPoint(505, 321).click();
                    }
                    else if (index == 5) // Grand PTC
                    {
                        doc.elementFromPoint(572, 391).click();
                        doc.elementFromPoint(699, 549).click();
                    }
                    else if (index == 6) // PTC Biz
                    {
                        doc.elementFromPoint(858, 335).click();
                    }
                    else if (index == 7) // PTC Wallet
                    {
                        doc.elementFromPoint(508, 454).click();
                    }
                    else if (index == 8) // Bux Inc
                    {
                        doc.elementFromPoint(901, 330).click();
                    }
                    else if (index == 9) // Fine PTC
                    {
                        doc.elementFromPoint(571, 412).click();
                    }
                    else if (index == 10) // Mystery PTC
                    {
                        doc.elementFromPoint(715, 491).click();
                    }
                    else if (index == 11) // Mystery Clickers PTC
                    {
                        doc.elementFromPoint(916, 106).click();
                    }
                    else if (index == 12) // Beach PTC
                    {
                        doc.elementFromPoint(600, 648).click();
                    }
                    else if (index == 13) // Billionaire PTC
                    {
                        doc.elementFromPoint(600, 298).click();
                    }
                    else if (index == 14) // Click For A Buck PTC
                    {
                        doc.elementFromPoint(599, 337).click();
                    }
                }
                else if (doc.title == ptcSites[index, 4])    // account page
                {
                    wbBrowser.Navigate(ptcSites[index, 1]); // open view ads page
                }
                else if (doc.url.StartsWith(ptcSites[index, 2]) && needStartWaitForClickTimer)    // count down page
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
                    wbBrowser.Navigate(ptcSites[index, 1]); // back to view ads page
                }
            }
            catch (Exception ex)
            {
                writeLog("Exception on wbBrowser_DocumentComplete: " + ex.Message + "\r\n" + ex.StackTrace);
                startAutoFreshTimer();
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

        private void wbBrowser_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
        {
            // popup blocker
            e.cancel = true;
        }

        private void waitForClick_Tick(object sender, EventArgs e)
        {
            try
            {
                doc = (HTMLDocument)wbBrowser.Document;

                if (doc.frames.length > 0)
                {
                    // find image and autoclick
                    surftopframe = (HTMLWindow2)doc.frames.item(ref obj);
                    countdownFrame = (HTMLDocument)surftopframe.document;
                    if (countdownFrame.getElementById("timer") != null)
                    {
                        if (countdownFrame.getElementById("timer").innerHTML.Contains("Click"))
                        {
                            string key = countdownFrame.getElementById("timer").innerHTML.Substring(6);
                            foreach (IHTMLElement link in countdownFrame.links)
                            {
                                if (link.innerHTML.Contains("clickimages/" + key + "."))
                                {
                                    link.click();
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
                startAutoFreshTimer();
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

        private void startAutoFreshTimer()
        {
            if (autoRefresh.Enabled)
            {
                autoRefresh.Stop();
            }
            autoRefresh.Interval = (int)(int.Parse(ptcSites[index, 7]) * 2.5);
            autoRefresh.Start();
        }

        private void autoRefresh_Tick(object sender, EventArgs e)
        {
            stopWaitForClickTimer();
            stopAutoFreshTimer();

            // refresh site if program is stopped
            writeLog("WARNING: Program is stopped => refresh site.");
            startSurf();    // refresh site
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
            int thisHandle = (FindWindow(null, "Windows Internet Explorer") != 0) ? FindWindow(null, "Windows Internet Explorer") :
                ((FindWindow(null, "Web Browser") != 0) ? FindWindow(null, "Web Browser") : FindWindow(null, "Message from webpage"));
            if (thisHandle != 0)
            {
                writeLog("This ads site is using ExitSplash!");
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
