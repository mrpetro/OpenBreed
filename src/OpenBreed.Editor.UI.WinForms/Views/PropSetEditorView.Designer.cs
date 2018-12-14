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
            this.PropSetEditor = new OpenBreed.Editor.UI.WinForms.Controls.Props.PropSetEditorCtrl();
            this.SuspendLayout();
            // 
            // PropSetEditor
            // 
            this.PropSetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropSetEditor.Location = new System.Drawing.Point(0, 0);
            this.PropSetEditor.Name = "PropSetEditor";
            this.PropSetEditor.Size = new System.Drawing.Size(802, 410);
            this.PropSetEditor.TabIndex = 1;
            // 
            // PropSetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 410);
            this.Controls.Add(this.PropSetEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "PropSetView";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.Props.PropSetEditorCtrl PropSetEditor;
    }
}