namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteSetFromSprEditorCtrl
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
            this.DGV = new OpenBreed.Common.UI.WinForms.Controls.DataGridViewEx();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblPalettes = new System.Windows.Forms.Label();
            this.cbxPalettes = new OpenBreed.Common.UI.WinForms.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.grpTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.CurrentRowIndex = -1;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(0, 49);
            this.DGV.Name = "DGV";
            this.DGV.Size = new System.Drawing.Size(426, 227);
            this.DGV.TabIndex = 11;
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.btnImport);
            this.grpTools.Controls.Add(this.lblPalettes);
            this.grpTools.Controls.Add(this.cbxPalettes);
            this.grpTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTools.Location = new System.Drawing.Point(0, 0);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(426, 49);
            this.grpTools.TabIndex = 12;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Tools";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(6, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 21);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // lblPalettes
            // 
            this.lblPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPalettes.AutoSize = true;
            this.lblPalettes.Location = new System.Drawing.Point(175, 23);
            this.lblPalettes.Name = "lblPalettes";
            this.lblPalettes.Size = new System.Drawing.Size(90, 13);
            this.lblPalettes.TabIndex = 3;
            this.lblPalettes.Text = "Example palettes:";
            // 
            // cbxPalettes
            // 
            this.cbxPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPalettes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPalettes.Location = new System.Drawing.Point(271, 20);
            this.cbxPalettes.Name = "cbxPalettes";
            this.cbxPalettes.Size = new System.Drawing.Size(149, 21);
            this.cbxPalettes.TabIndex = 2;
            // 
            // SpriteSetFromSprEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.grpTools);
            this.Name = "SpriteSetFromSprEditorCtrl";
            this.Size = new System.Drawing.Size(426, 276);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.grpTools.ResumeLayout(false);
            this.grpTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenBreed.Common.UI.WinForms.Controls.DataGridViewEx DGV;
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblPalettes;
        private OpenBreed.Common.UI.WinForms.Controls.ComboBoxEx cbxPalettes;
    }
}
