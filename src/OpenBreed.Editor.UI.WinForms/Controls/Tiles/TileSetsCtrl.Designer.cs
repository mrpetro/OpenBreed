namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    partial class TileSetsCtrl
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
            this.btnAddTileSet = new System.Windows.Forms.Button();
            this.btnRemoveTileSet = new System.Windows.Forms.Button();
            this.cbxTileSets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddTileSet
            // 
            this.btnAddTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTileSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTileSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddTileSet.Location = new System.Drawing.Point(740, 4);
            this.btnAddTileSet.Name = "btnAddTileSet";
            this.btnAddTileSet.Size = new System.Drawing.Size(21, 21);
            this.btnAddTileSet.TabIndex = 11;
            this.btnAddTileSet.Text = "+";
            this.btnAddTileSet.UseVisualStyleBackColor = true;
            // 
            // btnRemoveTileSet
            // 
            this.btnRemoveTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveTileSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveTileSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRemoveTileSet.Location = new System.Drawing.Point(767, 4);
            this.btnRemoveTileSet.Name = "btnRemoveTileSet";
            this.btnRemoveTileSet.Size = new System.Drawing.Size(21, 21);
            this.btnRemoveTileSet.TabIndex = 10;
            this.btnRemoveTileSet.Text = "-";
            this.btnRemoveTileSet.UseVisualStyleBackColor = true;
            // 
            // cbxTileSets
            // 
            this.cbxTileSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTileSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTileSets.FormattingEnabled = true;
            this.cbxTileSets.Location = new System.Drawing.Point(3, 4);
            this.cbxTileSets.Name = "cbxTileSets";
            this.cbxTileSets.Size = new System.Drawing.Size(731, 21);
            this.cbxTileSets.TabIndex = 9;
            // 
            // TileSetsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddTileSet);
            this.Controls.Add(this.btnRemoveTileSet);
            this.Controls.Add(this.cbxTileSets);
            this.Name = "TileSetsCtrl";
            this.Size = new System.Drawing.Size(791, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddTileSet;
        private System.Windows.Forms.Button btnRemoveTileSet;
        private System.Windows.Forms.ComboBox cbxTileSets;
    }
}
