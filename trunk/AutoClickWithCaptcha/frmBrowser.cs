using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClickWithCaptcha
{
    public partial class frmBrowser : Form
    {
        private string loginUrl;
        private string clickadsUrl;
        private string clickadsTitle;
        private int countdown;

        private Match matchObj;

        public frmBrowser(string loginUrl, string clickadsUrl, string clickadsTitle, int countdown)
        {
            InitializeComponent();
            this.loginUrl = loginUrl;
            this.clickadsUrl = clickadsUrl;
            this.clickadsTitle = clickadsTitle;
            this.countdown = countdown;

            wbBrowser.Navigate(loginUrl);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wbBrowser.DocumentText.Contains("Log off"))     // logged in
                {
                    matchObj = Regex.Match(wbBrowser.Document.Body.InnerHtml, "(?<=openad\\(\")[^\"]*");
                    Util.writeLog("link available to click? - " + matchObj.Success);
                    if (matchObj.Success)
                    {
                        wbBrowser.Navigate(clickadsUrl + "?h=" + matchObj.Value);
                    }
                    else
                    {
                        
                    }
                }
                else if (wbBrowser.DocumentTitle == clickadsTitle)  // count down page
                {
                    startWaitForClickTimer(countdown);
                }
                else
                {
                    if (wbBrowser.DocumentText.Contains("Enter security code"))
                    {
                        // input login info and wait for input captcha
                        wbBrowser.Document.GetElementById("username").SetAttribute("value", Util.USERNAME);
                        wbBrowser.Document.GetElementById("password").SetAttribute("value", Util.PASSWORD);
                        wbBrowser.Document.GetElementById("captcha").InvokeMember("focus");
                        Util.writeLog("Waiting for input security code ...");
                        startWaitForClickTimer(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.writeLog("Exception on DocumentCompleted: " + ex.Message + "\r\n" + ex.StackTrace);
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
                    wbBrowser.Document.GetElementFromPoint(new Point(665, 27)).InvokeMember("click");
                }
                else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("You did not click the right picture. Please reload this page."))
                {
                    Util.writeLog("Clicked wrong picture ==> Reload...");
                    waitForClick.Interval = countdown;
                    wbBrowser.Refresh();
                }
                else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("Your balance has been credited."))
                {
                    Util.writeLog("---CREDITED---");
                    stopWaitForClickTimer();

                    wbBrowser.Navigate(clickadsUrl);    // back to view ads page
                }
            }
            else if (wbBrowser.DocumentText.Contains("Your details"))   // account page
            {
                stopWaitForClickTimer();
                wbBrowser.Navigate(clickadsUrl);    // open view ads page
            }
        }

        private void startWaitForClickTimer(int interval)
        {
            if (!waitForClick.Enabled)
            {
                Util.writeLog("waitForClick timer - Start");
                waitForClick.Interval = interval;
                waitForClick.Start();
            }
        }

        private void stopWaitForClickTimer()
        {
            if (waitForClick.Enabled)
            {
                Util.writeLog("waitForClick timer - Stop");
                waitForClick.Stop();
            }
        }
    }
}
