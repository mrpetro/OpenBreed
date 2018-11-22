namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteSetsCtrl
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
            this.btnAddSpriteSet = new System.Windows.Forms.Button();
            this.btnRemoveSpriteSet = new System.Windows.Forms.Button();
            this.cbxSpriteSets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddSpriteSet
            // 
            this.btnAddSpriteSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSpriteSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSpriteSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddSpriteSet.Location = new System.Drawing.Point(740, 4);
            this.btnAddSpriteSet.Name = "btnAddSpriteSet";
            this.btnAddSpriteSet.Size = new System.Drawing.Size(21, 21);
            this.btnAddSpriteSet.TabIndex = 17;
            this.btnAddSpriteSet.Text = "+";
            this.btnAddSpriteSet.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSpriteSet
            // 
            this.btnRemoveSpriteSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSpriteSet.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveSpriteSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRemoveSpriteSet.Location = new System.Drawing.Point(767, 4);
            this.btnRemoveSpriteSet.Name = "btnRemoveSpriteSet";
            this.btnRemoveSpriteSet.Size = new System.Drawing.Size(21, 21);
            this.btnRemoveSpriteSet.TabIndex = 16;
            this.btnRemoveSpriteSet.Text = "-";
            this.btnRemoveSpriteSet.UseVisualStyleBackColor = true;
            // 
            // cbxSpriteSets
            // 
            this.cbxSpriteSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSpriteSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSpriteSets.FormattingEnabled = true;
            this.cbxSpriteSets.Location = new System.Drawing.Point(3, 4);
            this.cbxSpriteSets.Name = "cbxSpriteSets";
            this.cbxSpriteSets.Size = new System.Drawing.Size(731, 21);
            this.cbxSpriteSets.TabIndex = 15;
            // 
            // SpriteSetsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddSpriteSet);
            this.Controls.Add(this.btnRemoveSpriteSet);
            this.Controls.Add(this.cbxSpriteSets);
            this.Name = "SpriteSetsCtrl";
            this.Size = new System.Drawing.Size(791, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddSpriteSet;
        private System.Windows.Forms.Button btnRemoveSpriteSet;
        private System.Windows.Forms.ComboBox cbxSpriteSets;
    }
}
