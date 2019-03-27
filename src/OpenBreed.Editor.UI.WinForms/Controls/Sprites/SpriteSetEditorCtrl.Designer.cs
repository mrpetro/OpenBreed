namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteSetEditorCtrl
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
            this.SpriteSetViewer = new OpenBreed.Editor.UI.WinForms.Controls.Sprites.SpriteSetViewerCtrl();
            this.btnImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SpriteSetViewer
            // 
            this.SpriteSetViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpriteSetViewer.Location = new System.Drawing.Point(3, 32);
            this.SpriteSetViewer.Name = "SpriteSetViewer";
            this.SpriteSetViewer.Size = new System.Drawing.Size(575, 481);
            this.SpriteSetViewer.TabIndex = 0;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // SpriteSetEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.SpriteSetViewer);
            this.Name = "SpriteSetEditorCtrl";
            this.Size = new System.Drawing.Size(581, 513);
            this.ResumeLayout(false);

        }

        #endregion

        private SpriteSetViewerCtrl SpriteSetViewer;
        private System.Windows.Forms.Button btnImport;
    }
}
