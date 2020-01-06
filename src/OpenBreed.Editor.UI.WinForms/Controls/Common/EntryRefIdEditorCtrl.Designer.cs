namespace OpenBreed.Editor.UI.WinForms.Controls.Common
{
    partial class EntryRefIdEditorCtrl
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
            this.btnEntryIdSelect = new System.Windows.Forms.Button();
            this.tbxEntryId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnEntryIdSelect
            // 
            this.btnEntryIdSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEntryIdSelect.Location = new System.Drawing.Point(357, 0);
            this.btnEntryIdSelect.Margin = new System.Windows.Forms.Padding(0);
            this.btnEntryIdSelect.Name = "btnEntryIdSelect";
            this.btnEntryIdSelect.Size = new System.Drawing.Size(53, 20);
            this.btnEntryIdSelect.TabIndex = 2;
            this.btnEntryIdSelect.Text = "Select...";
            this.btnEntryIdSelect.UseVisualStyleBackColor = true;
            // 
            // tbxEntryId
            // 
            this.tbxEntryId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxEntryId.Location = new System.Drawing.Point(0, 0);
            this.tbxEntryId.Margin = new System.Windows.Forms.Padding(0);
            this.tbxEntryId.Name = "tbxEntryId";
            this.tbxEntryId.ReadOnly = true;
            this.tbxEntryId.Size = new System.Drawing.Size(350, 20);
            this.tbxEntryId.TabIndex = 3;
            // 
            // EntryRefIdEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxEntryId);
            this.Controls.Add(this.btnEntryIdSelect);
            this.Name = "EntryRefIdEditorCtrl";
            this.Size = new System.Drawing.Size(410, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEntryIdSelect;
        private System.Windows.Forms.TextBox tbxEntryId;
    }
}
