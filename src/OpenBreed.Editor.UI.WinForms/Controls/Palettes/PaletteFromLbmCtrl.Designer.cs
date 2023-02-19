namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class PaletteFromLbmCtrl
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
            this.btnDataRefBrowser = new System.Windows.Forms.Button();
            this.tbxLbmDataRef = new System.Windows.Forms.TextBox();
            this.grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            this.lblLbmDataRef = new System.Windows.Forms.Label();
            this.ColorSelector = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorSelectorCtrl();
            this.ColorEditor = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl();
            this.grpPalette = new System.Windows.Forms.GroupBox();
            this.grpPaletteFromMapSelection.SuspendLayout();
            this.grpPalette.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDataRefBrowser
            // 
            this.btnDataRefBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataRefBrowser.Location = new System.Drawing.Point(709, 22);
            this.btnDataRefBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDataRefBrowser.Name = "btnDataRefBrowser";
            this.btnDataRefBrowser.Size = new System.Drawing.Size(30, 23);
            this.btnDataRefBrowser.TabIndex = 0;
            this.btnDataRefBrowser.Text = "...";
            this.btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // tbxLbmDataRef
            // 
            this.tbxLbmDataRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxLbmDataRef.Location = new System.Drawing.Point(114, 22);
            this.tbxLbmDataRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbxLbmDataRef.Name = "tbxLbmDataRef";
            this.tbxLbmDataRef.Size = new System.Drawing.Size(587, 23);
            this.tbxLbmDataRef.TabIndex = 1;
            // 
            // grpPaletteFromMapSelection
            // 
            this.grpPaletteFromMapSelection.Controls.Add(this.lblLbmDataRef);
            this.grpPaletteFromMapSelection.Controls.Add(this.btnDataRefBrowser);
            this.grpPaletteFromMapSelection.Controls.Add(this.tbxLbmDataRef);
            this.grpPaletteFromMapSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPaletteFromMapSelection.Location = new System.Drawing.Point(0, 0);
            this.grpPaletteFromMapSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPaletteFromMapSelection.Name = "grpPaletteFromMapSelection";
            this.grpPaletteFromMapSelection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPaletteFromMapSelection.Size = new System.Drawing.Size(747, 57);
            this.grpPaletteFromMapSelection.TabIndex = 3;
            this.grpPaletteFromMapSelection.TabStop = false;
            this.grpPaletteFromMapSelection.Text = "Source data";
            // 
            // lblLbmDataRef
            // 
            this.lblLbmDataRef.AutoSize = true;
            this.lblLbmDataRef.Location = new System.Drawing.Point(7, 27);
            this.lblLbmDataRef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLbmDataRef.Name = "lblLbmDataRef";
            this.lblLbmDataRef.Size = new System.Drawing.Size(95, 15);
            this.lblLbmDataRef.TabIndex = 3;
            this.lblLbmDataRef.Text = "Image reference:";
            // 
            // ColorSelector
            // 
            this.ColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColorSelector.Location = new System.Drawing.Point(4, 134);
            this.ColorSelector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(739, 360);
            this.ColorSelector.TabIndex = 13;
            // 
            // ColorEditor
            // 
            this.ColorEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.ColorEditor.Location = new System.Drawing.Point(4, 19);
            this.ColorEditor.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColorEditor.Name = "ColorEditor";
            this.ColorEditor.Size = new System.Drawing.Size(739, 115);
            this.ColorEditor.TabIndex = 14;
            // 
            // grpPalette
            // 
            this.grpPalette.Controls.Add(this.ColorSelector);
            this.grpPalette.Controls.Add(this.ColorEditor);
            this.grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPalette.Location = new System.Drawing.Point(0, 57);
            this.grpPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPalette.Name = "grpPalette";
            this.grpPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPalette.Size = new System.Drawing.Size(747, 497);
            this.grpPalette.TabIndex = 15;
            this.grpPalette.TabStop = false;
            this.grpPalette.Text = "Palette";
            // 
            // PaletteFromLbmCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpPalette);
            this.Controls.Add(this.grpPaletteFromMapSelection);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "PaletteFromLbmCtrl";
            this.Size = new System.Drawing.Size(747, 554);
            this.grpPaletteFromMapSelection.ResumeLayout(false);
            this.grpPaletteFromMapSelection.PerformLayout();
            this.grpPalette.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.TextBox tbxLbmDataRef;
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.Label lblLbmDataRef;
        private ColorSelectorCtrl ColorSelector;
        private ColorEditorCtrl ColorEditor;
        private System.Windows.Forms.GroupBox grpPalette;
    }
}
