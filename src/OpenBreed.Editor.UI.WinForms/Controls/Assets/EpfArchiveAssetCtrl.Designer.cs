namespace OpenBreed.Editor.UI.WinForms.Controls.Assets
{
    partial class EpfArchiveAssetCtrl
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
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.tbxEpfArchivePath = new System.Windows.Forms.TextBox();
            this.cbxEntryName = new System.Windows.Forms.ComboBox();
            this.grpEpfArchiveAsset = new System.Windows.Forms.GroupBox();
            this.lblEpfArchiveFilePath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpEpfArchiveAsset.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileBrowser.Location = new System.Drawing.Point(383, 19);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(26, 20);
            this.btnFileBrowser.TabIndex = 0;
            this.btnFileBrowser.Text = "...";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            // 
            // tbxEpfArchivePath
            // 
            this.tbxEpfArchivePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxEpfArchivePath.Location = new System.Drawing.Point(98, 19);
            this.tbxEpfArchivePath.Name = "tbxEpfArchivePath";
            this.tbxEpfArchivePath.Size = new System.Drawing.Size(279, 20);
            this.tbxEpfArchivePath.TabIndex = 1;
            // 
            // cbxEntryName
            // 
            this.cbxEntryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxEntryName.FormattingEnabled = true;
            this.cbxEntryName.Location = new System.Drawing.Point(98, 53);
            this.cbxEntryName.Name = "cbxEntryName";
            this.cbxEntryName.Size = new System.Drawing.Size(279, 21);
            this.cbxEntryName.TabIndex = 2;
            // 
            // grpEpfArchiveAsset
            // 
            this.grpEpfArchiveAsset.Controls.Add(this.label1);
            this.grpEpfArchiveAsset.Controls.Add(this.lblEpfArchiveFilePath);
            this.grpEpfArchiveAsset.Controls.Add(this.cbxEntryName);
            this.grpEpfArchiveAsset.Controls.Add(this.btnFileBrowser);
            this.grpEpfArchiveAsset.Controls.Add(this.tbxEpfArchivePath);
            this.grpEpfArchiveAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEpfArchiveAsset.Location = new System.Drawing.Point(0, 0);
            this.grpEpfArchiveAsset.Name = "grpEpfArchiveAsset";
            this.grpEpfArchiveAsset.Size = new System.Drawing.Size(415, 81);
            this.grpEpfArchiveAsset.TabIndex = 3;
            this.grpEpfArchiveAsset.TabStop = false;
            this.grpEpfArchiveAsset.Text = "EPF Archive entry asset";
            // 
            // lblEpfArchiveFilePath
            // 
            this.lblEpfArchiveFilePath.AutoSize = true;
            this.lblEpfArchiveFilePath.Location = new System.Drawing.Point(6, 23);
            this.lblEpfArchiveFilePath.Name = "lblEpfArchiveFilePath";
            this.lblEpfArchiveFilePath.Size = new System.Drawing.Size(86, 13);
            this.lblEpfArchiveFilePath.TabIndex = 3;
            this.lblEpfArchiveFilePath.Text = "Archive file path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Entry name:";
            // 
            // EpfArchiveAssetCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpEpfArchiveAsset);
            this.Name = "EpfArchiveAssetCtrl";
            this.Size = new System.Drawing.Size(415, 81);
            this.grpEpfArchiveAsset.ResumeLayout(false);
            this.grpEpfArchiveAsset.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.TextBox tbxEpfArchivePath;
        private System.Windows.Forms.ComboBox cbxEntryName;
        private System.Windows.Forms.GroupBox grpEpfArchiveAsset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEpfArchiveFilePath;
    }
}
