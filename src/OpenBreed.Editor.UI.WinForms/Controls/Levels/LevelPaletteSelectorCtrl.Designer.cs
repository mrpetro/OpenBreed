namespace OpenBreed.Editor.UI.WinForms.Controls.Levels
{
    partial class LevelPaletteSelectorCtrl
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
            this.btnAddPalette = new System.Windows.Forms.Button();
            this.btnRemovePalette = new System.Windows.Forms.Button();
            this.cbxPalettes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddPalette
            // 
            this.btnAddPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPalette.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPalette.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddPalette.Location = new System.Drawing.Point(740, 3);
            this.btnAddPalette.Name = "btnAddPalette";
            this.btnAddPalette.Size = new System.Drawing.Size(21, 21);
            this.btnAddPalette.TabIndex = 8;
            this.btnAddPalette.Text = "+";
            this.btnAddPalette.UseVisualStyleBackColor = true;
            // 
            // btnRemovePalette
            // 
            this.btnRemovePalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePalette.Font = new System.Drawing.Font("Wide Latin", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemovePalette.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRemovePalette.Location = new System.Drawing.Point(767, 3);
            this.btnRemovePalette.Name = "btnRemovePalette";
            this.btnRemovePalette.Size = new System.Drawing.Size(21, 21);
            this.btnRemovePalette.TabIndex = 7;
            this.btnRemovePalette.Text = "-";
            this.btnRemovePalette.UseVisualStyleBackColor = true;
            // 
            // cbxPalettes
            // 
            this.cbxPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPalettes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPalettes.FormattingEnabled = true;
            this.cbxPalettes.Location = new System.Drawing.Point(3, 3);
            this.cbxPalettes.Name = "cbxPalettes";
            this.cbxPalettes.Size = new System.Drawing.Size(731, 21);
            this.cbxPalettes.TabIndex = 6;
            // 
            // PalettesCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddPalette);
            this.Controls.Add(this.btnRemovePalette);
            this.Controls.Add(this.cbxPalettes);
            this.Name = "PalettesCtrl";
            this.Size = new System.Drawing.Size(791, 28);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddPalette;
        private System.Windows.Forms.Button btnRemovePalette;
        private System.Windows.Forms.ComboBox cbxPalettes;
    }
}
