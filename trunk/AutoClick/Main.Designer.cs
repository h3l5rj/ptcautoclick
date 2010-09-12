namespace AutoClick
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.waitForClick = new System.Windows.Forms.Timer(this.components);
            this.autoRefresh = new System.Windows.Forms.Timer(this.components);
            this.wbBrowser = new AxSHDocVw.AxWebBrowser();
            this.autoClosePopup = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.wbBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // waitForClick
            // 
            this.waitForClick.Tick += new System.EventHandler(this.waitForClick_Tick);
            // 
            // autoRefresh
            // 
            this.autoRefresh.Enabled = true;
            this.autoRefresh.Tick += new System.EventHandler(this.autoRefresh_Tick);
            // 
            // wbBrowser
            // 
            this.wbBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbBrowser.Enabled = true;
            this.wbBrowser.Location = new System.Drawing.Point(0, 0);
            this.wbBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wbBrowser.OcxState")));
            this.wbBrowser.Size = new System.Drawing.Size(1016, 741);
            this.wbBrowser.TabIndex = 0;
            this.wbBrowser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.wbBrowser_BeforeNavigate2);
            this.wbBrowser.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.wbBrowser_DocumentComplete);
            this.wbBrowser.NewWindow3 += new AxSHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(this.wbBrowser_NewWindow3);
            // 
            // autoClosePopup
            // 
            this.autoClosePopup.Enabled = true;
            this.autoClosePopup.Interval = 1000;
            this.autoClosePopup.Tick += new System.EventHandler(this.autoClosePopup_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.wbBrowser);
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PTC AutoClick";
            ((System.ComponentModel.ISupportInitialize)(this.wbBrowser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer waitForClick;
        private System.Windows.Forms.Timer autoRefresh;
        private AxSHDocVw.AxWebBrowser wbBrowser;
        private System.Windows.Forms.Timer autoClosePopup;

    }
}

