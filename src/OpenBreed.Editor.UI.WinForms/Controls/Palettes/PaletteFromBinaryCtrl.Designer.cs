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
            tbxBinaryDataRef = new System.Windows.Forms.TextBox();
            btnDataRefBrowser = new System.Windows.Forms.Button();
            grpSourceData = new System.Windows.Forms.GroupBox();
            lblBinaryDataStart = new System.Windows.Forms.Label();
            numUpDown = new System.Windows.Forms.NumericUpDown();
            lblBinaryDataRef = new System.Windows.Forms.Label();
            grpPalette = new System.Windows.Forms.GroupBox();
            ColorSelector = new ColorSelectorCtrl();
            grpSourceData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUpDown).BeginInit();
            grpPalette.SuspendLayout();
            SuspendLayout();
            // 
            // tbxBinaryDataRef
            // 
            tbxBinaryDataRef.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbxBinaryDataRef.Location = new System.Drawing.Point(105, 22);
            tbxBinaryDataRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbxBinaryDataRef.Name = "tbxBinaryDataRef";
            tbxBinaryDataRef.Size = new System.Drawing.Size(597, 23);
            tbxBinaryDataRef.TabIndex = 3;
            // 
            // btnDataRefBrowser
            // 
            btnDataRefBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDataRefBrowser.Location = new System.Drawing.Point(709, 22);
            btnDataRefBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDataRefBrowser.Name = "btnDataRefBrowser";
            btnDataRefBrowser.Size = new System.Drawing.Size(30, 23);
            btnDataRefBrowser.TabIndex = 2;
            btnDataRefBrowser.Text = "...";
            btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // grpSourceData
            // 
            grpSourceData.Controls.Add(lblBinaryDataStart);
            grpSourceData.Controls.Add(numUpDown);
            grpSourceData.Controls.Add(lblBinaryDataRef);
            grpSourceData.Controls.Add(tbxBinaryDataRef);
            grpSourceData.Controls.Add(btnDataRefBrowser);
            grpSourceData.Dock = System.Windows.Forms.DockStyle.Top;
            grpSourceData.Location = new System.Drawing.Point(0, 0);
            grpSourceData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpSourceData.Name = "grpSourceData";
            grpSourceData.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpSourceData.Size = new System.Drawing.Size(747, 81);
            grpSourceData.TabIndex = 4;
            grpSourceData.TabStop = false;
            grpSourceData.Text = "Source data";
            // 
            // lblBinaryDataStart
            // 
            lblBinaryDataStart.AutoSize = true;
            lblBinaryDataStart.Location = new System.Drawing.Point(7, 53);
            lblBinaryDataStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBinaryDataStart.Name = "lblBinaryDataStart";
            lblBinaryDataStart.Size = new System.Drawing.Size(60, 15);
            lblBinaryDataStart.TabIndex = 6;
            lblBinaryDataStart.Text = "Data start:";
            // 
            // numUpDown
            // 
            numUpDown.Location = new System.Drawing.Point(105, 51);
            numUpDown.Maximum = new decimal(new int[] { 276447232, 23283, 0, 0 });
            numUpDown.Name = "numUpDown";
            numUpDown.Size = new System.Drawing.Size(88, 23);
            numUpDown.TabIndex = 5;
            // 
            // lblBinaryDataRef
            // 
            lblBinaryDataRef.AutoSize = true;
            lblBinaryDataRef.Location = new System.Drawing.Point(7, 25);
            lblBinaryDataRef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBinaryDataRef.Name = "lblBinaryDataRef";
            lblBinaryDataRef.Size = new System.Drawing.Size(86, 15);
            lblBinaryDataRef.TabIndex = 4;
            lblBinaryDataRef.Text = "Binary data ref:";
            // 
            // grpPalette
            // 
            grpPalette.Controls.Add(ColorSelector);
            grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            grpPalette.Location = new System.Drawing.Point(0, 81);
            grpPalette.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Name = "grpPalette";
            grpPalette.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpPalette.Size = new System.Drawing.Size(747, 473);
            grpPalette.TabIndex = 16;
            grpPalette.TabStop = false;
            grpPalette.Text = "Palette";
            // 
            // ColorSelector
            // 
            ColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            ColorSelector.Location = new System.Drawing.Point(4, 134);
            ColorSelector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ColorSelector.Name = "ColorSelector";
            ColorSelector.Size = new System.Drawing.Size(739, 336);
            ColorSelector.TabIndex = 13;
            // 
            // PaletteFromBinaryCtrl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpPalette);
            Controls.Add(grpSourceData);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "PaletteFromBinaryCtrl";
            Size = new System.Drawing.Size(747, 554);
            grpSourceData.ResumeLayout(false);
            grpSourceData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numUpDown).EndInit();
            grpPalette.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox tbxBinaryDataRef;
        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.GroupBox grpSourceData;
        private System.Windows.Forms.Label lblBinaryDataRef;
        private System.Windows.Forms.GroupBox grpPalette;
        private ColorSelectorCtrl ColorSelector;
        private System.Windows.Forms.Label lblBinaryDataStart;
        private System.Windows.Forms.NumericUpDown numUpDown;
    }
}
