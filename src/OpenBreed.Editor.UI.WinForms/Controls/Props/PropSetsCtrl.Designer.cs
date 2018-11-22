namespace OpenBreed.Editor.UI.WinForms.Controls.Props
{
    partial class PropSetsCtrl
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
            this.btnAddPropSet = new System.Windows.Forms.Button();
            this.btnRemovePropSet = new System.Windows.Forms.Button();
            this.cbxPropSets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddPropSet
            // 
            this.btnAddPropSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPropSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPropSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddPropSet.Location = new System.Drawing.Point(740, 4);
            this.btnAddPropSet.Name = "btnAddPropSet";
            this.btnAddPropSet.Size = new System.Drawing.Size(21, 21);
            this.btnAddPropSet.TabIndex = 14;
            this.btnAddPropSet.Text = "+";
            this.btnAddPropSet.UseVisualStyleBackColor = true;
            // 
            // btnRemovePropSet
            // 
            this.btnRemovePropSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePropSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemovePropSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRemovePropSet.Location = new System.Drawing.Point(767, 4);
            this.btnRemovePropSet.Name = "btnRemovePropSet";
            this.btnRemovePropSet.Size = new System.Drawing.Size(21, 21);
            this.btnRemovePropSet.TabIndex = 13;
            this.btnRemovePropSet.Text = "-";
            this.btnRemovePropSet.UseVisualStyleBackColor = true;
            // 
            // cbxPropSets
            // 
            this.cbxPropSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPropSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPropSets.FormattingEnabled = true;
            this.cbxPropSets.Location = new System.Drawing.Point(3, 4);
            this.cbxPropSets.Name = "cbxPropSets";
            this.cbxPropSets.Size = new System.Drawing.Size(731, 21);
            this.cbxPropSets.TabIndex = 12;
            // 
            // PropSetsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddPropSet);
            this.Controls.Add(this.btnRemovePropSet);
            this.Controls.Add(this.cbxPropSets);
            this.Name = "PropSetsCtrl";
            this.Size = new System.Drawing.Size(791, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddPropSet;
        private System.Windows.Forms.Button btnRemovePropSet;
        private System.Windows.Forms.ComboBox cbxPropSets;
    }
}
