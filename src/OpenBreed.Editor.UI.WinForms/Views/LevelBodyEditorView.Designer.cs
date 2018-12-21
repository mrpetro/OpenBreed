namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class LevelBodyEditorView
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
            this.MapBodyViewer = new OpenBreed.Editor.UI.WinForms.Controls.Levels.LevelBodyEditorCtrl();
            this.SuspendLayout();
            // 
            // MapBodyViewer
            // 
            this.MapBodyViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapBodyViewer.Location = new System.Drawing.Point(0, 0);
            this.MapBodyViewer.Name = "MapBodyViewer";
            this.MapBodyViewer.Size = new System.Drawing.Size(284, 261);
            this.MapBodyViewer.TabIndex = 0;
            // 
            // MapBodyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.MapBodyViewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "MapBodyView";
            this.Text = "Map body";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Levels.LevelBodyEditorCtrl MapBodyViewer;
    }
}
