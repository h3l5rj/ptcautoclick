namespace AutoClickWithCaptcha
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
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.switcher = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.ShowItemToolTips = false;
            this.toolbar.Size = new System.Drawing.Size(1016, 0);
            this.toolbar.TabIndex = 1;
            // 
            // switcher
            // 
            this.switcher.Enabled = true;
            this.switcher.Interval = 15000;
            this.switcher.Tick += new System.EventHandler(this.switcher_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.toolbar);
            this.IsMdiContainer = true;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PTC AutoClick With Captcha";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.Timer switcher;

    }
}

