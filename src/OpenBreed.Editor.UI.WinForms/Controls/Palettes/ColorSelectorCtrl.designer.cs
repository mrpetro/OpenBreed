namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class ColorSelectorCtrl
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
            this.SuspendLayout();
            // 
            // ColorSelectorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ColorSelectorCtrl";
            this.Size = new System.Drawing.Size(463, 277);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorSelectorCtrl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorSelectorCtrl_MouseDown);
            this.Resize += new System.EventHandler(this.PaletteView_Resize);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
