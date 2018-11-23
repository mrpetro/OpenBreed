namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class SpriteSetsView
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
            this.SpriteSets = new OpenBreed.Editor.UI.WinForms.Controls.Sprites.SpriteSetViewerCtrl();
            this.SpriteViewer = new OpenBreed.Editor.UI.WinForms.Controls.Sprites.SpriteViewerCtrl();
            this.SuspendLayout();
            // 
            // SpriteSets
            // 
            this.SpriteSets.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpriteSets.Location = new System.Drawing.Point(0, 0);
            this.SpriteSets.Name = "SpriteSets";
            this.SpriteSets.Size = new System.Drawing.Size(512, 28);
            this.SpriteSets.TabIndex = 0;
            // 
            // SpriteViewer
            // 
            this.SpriteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpriteViewer.Location = new System.Drawing.Point(0, 28);
            this.SpriteViewer.Name = "SpriteViewer";
            this.SpriteViewer.Size = new System.Drawing.Size(512, 403);
            this.SpriteViewer.TabIndex = 1;
            // 
            // SpriteSetsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 431);
            this.Controls.Add(this.SpriteViewer);
            this.Controls.Add(this.SpriteSets);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "SpriteSetsView";
            this.Text = "SpriteSetView";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Sprites.SpriteSetViewerCtrl SpriteSets;
        private Controls.Sprites.SpriteViewerCtrl SpriteViewer;
    }
}