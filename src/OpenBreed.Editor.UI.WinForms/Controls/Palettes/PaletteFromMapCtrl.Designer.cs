namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class PaletteFromMapCtrl
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
            grpPalette = new System.Windows.Forms.GroupBox();
            grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            lblMapBlockName = new System.Windows.Forms.Label();
            lblMapDataRef = new System.Windows.Forms.Label();
            cbxMapBlockName = new System.Windows.Forms.ComboBox();
            btnDataRefBrowser = new System.Windows.Forms.Button();
            tbxMapDataRef = new System.Windows.Forms.TextBox();
            grpPaletteFromMapSelection.SuspendLayout();
            SuspendLayout();
            // 
            // grpPalette
            // 
            grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            grpPalette.Location = new System.Drawing.Point(0, 95);
            grpPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Name = "grpPalette";
            grpPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Size = new System.Drawing.Size(640, 139);
            grpPalette.TabIndex = 17;
            grpPalette.TabStop = false;
            grpPalette.Text = "Palette";
            // 
            // grpPaletteFromMapSelection
            // 
            grpPaletteFromMapSelection.Controls.Add(lblMapBlockName);
            grpPaletteFromMapSelection.Controls.Add(lblMapDataRef);
            grpPaletteFromMapSelection.Controls.Add(cbxMapBlockName);
            grpPaletteFromMapSelection.Controls.Add(btnDataRefBrowser);
            grpPaletteFromMapSelection.Controls.Add(tbxMapDataRef);
            grpPaletteFromMapSelection.Dock = System.Windows.Forms.DockStyle.Top;
            grpPaletteFromMapSelection.Location = new System.Drawing.Point(0, 0);
            grpPaletteFromMapSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPaletteFromMapSelection.Name = "grpPaletteFromMapSelection";
            grpPaletteFromMapSelection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPaletteFromMapSelection.Size = new System.Drawing.Size(640, 95);
            grpPaletteFromMapSelection.TabIndex = 16;
            grpPaletteFromMapSelection.TabStop = false;
            grpPaletteFromMapSelection.Text = "Source data";
            // 
            // lblMapBlockName
            // 
            lblMapBlockName.AutoSize = true;
            lblMapBlockName.Location = new System.Drawing.Point(8, 65);
            lblMapBlockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMapBlockName.Name = "lblMapBlockName";
            lblMapBlockName.Size = new System.Drawing.Size(99, 15);
            lblMapBlockName.TabIndex = 4;
            lblMapBlockName.Text = "Map block name:";
            // 
            // lblMapDataRef
            // 
            lblMapDataRef.AutoSize = true;
            lblMapDataRef.Location = new System.Drawing.Point(8, 27);
            lblMapDataRef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMapDataRef.Name = "lblMapDataRef";
            lblMapDataRef.Size = new System.Drawing.Size(86, 15);
            lblMapDataRef.TabIndex = 3;
            lblMapDataRef.Text = "Map reference:";
            // 
            // cbxMapBlockName
            // 
            cbxMapBlockName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cbxMapBlockName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbxMapBlockName.FormattingEnabled = true;
            cbxMapBlockName.Location = new System.Drawing.Point(115, 61);
            cbxMapBlockName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbxMapBlockName.Name = "cbxMapBlockName";
            cbxMapBlockName.Size = new System.Drawing.Size(1025, 23);
            cbxMapBlockName.TabIndex = 2;
            // 
            // btnDataRefBrowser
            // 
            btnDataRefBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDataRefBrowser.Location = new System.Drawing.Point(1148, 22);
            btnDataRefBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDataRefBrowser.Name = "btnDataRefBrowser";
            btnDataRefBrowser.Size = new System.Drawing.Size(30, 23);
            btnDataRefBrowser.TabIndex = 0;
            btnDataRefBrowser.Text = "...";
            btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // tbxMapDataRef
            // 
            tbxMapDataRef.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbxMapDataRef.Location = new System.Drawing.Point(115, 22);
            tbxMapDataRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbxMapDataRef.Name = "tbxMapDataRef";
            tbxMapDataRef.Size = new System.Drawing.Size(1025, 23);
            tbxMapDataRef.TabIndex = 1;
            // 
            // PaletteFromMapCtrl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpPalette);
            Controls.Add(grpPaletteFromMapSelection);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "PaletteFromMapCtrl";
            Size = new System.Drawing.Size(640, 234);
            grpPaletteFromMapSelection.ResumeLayout(false);
            grpPaletteFromMapSelection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpPalette;
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.Label lblMapBlockName;
        private System.Windows.Forms.Label lblMapDataRef;
        private System.Windows.Forms.ComboBox cbxMapBlockName;
        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.TextBox tbxMapDataRef;
    }
}
