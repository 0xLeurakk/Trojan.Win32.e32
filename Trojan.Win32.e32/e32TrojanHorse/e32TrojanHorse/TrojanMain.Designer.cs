namespace e32TrojanHorse
{
    partial class TrojanMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.EncryptFilesFromPC = new System.Windows.Forms.Timer(this.components);
            this.NextPayload = new System.Windows.Forms.Timer(this.components);
            this.Finaldestruct = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // EncryptFilesFromPC
            // 
            this.EncryptFilesFromPC.Tick += new System.EventHandler(this.EncryptFilesFromPC_Tick);
            // 
            // NextPayload
            // 
            this.NextPayload.Interval = 9000;
            this.NextPayload.Tick += new System.EventHandler(this.NextPayload_Tick);
            // 
            // Finaldestruct
            // 
            this.Finaldestruct.Tick += new System.EventHandler(this.Finaldestruct_Tick);
            // 
            // TrojanMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(10, 10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrojanMain";
            this.Opacity = 0.01D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "E32";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrojanMain_FormClosing);
            this.Load += new System.EventHandler(this.TrojanMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer EncryptFilesFromPC;
        private System.Windows.Forms.Timer NextPayload;
        private System.Windows.Forms.Timer Finaldestruct;
    }
}

