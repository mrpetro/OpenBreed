namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class MapPalettesView
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
            this.PaletteEditor = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.PaletteEditorCtrl();
            this.Palettes = new OpenBreed.Editor.UI.WinForms.Controls.Palettes.PalettesCtrl();
            this.SuspendLayout();
            // 
            // PaletteEditor
            // 
            this.PaletteEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaletteEditor.Location = new System.Drawing.Point(0, 28);
            this.PaletteEditor.Name = "PaletteEditor";
            this.PaletteEditor.Size = new System.Drawing.Size(748, 343);
            this.PaletteEditor.TabIndex = 1;
            // 
            // Palettes
            // 
            this.Palettes.Dock = System.Windows.Forms.DockStyle.Top;
            this.Palettes.Location = new System.Drawing.Point(0, 0);
            this.Palettes.Name = "Palettes";
            this.Palettes.Size = new System.Drawing.Size(748, 28);
            this.Palettes.TabIndex = 0;
            // 
            // MapPalettesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 371);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.PaletteEditor);
            this.Controls.Add(this.Palettes);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "MapPalettesView";
            this.ShowIcon = false;
            this.Text = "Palettes";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Palettes.PalettesCtrl Palettes;
        private Controls.Palettes.PaletteEditorCtrl PaletteEditor;
    }
}
