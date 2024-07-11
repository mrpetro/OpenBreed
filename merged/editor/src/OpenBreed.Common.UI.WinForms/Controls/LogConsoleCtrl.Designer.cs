namespace OpenBreed.Common.UI.WinForms.Controls
{
    partial class LogConsoleCtrl
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
            this.tbxConsole = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tbxConsole
            // 
            this.tbxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxConsole.Location = new System.Drawing.Point(0, 0);
            this.tbxConsole.Name = "tbxConsole";
            this.tbxConsole.ReadOnly = true;
            this.tbxConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.tbxConsole.Size = new System.Drawing.Size(392, 136);
            this.tbxConsole.TabIndex = 0;
            this.tbxConsole.Text = "";
            // 
            // LogConsoleCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxConsole);
            this.Name = "LogConsoleCtrl";
            this.Size = new System.Drawing.Size(392, 136);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbxConsole;
    }
}
