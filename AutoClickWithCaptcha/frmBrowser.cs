using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClickWithCaptcha
{
    public partial class frmBrowser : Form
    {
        private string siteName;
        private string loginUrl;
        private string clickadsUrl;
        private int countdown;

        public frmBrowser(string siteName, string loginUrl, string clickadsUrl, int countdown)
        {
            InitializeComponent();

            this.siteName = siteName;
            this.loginUrl = loginUrl;
            this.clickadsUrl = clickadsUrl;
            this.countdown = countdown;

            Util.writeLog(siteName, loginUrl);
            wbBrowser.Navigate(loginUrl);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wbBrowser.ReadyState == WebBrowserReadyState.Complete)
                {
                    if (wbBrowser.Document.Body.InnerHtml.Contains("Log off"))     // logged in
                    {
                        Match matchObj = Regex.Match(wbBrowser.Document.Body.InnerHtml, "(?<=openad\\(\")[^\"]*");
                        Util.writeLog(siteName, "link available to click? - " + matchObj.Success);
                        if (matchObj.Success)
                        {
                            wbBrowser.Navigate(clickadsUrl + "?h=" + matchObj.Value);
                            startClicker(countdown);
                        }
                    }
                    else
                    {
                        if (wbBrowser.Document.Body.InnerHtml.Contains("Enter security code"))
                        {
                            // input login info and wait for input captcha
                            wbBrowser.Document.GetElementById("username").SetAttribute("value", Util.USERNAME);
                            wbBrowser.Document.GetElementById("password").SetAttribute("value", Util.PASSWORD);
                            wbBrowser.Document.GetElementById("captcha").InvokeMember("focus");
                            Util.writeLog(siteName, "Waiting for input security code ...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.writeLog(siteName, "Exception on DocumentCompleted: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void clicker_Tick(object sender, EventArgs e)
        {
            try
            {
                if (wbBrowser.Document.Window.Frames.Count > 0)
                {
                    clicker.Interval = 3000;

                    // click and check if it's a correct picture
                    if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml) == "Loading...")
                    {
                        // always click the 4th picture
                        wbBrowser.Document.GetElementFromPoint(new Point(813, 27)).InvokeMember("click");
                    }
                    else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("You did not click the right picture. Please reload this page."))
                    {
                        Util.writeLog(siteName, "Clicked wrong picture ==> Reload...");
                        clicker.Interval = countdown;
                        wbBrowser.Refresh();
                    }
                    else if ((wbBrowser.Document.GetElementById("captcharesultdiv").InnerHtml).Contains("Your balance has been credited."))
                    {
                        Util.writeLog(siteName, "---CREDITED---");
                        stopClicker();

                        viewAds();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.writeLog(siteName, "Exception on clicker_Tick: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void viewAds()
        {
            Util.writeLog(siteName, clickadsUrl);
            wbBrowser.Navigate(clickadsUrl);    // open view ads page
        }

        private void startClicker(int interval)
        {
            if (clicker.Enabled)
            {
                clicker.Stop();
            }
            Util.writeLog(siteName, "clicker timer - Start");
            clicker.Interval = interval;
            clicker.Start();
        }

        private void stopClicker()
        {
            if (clicker.Enabled)
            {
                Util.writeLog(siteName, "clicker timer - Stop");
                clicker.Stop();
            }
        }
    }
}
