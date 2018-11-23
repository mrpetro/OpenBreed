namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    partial class TileSetPaletteSelectorCtrl
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
            this.cbxTileSetPalettes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbxTileSetPalettes
            // 
            this.cbxTileSetPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTileSetPalettes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTileSetPalettes.FormattingEnabled = true;
            this.cbxTileSetPalettes.Location = new System.Drawing.Point(3, 4);
            this.cbxTileSetPalettes.Name = "cbxTileSetPalettes";
            this.cbxTileSetPalettes.Size = new System.Drawing.Size(785, 21);
            this.cbxTileSetPalettes.TabIndex = 18;
            // 
            // TileSetPaletteSelectorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbxTileSetPalettes);
            this.Name = "TileSetPaletteSelectorCtrl";
            this.Size = new System.Drawing.Size(791, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxTileSetPalettes;
    }
}
