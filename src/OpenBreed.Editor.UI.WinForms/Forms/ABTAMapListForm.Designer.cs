namespace OpenBreed.Editor.UI.WinForms.Forms
{
    partial class ABTAMapListForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpResources = new System.Windows.Forms.GroupBox();
            this.lblSpriteSets = new System.Windows.Forms.Label();
            this.lblTileSet = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.cbxSpriteSets = new System.Windows.Forms.ComboBox();
            this.tbxTileSet = new System.Windows.Forms.TextBox();
            this.tbxMap = new System.Windows.Forms.TextBox();
            this.grpLevels = new System.Windows.Forms.GroupBox();
            this.lstLevels = new System.Windows.Forms.ListBox();
            this.tbxDescription = new System.Windows.Forms.TextBox();
            this.btnLevelDelete = new System.Windows.Forms.Button();
            this.grpDescription = new System.Windows.Forms.GroupBox();
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.btnLevelNew = new System.Windows.Forms.Button();
            this.btnLevelEdit = new System.Windows.Forms.Button();
            this.grpSelectedLevel = new System.Windows.Forms.GroupBox();
            this.grpName = new System.Windows.Forms.GroupBox();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.grpResources.SuspendLayout();
            this.grpLevels.SuspendLayout();
            this.grpDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel1.SuspendLayout();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            this.grpSelectedLevel.SuspendLayout();
            this.grpName.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpResources
            // 
            this.grpResources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpResources.Controls.Add(this.lblSpriteSets);
            this.grpResources.Controls.Add(this.lblTileSet);
            this.grpResources.Controls.Add(this.lblMap);
            this.grpResources.Controls.Add(this.cbxSpriteSets);
            this.grpResources.Controls.Add(this.tbxTileSet);
            this.grpResources.Controls.Add(this.tbxMap);
            this.grpResources.Location = new System.Drawing.Point(3, 411);
            this.grpResources.Name = "grpResources";
            this.grpResources.Size = new System.Drawing.Size(497, 101);
            this.grpResources.TabIndex = 13;
            this.grpResources.TabStop = false;
            this.grpResources.Text = "Resources";
            // 
            // lblSpriteSets
            // 
            this.lblSpriteSets.AutoSize = true;
            this.lblSpriteSets.Location = new System.Drawing.Point(6, 76);
            this.lblSpriteSets.Name = "lblSpriteSets";
            this.lblSpriteSets.Size = new System.Drawing.Size(59, 13);
            this.lblSpriteSets.TabIndex = 15;
            this.lblSpriteSets.Text = "Sprite sets:";
            // 
            // lblTileSet
            // 
            this.lblTileSet.AutoSize = true;
            this.lblTileSet.Location = new System.Drawing.Point(6, 50);
            this.lblTileSet.Name = "lblTileSet";
            this.lblTileSet.Size = new System.Drawing.Size(44, 13);
            this.lblTileSet.TabIndex = 14;
            this.lblTileSet.Text = "Tile set:";
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Location = new System.Drawing.Point(6, 24);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(31, 13);
            this.lblMap.TabIndex = 13;
            this.lblMap.Text = "Map:";
            // 
            // cbxSpriteSets
            // 
            this.cbxSpriteSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSpriteSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSpriteSets.FormattingEnabled = true;
            this.cbxSpriteSets.Location = new System.Drawing.Point(71, 73);
            this.cbxSpriteSets.Name = "cbxSpriteSets";
            this.cbxSpriteSets.Size = new System.Drawing.Size(420, 21);
            this.cbxSpriteSets.TabIndex = 12;
            // 
            // tbxTileSet
            // 
            this.tbxTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxTileSet.Location = new System.Drawing.Point(71, 47);
            this.tbxTileSet.Name = "tbxTileSet";
            this.tbxTileSet.ReadOnly = true;
            this.tbxTileSet.Size = new System.Drawing.Size(420, 20);
            this.tbxTileSet.TabIndex = 9;
            // 
            // tbxMap
            // 
            this.tbxMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMap.Location = new System.Drawing.Point(71, 21);
            this.tbxMap.Name = "tbxMap";
            this.tbxMap.ReadOnly = true;
            this.tbxMap.Size = new System.Drawing.Size(420, 20);
            this.tbxMap.TabIndex = 6;
            // 
            // grpLevels
            // 
            this.grpLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLevels.Controls.Add(this.lstLevels);
            this.grpLevels.Location = new System.Drawing.Point(0, 0);
            this.grpLevels.Name = "grpLevels";
            this.grpLevels.Size = new System.Drawing.Size(456, 483);
            this.grpLevels.TabIndex = 1;
            this.grpLevels.TabStop = false;
            this.grpLevels.Text = "Levels";
            // 
            // lstLevels
            // 
            this.lstLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLevels.FormattingEnabled = true;
            this.lstLevels.Location = new System.Drawing.Point(3, 16);
            this.lstLevels.Name = "lstLevels";
            this.lstLevels.Size = new System.Drawing.Size(450, 464);
            this.lstLevels.TabIndex = 0;
            this.lstLevels.SelectedIndexChanged += new System.EventHandler(this.lstLevels_SelectedIndexChanged);
            // 
            // tbxDescription
            // 
            this.tbxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxDescription.Location = new System.Drawing.Point(3, 16);
            this.tbxDescription.Multiline = true;
            this.tbxDescription.Name = "tbxDescription";
            this.tbxDescription.ReadOnly = true;
            this.tbxDescription.Size = new System.Drawing.Size(491, 318);
            this.tbxDescription.TabIndex = 13;
            // 
            // btnLevelDelete
            // 
            this.btnLevelDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelDelete.Location = new System.Drawing.Point(215, 489);
            this.btnLevelDelete.Name = "btnLevelDelete";
            this.btnLevelDelete.Size = new System.Drawing.Size(80, 23);
            this.btnLevelDelete.TabIndex = 7;
            this.btnLevelDelete.Text = "Delete";
            this.btnLevelDelete.UseVisualStyleBackColor = true;
            // 
            // grpDescription
            // 
            this.grpDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDescription.Controls.Add(this.tbxDescription);
            this.grpDescription.Location = new System.Drawing.Point(3, 68);
            this.grpDescription.Name = "grpDescription";
            this.grpDescription.Size = new System.Drawing.Size(497, 337);
            this.grpDescription.TabIndex = 14;
            this.grpDescription.TabStop = false;
            this.grpDescription.Text = "Description";
            // 
            // MainSplit
            // 
            this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplit.Location = new System.Drawing.Point(0, 0);
            this.MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            this.MainSplit.Panel1.Controls.Add(this.btnLevelDelete);
            this.MainSplit.Panel1.Controls.Add(this.btnLevelNew);
            this.MainSplit.Panel1.Controls.Add(this.btnLevelEdit);
            this.MainSplit.Panel1.Controls.Add(this.grpLevels);
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.Controls.Add(this.grpSelectedLevel);
            this.MainSplit.Size = new System.Drawing.Size(963, 515);
            this.MainSplit.SplitterDistance = 456;
            this.MainSplit.TabIndex = 15;
            // 
            // btnLevelNew
            // 
            this.btnLevelNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelNew.Location = new System.Drawing.Point(111, 489);
            this.btnLevelNew.Name = "btnLevelNew";
            this.btnLevelNew.Size = new System.Drawing.Size(80, 23);
            this.btnLevelNew.TabIndex = 6;
            this.btnLevelNew.Text = "New...";
            this.btnLevelNew.UseVisualStyleBackColor = true;
            // 
            // btnLevelEdit
            // 
            this.btnLevelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelEdit.Location = new System.Drawing.Point(3, 489);
            this.btnLevelEdit.Name = "btnLevelEdit";
            this.btnLevelEdit.Size = new System.Drawing.Size(80, 23);
            this.btnLevelEdit.TabIndex = 5;
            this.btnLevelEdit.Text = "Edit";
            this.btnLevelEdit.UseVisualStyleBackColor = true;
            this.btnLevelEdit.Click += new System.EventHandler(this.btnLevelEdit_Click);
            // 
            // grpSelectedLevel
            // 
            this.grpSelectedLevel.Controls.Add(this.grpName);
            this.grpSelectedLevel.Controls.Add(this.grpDescription);
            this.grpSelectedLevel.Controls.Add(this.grpResources);
            this.grpSelectedLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSelectedLevel.Location = new System.Drawing.Point(0, 0);
            this.grpSelectedLevel.Name = "grpSelectedLevel";
            this.grpSelectedLevel.Size = new System.Drawing.Size(503, 515);
            this.grpSelectedLevel.TabIndex = 13;
            this.grpSelectedLevel.TabStop = false;
            this.grpSelectedLevel.Text = "Level information";
            // 
            // grpName
            // 
            this.grpName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpName.Controls.Add(this.tbxName);
            this.grpName.Location = new System.Drawing.Point(3, 16);
            this.grpName.Name = "grpName";
            this.grpName.Size = new System.Drawing.Size(497, 46);
            this.grpName.TabIndex = 15;
            this.grpName.TabStop = false;
            this.grpName.Text = "Name";
            // 
            // tbxName
            // 
            this.tbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxName.Location = new System.Drawing.Point(3, 19);
            this.tbxName.Name = "tbxName";
            this.tbxName.ReadOnly = true;
            this.tbxName.Size = new System.Drawing.Size(491, 20);
            this.tbxName.TabIndex = 0;
            // 
            // ABTAMapListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 515);
            this.Controls.Add(this.MainSplit);
            this.Name = "ABTAMapListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Alien Breed: Tower Assault map to edit...";
            this.grpResources.ResumeLayout(false);
            this.grpResources.PerformLayout();
            this.grpLevels.ResumeLayout(false);
            this.grpDescription.ResumeLayout(false);
            this.grpDescription.PerformLayout();
            this.MainSplit.Panel1.ResumeLayout(false);
            this.MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.grpSelectedLevel.ResumeLayout(false);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpResources;
        private System.Windows.Forms.Label lblSpriteSets;
        private System.Windows.Forms.Label lblTileSet;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.ComboBox cbxSpriteSets;
        private System.Windows.Forms.TextBox tbxTileSet;
        private System.Windows.Forms.TextBox tbxMap;
        private System.Windows.Forms.GroupBox grpLevels;
        private System.Windows.Forms.ListBox lstLevels;
        private System.Windows.Forms.TextBox tbxDescription;
        private System.Windows.Forms.Button btnLevelDelete;
        private System.Windows.Forms.GroupBox grpDescription;
        private System.Windows.Forms.SplitContainer MainSplit;
        private System.Windows.Forms.Button btnLevelNew;
        private System.Windows.Forms.Button btnLevelEdit;
        private System.Windows.Forms.GroupBox grpSelectedLevel;
        private System.Windows.Forms.GroupBox grpName;
        private System.Windows.Forms.TextBox tbxName;
    }
}