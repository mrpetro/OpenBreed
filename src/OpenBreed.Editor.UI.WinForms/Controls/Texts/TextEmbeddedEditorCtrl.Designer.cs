namespace OpenBreed.Editor.UI.WinForms.Controls.Texts
{
    partial class TextEmbeddedEditorCtrl
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
            this.grpText = new System.Windows.Forms.GroupBox();
            this.tbxText = new System.Windows.Forms.TextBox();
            this.grpText.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpText
            // 
            this.grpText.Controls.Add(this.tbxText);
            this.grpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpText.Location = new System.Drawing.Point(0, 0);
            this.grpText.Name = "grpText";
            this.grpText.Size = new System.Drawing.Size(640, 480);
            this.grpText.TabIndex = 16;
            this.grpText.TabStop = false;
            this.grpText.Text = "Text";
            // 
            // tbxText
            // 
            this.tbxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxText.Location = new System.Drawing.Point(3, 16);
            this.tbxText.Multiline = true;
            this.tbxText.Name = "tbxText";
            this.tbxText.Size = new System.Drawing.Size(634, 461);
            this.tbxText.TabIndex = 15;
            // 
            // TextEmbeddedEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpText);
            this.Name = "TextEmbeddedEditorCtrl";
            this.Size = new System.Drawing.Size(640, 480);
            this.grpText.ResumeLayout(false);
            this.grpText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpText;
        private System.Windows.Forms.TextBox tbxText;
    }
}
