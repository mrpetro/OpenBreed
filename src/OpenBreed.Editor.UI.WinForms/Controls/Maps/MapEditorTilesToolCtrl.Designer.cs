namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    partial class MapEditorTilesToolCtrl
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
            this.LayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.TileSetSelector = new OpenBreed.Editor.UI.WinForms.Controls.Maps.MapEditorTileSetSelectorCtrl();
            this.TilesSelector = new OpenBreed.Editor.UI.WinForms.Controls.Maps.MapEditorTilesSelectorCtrl();
            this.LayoutTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutTable
            // 
            this.LayoutTable.ColumnCount = 1;
            this.LayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Controls.Add(this.TileSetSelector, 0, 0);
            this.LayoutTable.Controls.Add(this.TilesSelector, 0, 1);
            this.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutTable.Location = new System.Drawing.Point(0, 0);
            this.LayoutTable.Name = "LayoutTable";
            this.LayoutTable.RowCount = 2;
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Size = new System.Drawing.Size(544, 393);
            this.LayoutTable.TabIndex = 0;
            // 
            // TileSetSelector
            // 
            this.TileSetSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TileSetSelector.Location = new System.Drawing.Point(3, 3);
            this.TileSetSelector.Name = "TileSetSelector";
            this.TileSetSelector.Size = new System.Drawing.Size(538, 28);
            this.TileSetSelector.TabIndex = 0;
            // 
            // TilesSelector
            // 
            this.TilesSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TilesSelector.Location = new System.Drawing.Point(3, 37);
            this.TilesSelector.Name = "TilesSelector";
            this.TilesSelector.Size = new System.Drawing.Size(538, 353);
            this.TilesSelector.TabIndex = 1;
            // 
            // MapEditorTilesToolCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LayoutTable);
            this.Name = "MapEditorTilesToolCtrl";
            this.Size = new System.Drawing.Size(544, 393);
            this.LayoutTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutTable;
        private MapEditorTileSetSelectorCtrl TileSetSelector;
        private MapEditorTilesSelectorCtrl TilesSelector;
    }
}
