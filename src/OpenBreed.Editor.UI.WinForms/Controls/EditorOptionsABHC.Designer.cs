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
            this.tbxGameRunFilePath = new System.Windows.Forms.TextBox();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbxGameFolderPath = new System.Windows.Forms.TextBox();
            this.btnSelectGameFolder = new System.Windows.Forms.Button();
            this.btnSelectGameRunFile = new System.Windows.Forms.Button();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tbxGameRunFileArgs = new System.Windows.Forms.TextBox();
            this.lblArgs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbxGameRunFilePath
            // 
            this.tbxGameRunFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxGameRunFilePath.Location = new System.Drawing.Point(122, 32);
            this.tbxGameRunFilePath.Name = "tbxGameRunFilePath";
            this.tbxGameRunFilePath.ReadOnly = true;
            this.tbxGameRunFilePath.Size = new System.Drawing.Size(160, 20);
            this.tbxGameRunFilePath.TabIndex = 7;
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
            // btnSelectGameRunFile
            // 
            this.btnSelectGameRunFile.Location = new System.Drawing.Point(4, 33);
            this.btnSelectGameRunFile.Name = "btnSelectGameRunFile";
            this.btnSelectGameRunFile.Size = new System.Drawing.Size(112, 20);
            this.btnSelectGameRunFile.TabIndex = 8;
            this.btnSelectGameRunFile.Text = "Select game run file";
            this.btnSelectGameRunFile.UseVisualStyleBackColor = true;
            // 
            // tbxGameRunFileArgs
            // 
            this.tbxGameRunFileArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxGameRunFileArgs.Location = new System.Drawing.Point(325, 32);
            this.tbxGameRunFileArgs.Name = "tbxGameRunFileArgs";
            this.tbxGameRunFileArgs.Size = new System.Drawing.Size(130, 20);
            this.tbxGameRunFileArgs.TabIndex = 12;
            // 
            // lblArgs
            // 
            this.lblArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArgs.AutoSize = true;
            this.lblArgs.Location = new System.Drawing.Point(288, 35);
            this.lblArgs.Name = "lblArgs";
            this.lblArgs.Size = new System.Drawing.Size(31, 13);
            this.lblArgs.TabIndex = 11;
            this.lblArgs.Text = "Args:";
            // 
            // EditorOptionsABHC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxGameRunFilePath);
            this.Controls.Add(this.tbxGameFolderPath);
            this.Controls.Add(this.btnSelectGameFolder);
            this.Controls.Add(this.btnSelectGameRunFile);
            this.Controls.Add(this.tbxGameRunFileArgs);
            this.Controls.Add(this.lblArgs);
            this.Name = "EditorOptionsABHC";
            this.Size = new System.Drawing.Size(457, 220);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxGameRunFilePath;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.TextBox tbxGameFolderPath;
        private System.Windows.Forms.Button btnSelectGameFolder;
        private System.Windows.Forms.Button btnSelectGameRunFile;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.TextBox tbxGameRunFileArgs;
        private System.Windows.Forms.Label lblArgs;
    }
}
