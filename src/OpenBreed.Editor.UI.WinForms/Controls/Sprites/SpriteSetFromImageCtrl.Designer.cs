namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteSetFromImageCtrl
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.DGV = new OpenBreed.Common.UI.WinForms.Controls.DataGridViewEx();
            this.btnAddSprite = new System.Windows.Forms.Button();
            this.btnRemoveSprite = new System.Windows.Forms.Button();
            this.Split = new System.Windows.Forms.SplitContainer();
            this.SpriteEditor = new OpenBreed.Editor.UI.WinForms.Controls.Sprites.SpriteFromImageEditorCtrl();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
            this.Split.Panel1.SuspendLayout();
            this.Split.Panel2.SuspendLayout();
            this.Split.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.DGV);
            this.MainPanel.Controls.Add(this.btnAddSprite);
            this.MainPanel.Controls.Add(this.btnRemoveSprite);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(332, 399);
            this.MainPanel.TabIndex = 9;
            // 
            // DGV
            // 
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.CurrentRowIndex = -1;
            this.DGV.Location = new System.Drawing.Point(2, 32);
            this.DGV.Name = "DGV";
            this.DGV.Size = new System.Drawing.Size(325, 363);
            this.DGV.TabIndex = 10;
            // 
            // btnAddSprite
            // 
            this.btnAddSprite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnAddSprite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAddSprite.Location = new System.Drawing.Point(3, 3);
            this.btnAddSprite.Name = "btnAddSprite";
            this.btnAddSprite.Size = new System.Drawing.Size(57, 23);
            this.btnAddSprite.TabIndex = 8;
            this.btnAddSprite.Text = "Add";
            this.btnAddSprite.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSprite
            // 
            this.btnRemoveSprite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSprite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveSprite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemoveSprite.Location = new System.Drawing.Point(270, 3);
            this.btnRemoveSprite.Name = "btnRemoveSprite";
            this.btnRemoveSprite.Size = new System.Drawing.Size(57, 23);
            this.btnRemoveSprite.TabIndex = 9;
            this.btnRemoveSprite.Text = "Remove";
            this.btnRemoveSprite.UseVisualStyleBackColor = true;
            // 
            // Split
            // 
            this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split.Location = new System.Drawing.Point(0, 0);
            this.Split.Name = "Split";
            // 
            // Split.Panel1
            // 
            this.Split.Panel1.Controls.Add(this.MainPanel);
            // 
            // Split.Panel2
            // 
            this.Split.Panel2.Controls.Add(this.SpriteEditor);
            this.Split.Size = new System.Drawing.Size(674, 399);
            this.Split.SplitterDistance = 332;
            this.Split.TabIndex = 0;
            // 
            // SpriteEditor
            // 
            this.SpriteEditor.AutoScroll = true;
            this.SpriteEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpriteEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpriteEditor.Location = new System.Drawing.Point(0, 0);
            this.SpriteEditor.Name = "SpriteEditor";
            this.SpriteEditor.Size = new System.Drawing.Size(338, 399);
            this.SpriteEditor.TabIndex = 0;
            // 
            // SpriteSetFromImageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Split);
            this.Name = "SpriteSetFromImageCtrl";
            this.Size = new System.Drawing.Size(674, 399);
            this.MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Split.Panel1.ResumeLayout(false);
            this.Split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
            this.Split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.SplitContainer Split;
        private SpriteFromImageEditorCtrl SpriteEditor;
        private System.Windows.Forms.Button btnAddSprite;
        private System.Windows.Forms.Button btnRemoveSprite;
        private OpenBreed.Common.UI.WinForms.Controls.DataGridViewEx DGV;
    }
}
