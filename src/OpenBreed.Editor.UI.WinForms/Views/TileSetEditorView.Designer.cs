namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class TileSetEditorView
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
            this.TileSetEditor = new OpenBreed.Editor.UI.WinForms.Controls.Tiles.TileSetEditorCtrl();
            this.SuspendLayout();
            // 
            // TileSetEditor
            // 
            this.TileSetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TileSetEditor.Location = new System.Drawing.Point(0, 0);
            this.TileSetEditor.Name = "TileSetEditor";
            this.TileSetEditor.Size = new System.Drawing.Size(802, 410);
            this.TileSetEditor.TabIndex = 1;
            // 
            // TileSetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 410);
            this.Controls.Add(this.TileSetEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "TileSetView";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.Tiles.TileSetEditorCtrl TileSetEditor;
    }
}