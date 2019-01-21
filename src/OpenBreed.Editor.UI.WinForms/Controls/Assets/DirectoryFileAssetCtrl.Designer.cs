namespace OpenBreed.Editor.UI.WinForms.Controls.Assets
{
    partial class DirectoryFileAssetCtrl
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
            this.tbxDirectoryFilePath = new System.Windows.Forms.TextBox();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.grpFilePath = new System.Windows.Forms.GroupBox();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.grpFilePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxDirectoryFilePath
            // 
            this.tbxDirectoryFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDirectoryFilePath.Location = new System.Drawing.Point(62, 19);
            this.tbxDirectoryFilePath.Name = "tbxDirectoryFilePath";
            this.tbxDirectoryFilePath.Size = new System.Drawing.Size(281, 20);
            this.tbxDirectoryFilePath.TabIndex = 3;
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileBrowser.Location = new System.Drawing.Point(349, 19);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(26, 20);
            this.btnFileBrowser.TabIndex = 2;
            this.btnFileBrowser.Text = "...";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            // 
            // grpFilePath
            // 
            this.grpFilePath.Controls.Add(this.lblFilePath);
            this.grpFilePath.Controls.Add(this.tbxDirectoryFilePath);
            this.grpFilePath.Controls.Add(this.btnFileBrowser);
            this.grpFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFilePath.Location = new System.Drawing.Point(0, 0);
            this.grpFilePath.Name = "grpFilePath";
            this.grpFilePath.Size = new System.Drawing.Size(381, 46);
            this.grpFilePath.TabIndex = 4;
            this.grpFilePath.TabStop = false;
            this.grpFilePath.Text = "File asset";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(6, 22);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(50, 13);
            this.lblFilePath.TabIndex = 4;
            this.lblFilePath.Text = "File path:";
            // 
            // DirectoryFileAssetCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpFilePath);
            this.Name = "DirectoryFileAssetCtrl";
            this.Size = new System.Drawing.Size(381, 46);
            this.grpFilePath.ResumeLayout(false);
            this.grpFilePath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbxDirectoryFilePath;
        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.GroupBox grpFilePath;
        private System.Windows.Forms.Label lblFilePath;
    }
}
