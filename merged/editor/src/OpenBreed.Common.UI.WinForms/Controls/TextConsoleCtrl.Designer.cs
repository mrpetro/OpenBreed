namespace OpenBreed.Common.UI.WinForms.Controls
{
    partial class TextConsoleCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TextConsoleCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TextConsoleCtrl";
            this.Size = new System.Drawing.Size(750, 269);
            this.Load += new System.EventHandler(this.TextConsoleCtrl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TextConsoleCtrl_Paint);
            this.Resize += new System.EventHandler(this.TextConsoleCtrl_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
