namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    partial class MapEditorActionsManCtrl
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
            this.btnActionSetSelect = new System.Windows.Forms.Button();
            this.tbxActionSetId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnActionSetSelect
            // 
            this.btnActionSetSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActionSetSelect.Location = new System.Drawing.Point(354, 7);
            this.btnActionSetSelect.Name = "btnActionSetSelect";
            this.btnActionSetSelect.Size = new System.Drawing.Size(53, 20);
            this.btnActionSetSelect.TabIndex = 2;
            this.btnActionSetSelect.Text = "Select...";
            this.btnActionSetSelect.UseVisualStyleBackColor = true;
            // 
            // tbxActionSetId
            // 
            this.tbxActionSetId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxActionSetId.Location = new System.Drawing.Point(3, 7);
            this.tbxActionSetId.Name = "tbxActionSetId";
            this.tbxActionSetId.ReadOnly = true;
            this.tbxActionSetId.Size = new System.Drawing.Size(345, 20);
            this.tbxActionSetId.TabIndex = 3;
            // 
            // MapEditorActionsManCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxActionSetId);
            this.Controls.Add(this.btnActionSetSelect);
            this.Name = "MapEditorActionsManCtrl";
            this.Size = new System.Drawing.Size(410, 34);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnActionSetSelect;
        private System.Windows.Forms.TextBox tbxActionSetId;
    }
}
