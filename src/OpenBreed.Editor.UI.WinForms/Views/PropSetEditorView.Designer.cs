namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class PropSetEditorView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EntryEditor = new OpenBreed.Editor.UI.WinForms.Controls.EntryEditorCtrl();
            this.SuspendLayout();
            // 
            // EntryEditor
            // 
            this.EntryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntryEditor.InnerCtrl = null;
            this.EntryEditor.Location = new System.Drawing.Point(0, 0);
            this.EntryEditor.MinimumSize = new System.Drawing.Size(232, 147);
            this.EntryEditor.Name = "EntryEditor";
            this.EntryEditor.Size = new System.Drawing.Size(802, 410);
            this.EntryEditor.TabIndex = 0;
            // 
            // PropSetEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 410);
            this.Controls.Add(this.EntryEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "PropSetEditorView";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.EntryEditorCtrl EntryEditor;
    }
}