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
            btnDataRefBrowser = new System.Windows.Forms.Button();
            tbxMapDataRef = new System.Windows.Forms.TextBox();
            cbxMapBlockName = new System.Windows.Forms.ComboBox();
            grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            lblMapBlockName = new System.Windows.Forms.Label();
            lblMapDataRef = new System.Windows.Forms.Label();
            ColorSelector = new ColorSelectorCtrl();
            grpPalette = new System.Windows.Forms.GroupBox();
            grpPaletteFromMapSelection.SuspendLayout();
            grpPalette.SuspendLayout();
            SuspendLayout();
            // 
            // btnDataRefBrowser
            // 
            btnDataRefBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDataRefBrowser.Location = new System.Drawing.Point(709, 22);
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
            tbxMapDataRef.Location = new System.Drawing.Point(114, 22);
            tbxMapDataRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbxMapDataRef.Name = "tbxMapDataRef";
            tbxMapDataRef.Size = new System.Drawing.Size(587, 23);
            tbxMapDataRef.TabIndex = 1;
            // 
            // cbxMapBlockName
            // 
            cbxMapBlockName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cbxMapBlockName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbxMapBlockName.FormattingEnabled = true;
            cbxMapBlockName.Location = new System.Drawing.Point(114, 61);
            cbxMapBlockName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbxMapBlockName.Name = "cbxMapBlockName";
            cbxMapBlockName.Size = new System.Drawing.Size(587, 23);
            cbxMapBlockName.TabIndex = 2;
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
            grpPaletteFromMapSelection.Size = new System.Drawing.Size(747, 95);
            grpPaletteFromMapSelection.TabIndex = 3;
            grpPaletteFromMapSelection.TabStop = false;
            grpPaletteFromMapSelection.Text = "Source data";
            // 
            // lblMapBlockName
            // 
            lblMapBlockName.AutoSize = true;
            lblMapBlockName.Location = new System.Drawing.Point(7, 65);
            lblMapBlockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMapBlockName.Name = "lblMapBlockName";
            lblMapBlockName.Size = new System.Drawing.Size(99, 15);
            lblMapBlockName.TabIndex = 4;
            lblMapBlockName.Text = "Map block name:";
            // 
            // lblMapDataRef
            // 
            lblMapDataRef.AutoSize = true;
            lblMapDataRef.Location = new System.Drawing.Point(7, 27);
            lblMapDataRef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMapDataRef.Name = "lblMapDataRef";
            lblMapDataRef.Size = new System.Drawing.Size(86, 15);
            lblMapDataRef.TabIndex = 3;
            lblMapDataRef.Text = "Map reference:";
            // 
            // ColorSelector
            // 
            ColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            ColorSelector.Location = new System.Drawing.Point(4, 19);
            ColorSelector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ColorSelector.Name = "ColorSelector";
            ColorSelector.Size = new System.Drawing.Size(739, 437);
            ColorSelector.TabIndex = 13;
            // 
            // grpPalette
            // 
            grpPalette.Controls.Add(ColorSelector);
            grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            grpPalette.Location = new System.Drawing.Point(0, 95);
            grpPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Name = "grpPalette";
            grpPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Size = new System.Drawing.Size(747, 459);
            grpPalette.TabIndex = 15;
            grpPalette.TabStop = false;
            grpPalette.Text = "Palette";
            // 
            // PaletteFromMapCtrl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpPalette);
            Controls.Add(grpPaletteFromMapSelection);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "PaletteFromMapCtrl";
            Size = new System.Drawing.Size(747, 554);
            grpPaletteFromMapSelection.ResumeLayout(false);
            grpPaletteFromMapSelection.PerformLayout();
            grpPalette.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.TextBox tbxMapDataRef;
        private System.Windows.Forms.ComboBox cbxMapBlockName;
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.Label lblMapBlockName;
        private System.Windows.Forms.Label lblMapDataRef;
        private ColorSelectorCtrl ColorSelector;
        private System.Windows.Forms.GroupBox grpPalette;
    }
}
