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
            this.DataSource = new OpenBreed.Editor.UI.WinForms.Controls.Images.ImageDataSourceCtrl();
            this.ImageView = new OpenBreed.Editor.UI.WinForms.Controls.Images.ImageViewCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
            this.Split.Panel1.SuspendLayout();
            this.Split.Panel2.SuspendLayout();
            this.Split.SuspendLayout();
            this.SuspendLayout();
            // 
            // Split
            // 
            this.Split.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Split.Location = new System.Drawing.Point(0, 0);
            this.Split.Name = "Split";
            // 
            // Split.Panel1
            // 
            this.Split.Panel1.Controls.Add(this.ImageView);
            // 
            // Split.Panel2
            // 
            this.Split.Panel2.Controls.Add(this.DataSource);
            this.Split.Panel2MinSize = 350;
            this.Split.Size = new System.Drawing.Size(783, 382);
            this.Split.SplitterDistance = 429;
            this.Split.TabIndex = 0;
            // 
            // DataSource
            // 
            this.DataSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSource.Location = new System.Drawing.Point(0, 0);
            this.DataSource.Name = "DataSource";
            this.DataSource.Size = new System.Drawing.Size(346, 378);
            this.DataSource.TabIndex = 0;
            // 
            // ImageView
            // 
            this.ImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageView.Location = new System.Drawing.Point(0, 0);
            this.ImageView.Name = "ImageView";
            this.ImageView.Size = new System.Drawing.Size(425, 378);
            this.ImageView.TabIndex = 0;
            // 
            // ImageEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Split);
            this.Name = "ImageEditorCtrl";
            this.Size = new System.Drawing.Size(783, 382);
            this.Split.Panel1.ResumeLayout(false);
            this.Split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
            this.Split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer Split;
        private ImageViewCtrl ImageView;
        private Images.ImageDataSourceCtrl DataSource;
    }
}
