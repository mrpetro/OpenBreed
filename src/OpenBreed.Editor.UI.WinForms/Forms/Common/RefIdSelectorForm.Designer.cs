namespace OpenBreed.Editor.UI.WinForms.Forms.Common
{
    partial class RefIdSelectorForm
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
            this.Ctrl = new OpenBreed.Editor.UI.WinForms.Controls.Common.EntryRefIdSelectorCtrl();
            this.SuspendLayout();
            // 
            // Ctrl
            // 
            this.Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ctrl.Location = new System.Drawing.Point(0, 0);
            this.Ctrl.Name = "Ctrl";
            this.Ctrl.Size = new System.Drawing.Size(293, 57);
            this.Ctrl.TabIndex = 0;
            // 
            // RefIdSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 57);
            this.Controls.Add(this.Ctrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RefIdSelectorForm";
            this.Text = "RefIdSelectorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Common.EntryRefIdSelectorCtrl Ctrl;
    }
}