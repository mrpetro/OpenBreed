namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class LogConsoleView
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
            this.LogConsoleCtrl = new OpenBreed.Common.UI.WinForms.Controls.LogConsoleCtrl();
            this.SuspendLayout();
            // 
            // LogConsoleCtrl
            // 
            this.LogConsoleCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogConsoleCtrl.Location = new System.Drawing.Point(0, 0);
            this.LogConsoleCtrl.Name = "LogConsoleCtrl";
            this.LogConsoleCtrl.Size = new System.Drawing.Size(427, 201);
            this.LogConsoleCtrl.TabIndex = 0;
            // 
            // LogConsoleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 201);
            this.Controls.Add(this.LogConsoleCtrl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "LogConsoleView";
            this.Text = "Log Console";

            this.ResumeLayout(false);

        }

        #endregion

        private OpenBreed.Common.UI.WinForms.Controls.LogConsoleCtrl LogConsoleCtrl;
    }
}