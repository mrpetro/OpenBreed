namespace OpenBreed.Editor.UI.WinForms.Controls
{
    partial class EntryEditorCtrl
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
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.Split = new System.Windows.Forms.SplitContainer();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.tbxDescription = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
            this.Split.Panel1.SuspendLayout();
            this.Split.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Location = new System.Drawing.Point(113, 3);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(52, 23);
            this.btnCommit.TabIndex = 0;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            // 
            // btnRevert
            // 
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevert.Location = new System.Drawing.Point(171, 3);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(52, 23);
            this.btnRevert.TabIndex = 1;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(294, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Location = new System.Drawing.Point(265, 3);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(23, 23);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            // 
            // Split
            // 
            this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Split.IsSplitterFixed = true;
            this.Split.Location = new System.Drawing.Point(0, 0);
            this.Split.Name = "Split";
            this.Split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split.Panel1
            // 
            this.Split.Panel1.Controls.Add(this.tbxDescription);
            this.Split.Panel1.Controls.Add(this.tbxName);
            this.Split.Panel1.Controls.Add(this.btnCommit);
            this.Split.Panel1.Controls.Add(this.btnPrevious);
            this.Split.Panel1.Controls.Add(this.btnRevert);
            this.Split.Panel1.Controls.Add(this.btnNext);
            this.Split.Size = new System.Drawing.Size(320, 240);
            this.Split.SplitterDistance = 56;
            this.Split.TabIndex = 5;
            // 
            // tbxName
            // 
            this.tbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxName.Location = new System.Drawing.Point(3, 5);
            this.tbxName.Name = "tbxName";
            this.tbxName.ReadOnly = true;
            this.tbxName.Size = new System.Drawing.Size(104, 20);
            this.tbxName.TabIndex = 4;
            // 
            // tbxDescription
            // 
            this.tbxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDescription.Location = new System.Drawing.Point(3, 31);
            this.tbxDescription.Name = "tbxDescription";
            this.tbxDescription.Size = new System.Drawing.Size(314, 20);
            this.tbxDescription.TabIndex = 5;
            // 
            // EntryEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Split);
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "EntryEditorCtrl";
            this.Size = new System.Drawing.Size(320, 240);
            this.Split.Panel1.ResumeLayout(false);
            this.Split.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
            this.Split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.SplitContainer Split;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.TextBox tbxDescription;
    }
}
