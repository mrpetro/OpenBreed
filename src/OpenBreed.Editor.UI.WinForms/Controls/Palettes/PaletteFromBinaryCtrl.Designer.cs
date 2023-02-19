namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class PaletteFromBinaryCtrl
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
            this.tbxBinaryDataRef = new System.Windows.Forms.TextBox();
            this.btnDataRefBrowser = new System.Windows.Forms.Button();
            this.grpSourceData = new System.Windows.Forms.GroupBox();
            this.lblBinaryDataStart = new System.Windows.Forms.Label();
            this.numUpDown = new System.Windows.Forms.NumericUpDown();
            this.lblBinaryDataRef = new System.Windows.Forms.Label();
            this.grpPalette = new System.Windows.Forms.GroupBox();
            this.ColorSelector = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorSelectorCtrl();
            this.ColorEditor = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl();
            this.grpSourceData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).BeginInit();
            this.grpPalette.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxBinaryDataRef
            // 
            this.tbxBinaryDataRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxBinaryDataRef.Location = new System.Drawing.Point(105, 22);
            this.tbxBinaryDataRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbxBinaryDataRef.Name = "tbxBinaryDataRef";
            this.tbxBinaryDataRef.Size = new System.Drawing.Size(597, 23);
            this.tbxBinaryDataRef.TabIndex = 3;
            // 
            // btnDataRefBrowser
            // 
            this.btnDataRefBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataRefBrowser.Location = new System.Drawing.Point(709, 22);
            this.btnDataRefBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDataRefBrowser.Name = "btnDataRefBrowser";
            this.btnDataRefBrowser.Size = new System.Drawing.Size(30, 23);
            this.btnDataRefBrowser.TabIndex = 2;
            this.btnDataRefBrowser.Text = "...";
            this.btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // grpSourceData
            // 
            this.grpSourceData.Controls.Add(this.lblBinaryDataStart);
            this.grpSourceData.Controls.Add(this.numUpDown);
            this.grpSourceData.Controls.Add(this.lblBinaryDataRef);
            this.grpSourceData.Controls.Add(this.tbxBinaryDataRef);
            this.grpSourceData.Controls.Add(this.btnDataRefBrowser);
            this.grpSourceData.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSourceData.Location = new System.Drawing.Point(0, 0);
            this.grpSourceData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpSourceData.Name = "grpSourceData";
            this.grpSourceData.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpSourceData.Size = new System.Drawing.Size(747, 81);
            this.grpSourceData.TabIndex = 4;
            this.grpSourceData.TabStop = false;
            this.grpSourceData.Text = "Source data";
            // 
            // lblBinaryDataStart
            // 
            this.lblBinaryDataStart.AutoSize = true;
            this.lblBinaryDataStart.Location = new System.Drawing.Point(7, 53);
            this.lblBinaryDataStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBinaryDataStart.Name = "lblBinaryDataStart";
            this.lblBinaryDataStart.Size = new System.Drawing.Size(60, 15);
            this.lblBinaryDataStart.TabIndex = 6;
            this.lblBinaryDataStart.Text = "Data start:";
            // 
            // numUpDown
            // 
            this.numUpDown.Location = new System.Drawing.Point(105, 51);
            this.numUpDown.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numUpDown.Name = "numUpDown";
            this.numUpDown.Size = new System.Drawing.Size(88, 23);
            this.numUpDown.TabIndex = 5;
            // 
            // lblBinaryDataRef
            // 
            this.lblBinaryDataRef.AutoSize = true;
            this.lblBinaryDataRef.Location = new System.Drawing.Point(7, 25);
            this.lblBinaryDataRef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBinaryDataRef.Name = "lblBinaryDataRef";
            this.lblBinaryDataRef.Size = new System.Drawing.Size(86, 15);
            this.lblBinaryDataRef.TabIndex = 4;
            this.lblBinaryDataRef.Text = "Binary data ref:";
            // 
            // grpPalette
            // 
            this.grpPalette.Controls.Add(this.ColorSelector);
            this.grpPalette.Controls.Add(this.ColorEditor);
            this.grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPalette.Location = new System.Drawing.Point(0, 81);
            this.grpPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPalette.Name = "grpPalette";
            this.grpPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPalette.Size = new System.Drawing.Size(747, 473);
            this.grpPalette.TabIndex = 16;
            this.grpPalette.TabStop = false;
            this.grpPalette.Text = "Palette";
            // 
            // ColorSelector
            // 
            this.ColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColorSelector.Location = new System.Drawing.Point(4, 134);
            this.ColorSelector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(739, 336);
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
            // PaletteFromBinaryCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpPalette);
            this.Controls.Add(this.grpSourceData);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "PaletteFromBinaryCtrl";
            this.Size = new System.Drawing.Size(747, 554);
            this.grpSourceData.ResumeLayout(false);
            this.grpSourceData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).EndInit();
            this.grpPalette.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbxBinaryDataRef;
        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.GroupBox grpSourceData;
        private System.Windows.Forms.Label lblBinaryDataRef;
        private System.Windows.Forms.GroupBox grpPalette;
        private ColorSelectorCtrl ColorSelector;
        private ColorEditorCtrl ColorEditor;
        private System.Windows.Forms.Label lblBinaryDataStart;
        private System.Windows.Forms.NumericUpDown numUpDown;
    }
}
