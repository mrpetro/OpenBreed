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
            this.ColorSelector.Location = new System.Drawing.Point(4, 126);
            this.ColorSelector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(634, 105);
            this.ColorSelector.TabIndex = 12;
            // 
            // ColorEditor
            // 
            this.ColorEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorEditor.Location = new System.Drawing.Point(4, 3);
            this.ColorEditor.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ColorEditor.Name = "ColorEditor";
            this.ColorEditor.Size = new System.Drawing.Size(634, 138);
            this.ColorEditor.TabIndex = 11;
            // 
            // PaletteEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ColorSelector);
            this.Controls.Add(this.ColorEditor);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "PaletteEditorCtrl";
            this.Size = new System.Drawing.Size(640, 234);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorSelectorCtrl ColorSelector;
        private OpenBreed.Editor.UI.WinForms.Controls.Palettes.ColorEditorCtrl ColorEditor;
    }
}
