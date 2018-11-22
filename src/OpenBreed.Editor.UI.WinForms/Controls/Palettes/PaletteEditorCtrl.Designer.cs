namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class PaletteEditorCtrl
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
            this.ColorSelector = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorSelectorCtrl();
            this.ColorEditor = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl();
            this.SuspendLayout();
            // 
            // ColorSelector
            // 
            this.ColorSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorSelector.Location = new System.Drawing.Point(3, 109);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(543, 91);
            this.ColorSelector.TabIndex = 12;
            // 
            // ColorEditor
            // 
            this.ColorEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorEditor.Location = new System.Drawing.Point(3, 3);
            this.ColorEditor.Name = "ColorEditor";
            this.ColorEditor.Size = new System.Drawing.Size(543, 100);
            this.ColorEditor.TabIndex = 11;
            // 
            // PaletteCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ColorSelector);
            this.Controls.Add(this.ColorEditor);
            this.Name = "PaletteCtrl";
            this.Size = new System.Drawing.Size(549, 203);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorSelectorCtrl ColorSelector;
        private OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl ColorEditor;
    }
}
