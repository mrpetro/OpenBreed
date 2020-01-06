﻿namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    partial class ImageEditorCtrl
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
            this.ImageView = new OpenBreed.Editor.UI.WinForms.Controls.Images.ImageViewCtrl();
            this.grpImageAssetRefIdEditor = new System.Windows.Forms.GroupBox();
            this.ImageAssetRefIdEditor = new OpenBreed.Editor.UI.WinForms.Controls.Common.EntryRefIdEditorCtrl();
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpImageAssetRefIdEditor.SuspendLayout();
            this.TablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageView
            // 
            this.ImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageView.Location = new System.Drawing.Point(3, 53);
            this.ImageView.Name = "ImageView";
            this.ImageView.Size = new System.Drawing.Size(777, 326);
            this.ImageView.TabIndex = 0;
            // 
            // grpImageAssetRefIdEditor
            // 
            this.grpImageAssetRefIdEditor.Controls.Add(this.ImageAssetRefIdEditor);
            this.grpImageAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpImageAssetRefIdEditor.Location = new System.Drawing.Point(3, 3);
            this.grpImageAssetRefIdEditor.Name = "grpImageAssetRefIdEditor";
            this.grpImageAssetRefIdEditor.Size = new System.Drawing.Size(777, 44);
            this.grpImageAssetRefIdEditor.TabIndex = 2;
            this.grpImageAssetRefIdEditor.TabStop = false;
            this.grpImageAssetRefIdEditor.Text = "Asset image reference ID";
            // 
            // ImageAssetRefIdEditor
            // 
            this.ImageAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageAssetRefIdEditor.Location = new System.Drawing.Point(3, 16);
            this.ImageAssetRefIdEditor.Name = "ImageAssetRefIdEditor";
            this.ImageAssetRefIdEditor.Size = new System.Drawing.Size(771, 25);
            this.ImageAssetRefIdEditor.TabIndex = 0;
            // 
            // TablePanel
            // 
            this.TablePanel.ColumnCount = 1;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TablePanel.Controls.Add(this.ImageView, 0, 1);
            this.TablePanel.Controls.Add(this.grpImageAssetRefIdEditor, 0, 0);
            this.TablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TablePanel.Location = new System.Drawing.Point(0, 0);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 2;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.08901F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.911F));
            this.TablePanel.Size = new System.Drawing.Size(783, 382);
            this.TablePanel.TabIndex = 1;
            // 
            // ImageEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TablePanel);
            this.Name = "ImageEditorCtrl";
            this.Size = new System.Drawing.Size(783, 382);
            this.grpImageAssetRefIdEditor.ResumeLayout(false);
            this.TablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ImageViewCtrl ImageView;
        private System.Windows.Forms.GroupBox grpImageAssetRefIdEditor;
        private Common.EntryRefIdEditorCtrl ImageAssetRefIdEditor;
        private System.Windows.Forms.TableLayoutPanel TablePanel;
    }
}
