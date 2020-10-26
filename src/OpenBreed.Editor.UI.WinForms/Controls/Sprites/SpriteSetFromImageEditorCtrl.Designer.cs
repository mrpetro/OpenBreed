namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteSetFromImageEditorCtrl
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
            this.LayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.grpImageAssetRefIdEditor = new System.Windows.Forms.GroupBox();
            this.ImageAssetRefIdEditor = new OpenBreed.Editor.UI.WinForms.Controls.Common.EntryRefIdEditorCtrl();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblPalettes = new System.Windows.Forms.Label();
            this.cbxPalettes = new System.Windows.Forms.ComboBox();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
            this.Split.Panel1.SuspendLayout();
            this.Split.Panel2.SuspendLayout();
            this.Split.SuspendLayout();
            this.LayoutTable.SuspendLayout();
            this.grpImageAssetRefIdEditor.SuspendLayout();
            this.grpTools.SuspendLayout();
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
            this.MainPanel.Size = new System.Drawing.Size(329, 294);
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
            this.DGV.Size = new System.Drawing.Size(322, 258);
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
            this.btnRemoveSprite.Location = new System.Drawing.Point(267, 3);
            this.btnRemoveSprite.Name = "btnRemoveSprite";
            this.btnRemoveSprite.Size = new System.Drawing.Size(57, 23);
            this.btnRemoveSprite.TabIndex = 9;
            this.btnRemoveSprite.Text = "Remove";
            this.btnRemoveSprite.UseVisualStyleBackColor = true;
            // 
            // Split
            // 
            this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split.Location = new System.Drawing.Point(3, 53);
            this.Split.Name = "Split";
            // 
            // Split.Panel1
            // 
            this.Split.Panel1.Controls.Add(this.MainPanel);
            // 
            // Split.Panel2
            // 
            this.Split.Panel2.Controls.Add(this.SpriteEditor);
            this.Split.Size = new System.Drawing.Size(668, 294);
            this.Split.SplitterDistance = 329;
            this.Split.TabIndex = 0;
            // 
            // SpriteEditor
            // 
            this.SpriteEditor.AutoScroll = true;
            this.SpriteEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpriteEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpriteEditor.Location = new System.Drawing.Point(0, 0);
            this.SpriteEditor.Name = "SpriteEditor";
            this.SpriteEditor.Size = new System.Drawing.Size(335, 294);
            this.SpriteEditor.TabIndex = 0;
            // 
            // LayoutTable
            // 
            this.LayoutTable.ColumnCount = 1;
            this.LayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Controls.Add(this.grpImageAssetRefIdEditor, 0, 0);
            this.LayoutTable.Controls.Add(this.Split, 0, 1);
            this.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutTable.Location = new System.Drawing.Point(0, 49);
            this.LayoutTable.Name = "LayoutTable";
            this.LayoutTable.RowCount = 2;
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Size = new System.Drawing.Size(674, 350);
            this.LayoutTable.TabIndex = 1;
            // 
            // grpImageAssetRefIdEditor
            // 
            this.grpImageAssetRefIdEditor.Controls.Add(this.ImageAssetRefIdEditor);
            this.grpImageAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpImageAssetRefIdEditor.Location = new System.Drawing.Point(3, 3);
            this.grpImageAssetRefIdEditor.Name = "grpImageAssetRefIdEditor";
            this.grpImageAssetRefIdEditor.Size = new System.Drawing.Size(668, 44);
            this.grpImageAssetRefIdEditor.TabIndex = 3;
            this.grpImageAssetRefIdEditor.TabStop = false;
            this.grpImageAssetRefIdEditor.Text = "Asset image reference ID";
            // 
            // ImageAssetRefIdEditor
            // 
            this.ImageAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageAssetRefIdEditor.Location = new System.Drawing.Point(3, 16);
            this.ImageAssetRefIdEditor.Name = "ImageAssetRefIdEditor";
            this.ImageAssetRefIdEditor.Size = new System.Drawing.Size(662, 25);
            this.ImageAssetRefIdEditor.TabIndex = 0;
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.btnImport);
            this.grpTools.Controls.Add(this.lblPalettes);
            this.grpTools.Controls.Add(this.cbxPalettes);
            this.grpTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTools.Location = new System.Drawing.Point(0, 0);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(674, 49);
            this.grpTools.TabIndex = 6;
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
            this.lblPalettes.Location = new System.Drawing.Point(423, 23);
            this.lblPalettes.Name = "lblPalettes";
            this.lblPalettes.Size = new System.Drawing.Size(90, 13);
            this.lblPalettes.TabIndex = 3;
            this.lblPalettes.Text = "Example palettes:";
            // 
            // cbxPalettes
            // 
            this.cbxPalettes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPalettes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPalettes.Location = new System.Drawing.Point(519, 20);
            this.cbxPalettes.Name = "cbxPalettes";
            this.cbxPalettes.Size = new System.Drawing.Size(149, 21);
            this.cbxPalettes.TabIndex = 2;
            // 
            // SpriteSetFromImageEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LayoutTable);
            this.Controls.Add(this.grpTools);
            this.Name = "SpriteSetFromImageEditorCtrl";
            this.Size = new System.Drawing.Size(674, 399);
            this.MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Split.Panel1.ResumeLayout(false);
            this.Split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
            this.Split.ResumeLayout(false);
            this.LayoutTable.ResumeLayout(false);
            this.grpImageAssetRefIdEditor.ResumeLayout(false);
            this.grpTools.ResumeLayout(false);
            this.grpTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.SplitContainer Split;
        private SpriteFromImageEditorCtrl SpriteEditor;
        private System.Windows.Forms.Button btnAddSprite;
        private System.Windows.Forms.Button btnRemoveSprite;
        private OpenBreed.Common.UI.WinForms.Controls.DataGridViewEx DGV;
        private System.Windows.Forms.TableLayoutPanel LayoutTable;
        private System.Windows.Forms.GroupBox grpImageAssetRefIdEditor;
        private Common.EntryRefIdEditorCtrl ImageAssetRefIdEditor;
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblPalettes;
        private System.Windows.Forms.ComboBox cbxPalettes;
    }
}
