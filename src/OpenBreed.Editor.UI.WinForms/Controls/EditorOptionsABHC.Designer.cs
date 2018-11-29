namespace OpenBreed.Editor.UI.WinForms.Controls
{
    partial class EditorOptionsABHC
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
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbxGameFolderPath = new System.Windows.Forms.TextBox();
            this.btnSelectGameFolder = new System.Windows.Forms.Button();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // tbxGameFolderPath
            // 
            this.tbxGameFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxGameFolderPath.Location = new System.Drawing.Point(122, 6);
            this.tbxGameFolderPath.Name = "tbxGameFolderPath";
            this.tbxGameFolderPath.ReadOnly = true;
            this.tbxGameFolderPath.Size = new System.Drawing.Size(333, 20);
            this.tbxGameFolderPath.TabIndex = 10;
            // 
            // btnSelectGameFolder
            // 
            this.btnSelectGameFolder.Location = new System.Drawing.Point(4, 4);
            this.btnSelectGameFolder.Name = "btnSelectGameFolder";
            this.btnSelectGameFolder.Size = new System.Drawing.Size(112, 23);
            this.btnSelectGameFolder.TabIndex = 9;
            this.btnSelectGameFolder.Text = "Select game folder";
            this.btnSelectGameFolder.UseVisualStyleBackColor = true;
            // 
            // EditorOptionsABHC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxGameFolderPath);
            this.Controls.Add(this.btnSelectGameFolder);
            this.Name = "EditorOptionsABHC";
            this.Size = new System.Drawing.Size(457, 220);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.TextBox tbxGameFolderPath;
        private System.Windows.Forms.Button btnSelectGameFolder;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
    }
}
