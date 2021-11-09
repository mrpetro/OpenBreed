namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    partial class TileSetFromBlkEditorCtrl
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
            this.btnImport = new System.Windows.Forms.Button();
            this.cbxPalettes = new System.Windows.Forms.ComboBox();
            this.lblPalettes = new System.Windows.Forms.Label();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.TileSetViewer = new OpenBreed.Editor.UI.WinForms.Controls.Tiles.TileSetViewerCtrl();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.CurrentIndexInfoLbl = new OpenBreed.Common.UI.WinForms.Controls.ToolStripStatusLabelEx();
            this.grpTools.SuspendLayout();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(6, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 21);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbxPalettes
            // 
            this.cbxPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPalettes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPalettes.FormattingEnabled = true;
            this.cbxPalettes.Location = new System.Drawing.Point(325, 20);
            this.cbxPalettes.Name = "cbxPalettes";
            this.cbxPalettes.Size = new System.Drawing.Size(149, 21);
            this.cbxPalettes.TabIndex = 2;
            // 
            // lblPalettes
            // 
            this.lblPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPalettes.AutoSize = true;
            this.lblPalettes.Location = new System.Drawing.Point(229, 23);
            this.lblPalettes.Name = "lblPalettes";
            this.lblPalettes.Size = new System.Drawing.Size(90, 13);
            this.lblPalettes.TabIndex = 3;
            this.lblPalettes.Text = "Example palettes:";
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.btnImport);
            this.grpTools.Controls.Add(this.lblPalettes);
            this.grpTools.Controls.Add(this.cbxPalettes);
            this.grpTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTools.Location = new System.Drawing.Point(0, 0);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(480, 49);
            this.grpTools.TabIndex = 4;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Tools";
            // 
            // TileSetViewer
            // 
            this.TileSetViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TileSetViewer.Location = new System.Drawing.Point(0, 49);
            this.TileSetViewer.Name = "TileSetViewer";
            this.TileSetViewer.Size = new System.Drawing.Size(480, 247);
            this.TileSetViewer.TabIndex = 0;
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentIndexInfoLbl});
            this.Status.Location = new System.Drawing.Point(0, 296);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(480, 24);
            this.Status.TabIndex = 5;
            this.Status.Text = "statusStrip1";
            // 
            // CurrentIndexInfoLbl
            // 
            this.CurrentIndexInfoLbl.AutoSize = false;
            this.CurrentIndexInfoLbl.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.CurrentIndexInfoLbl.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.CurrentIndexInfoLbl.Name = "CurrentIndexInfoLbl";
            this.CurrentIndexInfoLbl.Size = new System.Drawing.Size(100, 19);
            this.CurrentIndexInfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TileSetFromBlkEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TileSetViewer);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.grpTools);
            this.Name = "TileSetFromBlkEditorCtrl";
            this.Size = new System.Drawing.Size(480, 320);
            this.grpTools.ResumeLayout(false);
            this.grpTools.PerformLayout();
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ComboBox cbxPalettes;
        private System.Windows.Forms.Label lblPalettes;
        private System.Windows.Forms.GroupBox grpTools;
        private TileSetViewerCtrl TileSetViewer;
        private System.Windows.Forms.StatusStrip Status;
        private OpenBreed.Common.UI.WinForms.Controls.ToolStripStatusLabelEx CurrentIndexInfoLbl;
    }
}
