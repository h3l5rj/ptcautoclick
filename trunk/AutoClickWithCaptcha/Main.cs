using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClickWithCaptcha
{
    public partial class Main : Form
    {
        private Boolean logToFile = false;

        private Boolean loggedIn4All = true;

        private static uint index = 0;
        private string[,] ptcSites = new string[,] {
            { "http://www.buxincomecafe.com/pages/login", "http://www.buxincomecafe.com/pages/clickads", "http://www.buxincomecafe.com/pages/clickads?h=",
                "Click Ads - BuxIncomeCafe.com", "25000"},
            { "http://www.nycebux.com/pages/login", "http://www.nycebux.com/pages/clickads", "http://www.nycebux.com/pages/clickads?h=",
                "Click Ads - NYCE Bux", "35000"}
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
            int i = 0;
            if (loggedIn4All)
            {
                i = 1;
            }
            writeLog(index + ": " + ptcSites[index, i]);
            wbBrowser.Navigate(ptcSites[index, i]);
        }

        private void wbBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (wbBrowser.DocumentText.Contains("Your details"))    // view ads page
            {
                if (!loggedIn4All)
                {
                    index++;
                    if (index >= ptcSites.GetLength(0))     // logged for all sites
                    {
                        writeLog("Logged for all sites! ==> Start viewing ads.");
                        loggedIn4All = true;
                        index = 0;
                    }
                    else
                    {
                        writeLog("Still not logged for all sites! ==> Log in for next site.");
                    }
                    startSurf();    // log in for next site
                }
            }
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wbBrowser.DocumentText.Contains("Log off"))    // logged in
                {
                    matchObj = Regex.Match(wbBrowser.DocumentText, "(?<=openad\\(\")[^\"]*");
                    writeLog("link available to click? - " + matchObj.Success);
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
                else if (wbBrowser.DocumentTitle == ptcSites[index, 3]) // count down page
                {
                    if (!waitForClick.Enabled)
                    {
                        writeLog("waitForClick timer - Start");
                        waitForClick.Interval = int.Parse(ptcSites[index, 4]);
                        waitForClick.Start();
                    }
                }
                else
                {
                    loggedIn4All = false;

                    if (wbBrowser.DocumentText.Contains("Enter security code"))
                    {
                        // input login info and wait for input captcha
                        wbBrowser.Document.GetElementById("username").SetAttribute("value", USERNAME);
                        wbBrowser.Document.GetElementById("password").SetAttribute("value", PASSWORD);
                        wbBrowser.Document.GetElementById("captcha").InvokeMember("focus");
                        writeLog("Waiting for input captcha ...");
                    }
                    else
                    {
                        writeLog("Need to log in all sites before start viewing ads!");
                        startSurf();
                    }
                }
            }
            catch (Exception ex)
            {
                writeLog("Exception on DocumentCompleted: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void waitForClick_Tick(object sender, EventArgs e)
        {
            if (wbBrowser.Document.Window.Frames.Count > 0)
            {
                waitForClick.Interval = 3000;

                // click and check if it's a correct picture
                if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml) == "Loading...")
                {
                    if (index == 0)
                    {
                        wbBrowser.Document.GetElementFromPoint(new Point(665, 27)).InvokeMember("click");
                    }
                }
                else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("You did not click the right picture. Please reload this page."))
                {
                    writeLog("Clicked wrong image ==> reload...");
                    waitForClick.Interval = int.Parse(ptcSites[index, 4]);
                    wbBrowser.Refresh();
                }
                else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("Your balance has been credited."))
                {
                    writeLog("---CREDITED---");
                    stopWaitForClickTimer();

                    wbBrowser.Navigate(ptcSites[index, 1]); // back to view ads page
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
