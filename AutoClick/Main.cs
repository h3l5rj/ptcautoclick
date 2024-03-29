﻿using System;
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

        // Send a message to window.
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_CLOSE = 0xF060;

        private Boolean logToFile = true;

        private static uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "http://www.neodollar.com/index.php?view=login", "http://www.neodollar.com/index.php?view=click", "http://www.neodollar.com/gpt.php", "30000"},
            { "http://www.tendollarclick.com/index.php?view=login", "http://www.tendollarclick.com/index.php?view=click", "http://www.tendollarclick.com/gpt.php", "60000"},
            { "http://www.ptcsense.com/index.php?view=login", "http://www.ptcsense.com/index.php?view=click", "http://www.ptcsense.com/gpt.php", "30000"},
            { "http://www.richptc.com/index.php?view=login", "http://www.richptc.com/index.php?view=click", "http://www.richptc.com/gpt.php", "30000"},
            { "http://www.bigmoneyptc.com/index.php?view=login", "http://www.bigmoneyptc.com/index.php?view=click", "http://www.bigmoneyptc.com/gpt.php", "30000"},
            { "http://www.grandptc.com/index.php?view=login", "http://www.grandptc.com/index.php?view=click", "http://www.grandptc.com/gpt.php", "30000"},
            { "http://www.fastcarptc.com/index.php?view=login", "http://www.fastcarptc.com/index.php?view=click", "http://www.fastcarptc.com/gpt.php", "10000"},
            { "http://www.ptcbiz.com/index.php?view=login", "http://www.ptcbiz.com/index.php?view=click", "http://www.ptcbiz.com/gpt.php", "30000"},
            { "http://www.ptcwallet.com/index.php?view=login", "http://www.ptcwallet.com/index.php?view=click", "http://www.ptcwallet.com/gpt.php", "15000"},
            { "http://www.buxinc.com/index.php?view=login", "http://www.buxinc.com/index.php?view=click", "http://www.buxinc.com/gpt.php", "30000"},
            { "http://www.fineptc.com/index.php?view=login", "http://www.fineptc.com/index.php?view=click", "http://www.fineptc.com/gpt.php", "60000"},
            { "http://mysteryptc.com/index.php?view=login", "http://mysteryptc.com/index.php?view=click", "http://mysteryptc.com/gpt.php", "10000"},
            { "http://mysteryclickers.com/index.php?view=login", "http://mysteryclickers.com/index.php?view=click", "http://mysteryclickers.com/gpt.php", "15000"},
            { "http://www.billionaireptc.com/index.php?view=login", "http://www.billionaireptc.com/index.php?view=click", "http://www.billionaireptc.com/gpt.php", "25000"},
            { "http://bestdollarclicks.com/index.php?view=login", "http://bestdollarclicks.com/index.php?view=click", "http://bestdollarclicks.com/gpt.php", "30000"},
            { "http://beachptc.com/index.php?view=login", "http://beachptc.com/index.php?view=click", "http://beachptc.com/gpt.php", "25000"},
            { "http://www.clickforabuck.com/index.php?view=login", "http://www.clickforabuck.com/index.php?view=click", "http://www.clickforabuck.com/gpt.php", "25000"},
            { "http://www.onedollarptc.com/index.php?view=login", "http://www.onedollarptc.com/index.php?view=click", "http://www.onedollarptc.com/gpt.php", "25000"},
            { "http://www.twodollarptc.com/index.php?view=login", "http://www.twodollarptc.com/index.php?view=click", "http://www.twodollarptc.com/gpt.php", "25000"},
            { "http://www.kazariangpt.com/index.php?view=login", "http://www.kazariangpt.com/index.php?view=click", "http://www.kazariangpt.com/gpt.php", "25000"},
            { "http://promoterheavenptc.com/index.php?view=login", "http://promoterheavenptc.com/index.php?view=click", "http://promoterheavenptc.com/gpt.php", "25000"},
            { "http://www.1dollarptc.com/index.php?view=login", "http://www.1dollarptc.com/index.php?view=click", "http://www.1dollarptc.com/gpt.php", "25000"},
            { "login using js", "http://www.dollargpt.com/index.php?view=ads", "http://www.dollargpt.com/index.php?view=surfer", "60000"}
        };
        private const string USERNAME = "tranvinhtruong";
        private const string PASSWORD = "tctlT1005";

        private string url = "";
        private Match matchObj;
        private int i;
        private int max;
        private HtmlElement link;
        private bool needStartWaitForClickTimer = false;
        private HtmlDocument countdownFrame;
        private string previousAds = "";

        public Main()
        {
            InitializeComponent();

            startSurf();
        }

        private void startSurf()
        {
            writeLog(index + ": " + ptcSites[index, 1]);

            if (index == 6 || index == 15 || index == 16) // Fast Car PTC, Beach PTC, Click For A Buck PTC
            {
                writeLog("INFORMATION: Waiting for payment before continue!!!");
                surfNextSite();
            }
            else
            {
                wbBrowser.Navigate(ptcSites[index, 1]); // view ads page
            }
        }

        private void wbBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // always start autoRefresh timer on wbBrowser_Navigating
            startAutoFreshTimer();
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wbBrowser.ReadyState == WebBrowserReadyState.Interactive || wbBrowser.ReadyState == WebBrowserReadyState.Complete)
                {
                    // restart autoRefresh timer
                    startAutoFreshTimer();

                    url = wbBrowser.Url.ToString();

                    if (url.StartsWith(ptcSites[index, 0])) // log in page
                    {
                        if (index == 11) // Mystery PTC
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
                        else if (index == 6) // Fast Car PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(599, 641)).InvokeMember("click");
                        }
                        else if (index == 7) // PTC Biz
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(858, 335)).InvokeMember("click");
                        }
                        else if (index == 8) // PTC Wallet
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(508, 454)).InvokeMember("click");
                        }
                        else if (index == 9) // Bux Inc
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(901, 330)).InvokeMember("click");
                        }
                        else if (index == 10) // Fine PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(571, 412)).InvokeMember("click");
                        }
                        else if (index == 11) // Mystery PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(715, 491)).InvokeMember("click");
                        }
                        else if (index == 12) // Mystery Clickers PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(916, 106)).InvokeMember("click");
                        }
                        else if (index == 13) // Billionaire PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(600, 298)).InvokeMember("click");
                        }
                        else if (index == 14) // Best Dollar Clicks
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(599, 467)).InvokeMember("click");
                        }
                        else if (index == 15) // Beach PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(600, 648)).InvokeMember("click");
                        }
                        else if (index == 16) // Click For A Buck PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(599, 337)).InvokeMember("click");
                        }
                        else if (index == 17) // onedollarptc
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(588, 342)).InvokeMember("click");
                        }
                        else if (index == 18) // Two Dollar PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(582, 432)).InvokeMember("click");
                        }
                        else if (index == 19) // Kazarian PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(600, 489)).InvokeMember("click");
                        }
                        else if (index == 20) // Promoter Heaven PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(599, 475)).InvokeMember("click");
                        }
                        else if (index == 21) // 1 Dollar PTC
                        {
                            wbBrowser.Document.GetElementFromPoint(new Point(603, 557)).InvokeMember("click");
                        }
                        writeLog("Login ...");
                    }
                    else if (url.StartsWith(ptcSites[index, 1]))    // view ads page
                    {
                        if (!wbBrowser.DocumentText.Contains("Logout") && !wbBrowser.DocumentText.Contains("LogOut"))    // not logged in
                        {
                            if (index == 22) // Dollar GPT
                            {
                                wbBrowser.Document.InvokeScript("ptcevolution_login");
                                wbBrowser.Document.GetElementById("username").SetAttribute("value", USERNAME);
                                wbBrowser.Document.GetElementById("password").SetAttribute("value", PASSWORD);
                                wbBrowser.Document.InvokeScript("loginform");
                                writeLog("Login ...");
                            }
                            else
                            {
                                wbBrowser.Navigate(ptcSites[index, 0]);    // open log in page
                            }
                        }
                        else
                        {
                            if (index == 22) // Dollar GPT
                            {
                                matchObj = Regex.Match(wbBrowser.DocumentText, "(?<=a href=\"index.php\\?view=surfer)[^\"]*");
                            }
                            else
                            {
                                matchObj = Regex.Match(wbBrowser.DocumentText, "(?<=a href=\"gpt.php)[^\"]*");
                            }
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
                                        if (!previousAds.Contains(link.InnerHtml)
                                            && !(link.InnerHtml.Equals("New Ptc!! Rapidobux!!")
                                            || link.InnerHtml.Equals("**the Power Behind Ebusiness** ")
                                            || link.InnerHtml.Equals("Surf These Links")
                                            || link.InnerHtml.Equals("18 Carats")
                                            || link.InnerHtml.Equals("** Do Not Call List Creates A High-paying Job !...")
                                            || link.InnerHtml.Equals("Gagnez De Largent Le Plus Simplement Du Monde Avec...")
                                            || link.InnerHtml.Equals("Real Income For Free")
                                            || link.InnerHtml.Equals("Absolutely Free Money")
                                            || link.InnerHtml.Equals("The Underground Super Affilaite")
                                            || link.InnerHtml.Equals("He Can Turn You Into One Of Ebay")
                                            || link.InnerHtml.StartsWith("Discover The System That Makes Me")
                                            || link.InnerHtml.Equals("Worldwide-cash")
                                            || link.InnerHtml.Equals("Big Money")
                                            || link.InnerHtml.Equals("New Site")
                                            || link.InnerHtml.Equals("5 Dollars Bonus")
                                            || link.InnerHtml.Equals("Newquay Hair Do!!")
                                            || link.InnerHtml.Equals("100 Usd Bonus")
                                            || link.InnerHtml.Equals("Best Site")
                                            || link.InnerHtml.Equals("Real Cash")
                                            || link.InnerHtml.Equals("Gold 100%")
                                            || (link.InnerHtml.Equals("Earn Online") && index != 9)
                                            || link.InnerHtml.Equals("Instant Money")
                                            || link.InnerHtml.Equals("Be Rich")
                                            || link.InnerHtml.Equals("Click It And Make Money")
                                            || link.InnerHtml.Equals("Free Upgrade")
                                            || link.InnerHtml.Equals("Click Now")
                                            || link.InnerHtml.Equals("Money")
                                            || link.InnerHtml.Equals("Super Money")
                                            || link.InnerHtml.Equals("Mega Cash")
                                            || link.InnerHtml.Equals("Make Money")
                                            || link.InnerHtml.Equals("Free Money")
                                            || link.InnerHtml.Equals("Big Cash")
                                            || link.InnerHtml.Equals("Lavoro Online")
                                            || link.InnerHtml.Equals("10 Usd For Click It")
                                            || link.InnerHtml.Equals("500 Dollars For You")
                                            || link.InnerHtml.Equals("Instant Bonus")
                                            || link.InnerHtml.Equals("Instant Cash")
                                            || link.InnerHtml.Equals("Newly Devised Highly Tested Guaranteed Money Maker")
                                            || link.InnerHtml.Equals("15 Adaily Share 90%")
                                            || link.InnerHtml.Equals("Pays Instanly")
                                            || link.InnerHtml.Equals("Best Performing Forex Product On The Planet")
                                            || link.InnerHtml.Equals("Discover The #1 Way To Slapp Google")
                                            || link.InnerHtml.Equals("Free Site Signup")
                                            || link.InnerHtml.Equals("Every Week $20 Free")
                                            || link.InnerHtml.Equals("Download Free Metal Music!")
                                            || link.InnerHtml.Equals("Ptp4ever")
                                            || link.InnerHtml.Equals("Registrate En Neopays.com Si Paga")
                                            || link.InnerHtml.Equals("Earn By Sharing Your Files!! Great Cashouts!!")
                                            || link.InnerHtml.Equals("Cash For Young And Dumb")
                                            || link.InnerHtml.Equals("Join This Great Site!")
                                            || link.InnerHtml.Equals("Mysitesubmit ")
                                            || link.InnerHtml.Equals("Ourboundofjoy")
                                            || link.InnerHtml.Equals("Freelivead.info")
                                            || link.InnerHtml.Equals("Power Bar Club")
                                            || link.InnerHtml.Equals("Earn By Sharing Your Files!! Great Cashouts!! ")
                                            || link.InnerHtml.Equals("Supreme 2 X 2")
                                            || link.InnerHtml.Equals("20 Minute Pay")
                                            || link.InnerHtml.Equals("Mobile Monopoly")
                                            || link.InnerHtml.StartsWith("Dizee Ptp: Incredible Cpm Rate With Honnest And Pa")
                                            || link.InnerHtml.Equals("Gptcash4")
                                            || link.InnerHtml.StartsWith("Sexy Ptp")
                                            || link.InnerHtml.Equals("Fashion Ptp")
                                            || link.InnerHtml.Equals("Neteuros Ptp")
                                            || link.InnerHtml.Equals("Absolute-ptp")))
                                        {
                                            previousAds += link.InnerHtml;
                                            needStartWaitForClickTimer = true;
                                            writeLog("link.InnerHtml: " + link.InnerHtml);
                                            waitForClick.Stop();    // make sure waitForClick timer is stopped
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
                    else if (url.StartsWith(ptcSites[index, 2]) && needStartWaitForClickTimer)    // count down page
                    {
                        if (wbBrowser.DocumentText.Contains("You Have Already Clicked This Link Today"))
                        {
                            startSurf();
                        }
                        else if (!waitForClick.Enabled)
                        {
                            writeLog("waitForClick timer - Start");
                            waitForClick.Interval = (int)(int.Parse(ptcSites[index, 3]) * 1.2);
                            waitForClick.Start();
                        }
                    }
                    else
                    {
                        startSurf();
                    }
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
            previousAds = "";
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
                if (index == 22) // Dollar GPT
                {
                    waitForClick.Interval = 3000;

                    // click and check if it's a correct picture
                    if (wbBrowser.Document.GetElementById("surfbar").InnerHtml.Equals("You must wait 0 seconds before opening another link"))
                    {
                        // click first picture
                        wbBrowser.Document.GetElementFromPoint(new Point(542, 33)).InvokeMember("click");
                    }
                    else if (wbBrowser.Document.GetElementById("surfbar").InnerHtml.Equals("Invalid Image Verification"))
                    {
                        writeLog("Clicked wrong picture ==> Reload...");
                        waitForClick.Interval = (int)(int.Parse(ptcSites[index, 3]) * 1.2);
                        wbBrowser.Refresh();
                    }
                    else if (wbBrowser.Document.GetElementById("surfbar").InnerHtml.StartsWith("Account credited."))
                    {
                        writeLog("---CLICK---");
                        stopWaitForClickTimer();
                        stopAutoFreshTimer();

                        startSurf();
                    }
                }
                else
                {
                    if (wbBrowser.Document.Window.Frames.Count > 0)
                    {
                        // find image and autoclick
                        countdownFrame = wbBrowser.Document.Window.Frames[0].Document;
                        if (countdownFrame.GetElementById("timer") != null)
                        {
                            if (countdownFrame.GetElementById("timer").InnerHtml.StartsWith("Click"))
                            {
                                string key = countdownFrame.GetElementById("timer").InnerHtml.Substring(6);
                                foreach (HtmlElement link in countdownFrame.Links)
                                {
                                    if (link.InnerHtml.Contains("clickimages/" + key + "."))
                                    {
                                        link.InvokeMember("click");
                                        link.InvokeMember("click");
                                        writeLog("---CLICK---");
                                        stopWaitForClickTimer();
                                        stopAutoFreshTimer();
                                        break;
                                    }
                                }
                            }
                            else if (countdownFrame.GetElementById("timer").InnerHtml.Equals("Loading"))
                            {
                                wbBrowser.Refresh();
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

        private void startAutoFreshTimer()
        {
            if (autoRefresh.Enabled)
            {
                autoRefresh.Stop();
            }
            autoRefresh.Interval = (int)(int.Parse(ptcSites[index, 3]) * 2.5);
            autoRefresh.Start();
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
                // close popup using API
                SendMessage(thisHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
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
