namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class SoundEditorView
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
            this.SoundEditor = new OpenBreed.Editor.UI.WinForms.Controls.Sounds.SoundEditorCtrl();
            this.SuspendLayout();
            // 
            // SoundEditor
            // 
            this.SoundEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SoundEditor.Location = new System.Drawing.Point(0, 0);
            this.SoundEditor.Name = "SoundEditor";
            this.SoundEditor.Size = new System.Drawing.Size(284, 261);
            this.SoundEditor.TabIndex = 0;
            // 
            // ImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SoundEditor);
            this.HideOnClose = true;
            this.Name = "ImageView";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Sounds.SoundEditorCtrl SoundEditor;
    }
}
