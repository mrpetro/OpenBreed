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
            this.lblBinaryDataRef = new System.Windows.Forms.Label();
            this.grpPalette = new System.Windows.Forms.GroupBox();
            this.ColorSelector = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorSelectorCtrl();
            this.ColorEditor = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl();
            this.grpSourceData.SuspendLayout();
            this.grpPalette.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxBinaryDataRef
            // 
            this.tbxBinaryDataRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxBinaryDataRef.Location = new System.Drawing.Point(90, 19);
            this.tbxBinaryDataRef.Name = "tbxBinaryDataRef";
            this.tbxBinaryDataRef.Size = new System.Drawing.Size(319, 20);
            this.tbxBinaryDataRef.TabIndex = 3;
            // 
            // btnDataRefBrowser
            // 
            this.btnDataRefBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataRefBrowser.Location = new System.Drawing.Point(415, 19);
            this.btnDataRefBrowser.Name = "btnDataRefBrowser";
            this.btnDataRefBrowser.Size = new System.Drawing.Size(26, 20);
            this.btnDataRefBrowser.TabIndex = 2;
            this.btnDataRefBrowser.Text = "...";
            this.btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // grpSourceData
            // 
            this.grpSourceData.Controls.Add(this.lblBinaryDataRef);
            this.grpSourceData.Controls.Add(this.tbxBinaryDataRef);
            this.grpSourceData.Controls.Add(this.btnDataRefBrowser);
            this.grpSourceData.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSourceData.Location = new System.Drawing.Point(0, 0);
            this.grpSourceData.Name = "grpSourceData";
            this.grpSourceData.Size = new System.Drawing.Size(447, 47);
            this.grpSourceData.TabIndex = 4;
            this.grpSourceData.TabStop = false;
            this.grpSourceData.Text = "Source data";
            // 
            // lblBinaryDataRef
            // 
            this.lblBinaryDataRef.AutoSize = true;
            this.lblBinaryDataRef.Location = new System.Drawing.Point(6, 22);
            this.lblBinaryDataRef.Name = "lblBinaryDataRef";
            this.lblBinaryDataRef.Size = new System.Drawing.Size(78, 13);
            this.lblBinaryDataRef.TabIndex = 4;
            this.lblBinaryDataRef.Text = "Binary data ref:";
            // 
            // grpPalette
            // 
            this.grpPalette.Controls.Add(this.ColorSelector);
            this.grpPalette.Controls.Add(this.ColorEditor);
            this.grpPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPalette.Location = new System.Drawing.Point(0, 47);
            this.grpPalette.Name = "grpPalette";
            this.grpPalette.Size = new System.Drawing.Size(447, 381);
            this.grpPalette.TabIndex = 16;
            this.grpPalette.TabStop = false;
            this.grpPalette.Text = "Palette";
            // 
            // ColorSelector
            // 
            this.ColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColorSelector.Location = new System.Drawing.Point(3, 116);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(441, 262);
            this.ColorSelector.TabIndex = 13;
            // 
            // ColorEditor
            // 
            this.ColorEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.ColorEditor.Location = new System.Drawing.Point(3, 16);
            this.ColorEditor.Name = "ColorEditor";
            this.ColorEditor.Size = new System.Drawing.Size(441, 100);
            this.ColorEditor.TabIndex = 14;
            // 
            // PaletteFromBinaryCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpPalette);
            this.Controls.Add(this.grpSourceData);
            this.Name = "PaletteFromBinaryCtrl";
            this.Size = new System.Drawing.Size(447, 428);
            this.grpSourceData.ResumeLayout(false);
            this.grpSourceData.PerformLayout();
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
    }
}
