namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class TileSetsView
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
            this.TileSets = new OpenBreed.Editor.UI.WinForms.Controls.Tiles.TileSetsCtrl();
            this.TileSelector = new OpenBreed.Editor.UI.WinForms.Controls.Tiles.TileSetViewerCtrl();
            this.Panel = new System.Windows.Forms.Panel();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TileSets
            // 
            this.TileSets.Dock = System.Windows.Forms.DockStyle.Top;
            this.TileSets.Location = new System.Drawing.Point(0, 0);
            this.TileSets.Name = "TileSets";
            this.TileSets.Size = new System.Drawing.Size(304, 30);
            this.TileSets.TabIndex = 0;
            // 
            // TileSelector
            // 
            this.TileSelector.Location = new System.Drawing.Point(0, 0);
            this.TileSelector.Name = "TileSelector";
            this.TileSelector.Size = new System.Drawing.Size(257, 277);
            this.TileSelector.TabIndex = 1;
            // 
            // Panel
            // 
            this.Panel.AutoScroll = true;
            this.Panel.Controls.Add(this.TileSelector);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel.Location = new System.Drawing.Point(0, 30);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(304, 363);
            this.Panel.TabIndex = 2;
            // 
            // TileSetsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 393);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.TileSets);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "TileSetsView";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Tiles";
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Tiles.TileSetsCtrl TileSets;
        private Controls.Tiles.TileSetViewerCtrl TileSelector;
        private System.Windows.Forms.Panel Panel;
    }
}
