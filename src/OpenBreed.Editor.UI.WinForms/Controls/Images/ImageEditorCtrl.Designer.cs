namespace OpenBreed.Editor.UI.WinForms.Controls.Images
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
            this.Split = new System.Windows.Forms.SplitContainer();
            this.ImageView = new OpenBreed.Editor.UI.WinForms.Controls.Images.ImageViewCtrl();
            this.AssetRef = new OpenBreed.Editor.UI.WinForms.Controls.Common.DbEntryAssetRefCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
            this.Split.Panel1.SuspendLayout();
            this.Split.Panel2.SuspendLayout();
            this.Split.SuspendLayout();
            this.SuspendLayout();
            // 
            // Split
            // 
            this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split.Location = new System.Drawing.Point(0, 0);
            this.Split.Name = "Split";
            this.Split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split.Panel1
            // 
            this.Split.Panel1.Controls.Add(this.AssetRef);
            // 
            // Split.Panel2
            // 
            this.Split.Panel2.Controls.Add(this.ImageView);
            this.Split.Size = new System.Drawing.Size(415, 294);
            this.Split.SplitterDistance = 121;
            this.Split.TabIndex = 0;
            // 
            // ImageView
            // 
            this.ImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageView.Location = new System.Drawing.Point(0, 0);
            this.ImageView.Name = "ImageView";
            this.ImageView.Size = new System.Drawing.Size(415, 169);
            this.ImageView.TabIndex = 0;
            // 
            // AssetRef
            // 
            this.AssetRef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssetRef.Location = new System.Drawing.Point(0, 0);
            this.AssetRef.Name = "AssetRef";
            this.AssetRef.Size = new System.Drawing.Size(415, 121);
            this.AssetRef.TabIndex = 0;
            // 
            // ImageEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Split);
            this.Name = "ImageEditorCtrl";
            this.Size = new System.Drawing.Size(415, 294);
            this.Split.Panel1.ResumeLayout(false);
            this.Split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
            this.Split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer Split;
        private ImageViewCtrl ImageView;
        private Common.DbEntryAssetRefCtrl AssetRef;
    }
}
