namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class DbTablesEditorView
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
            this.DatabaseViewer = new OpenBreed.Editor.UI.WinForms.Controls.Database.DatabaseViewerCtrl();
            this.SuspendLayout();
            // 
            // DatabaseViewer
            // 
            this.DatabaseViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DatabaseViewer.Location = new System.Drawing.Point(0, 0);
            this.DatabaseViewer.Name = "DatabaseViewer";
            this.DatabaseViewer.Size = new System.Drawing.Size(585, 329);
            this.DatabaseViewer.TabIndex = 0;
            // 
            // DatabaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 329);
            this.Controls.Add(this.DatabaseViewer);
            this.HideOnClose = true;
            this.Name = "DatabaseView";
            this.Text = "Database";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Database.DatabaseViewerCtrl DatabaseViewer;
    }
}
