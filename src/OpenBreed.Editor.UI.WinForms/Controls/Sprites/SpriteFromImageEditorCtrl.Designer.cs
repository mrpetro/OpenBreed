namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    partial class SpriteFromImageEditorCtrl
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
            this.ImageView = new OpenBreed.Editor.UI.WinForms.Controls.Sprites.SpriteEditorViewCtrl();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.Table = new System.Windows.Forms.TableLayoutPanel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.lblSpriteCoords = new System.Windows.Forms.Label();
            this.Table.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageView
            // 
            this.ImageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageView.Location = new System.Drawing.Point(3, 36);
            this.ImageView.Name = "ImageView";
            this.ImageView.Size = new System.Drawing.Size(456, 297);
            this.ImageView.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(3, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Location = new System.Drawing.Point(378, 3);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(75, 23);
            this.btnUndo.TabIndex = 3;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // Table
            // 
            this.Table.ColumnCount = 1;
            this.Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Table.Controls.Add(this.ImageView, 0, 1);
            this.Table.Controls.Add(this.TopPanel, 0, 0);
            this.Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Table.Location = new System.Drawing.Point(0, 0);
            this.Table.Name = "Table";
            this.Table.RowCount = 2;
            this.Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.Table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Table.Size = new System.Drawing.Size(462, 336);
            this.Table.TabIndex = 8;
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.lblSpriteCoords);
            this.TopPanel.Controls.Add(this.btnUndo);
            this.TopPanel.Controls.Add(this.btnUpdate);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopPanel.Location = new System.Drawing.Point(3, 3);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(456, 27);
            this.TopPanel.TabIndex = 1;
            // 
            // lblSpriteCoords
            // 
            this.lblSpriteCoords.AutoSize = true;
            this.lblSpriteCoords.Location = new System.Drawing.Point(84, 8);
            this.lblSpriteCoords.Name = "lblSpriteCoords";
            this.lblSpriteCoords.Size = new System.Drawing.Size(0, 13);
            this.lblSpriteCoords.TabIndex = 4;
            // 
            // SpriteFromImageEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Table);
            this.Name = "SpriteFromImageEditorCtrl";
            this.Size = new System.Drawing.Size(462, 336);
            this.Table.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SpriteEditorViewCtrl ImageView;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.TableLayoutPanel Table;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Label lblSpriteCoords;
    }
}
