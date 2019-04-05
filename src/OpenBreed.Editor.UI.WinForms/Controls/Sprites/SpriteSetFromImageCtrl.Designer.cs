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
            this.lblSpriteNo = new System.Windows.Forms.Label();
            this.numSpriteNo = new System.Windows.Forms.NumericUpDown();
            this.pnlSprite = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numSpriteNo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSpriteNo
            // 
            this.lblSpriteNo.AutoSize = true;
            this.lblSpriteNo.Location = new System.Drawing.Point(3, 5);
            this.lblSpriteNo.Name = "lblSpriteNo";
            this.lblSpriteNo.Size = new System.Drawing.Size(52, 13);
            this.lblSpriteNo.TabIndex = 7;
            this.lblSpriteNo.Text = "Sprite no:";
            // 
            // numSpriteNo
            // 
            this.numSpriteNo.Location = new System.Drawing.Point(61, 3);
            this.numSpriteNo.Name = "numSpriteNo";
            this.numSpriteNo.Size = new System.Drawing.Size(53, 20);
            this.numSpriteNo.TabIndex = 6;
            // 
            // pnlSprite
            // 
            this.pnlSprite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSprite.Location = new System.Drawing.Point(6, 29);
            this.pnlSprite.Name = "pnlSprite";
            this.pnlSprite.Size = new System.Drawing.Size(417, 244);
            this.pnlSprite.TabIndex = 5;
            // 
            // SpriteSetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSpriteNo);
            this.Controls.Add(this.numSpriteNo);
            this.Controls.Add(this.pnlSprite);
            this.Name = "SpriteSetView";
            this.Size = new System.Drawing.Size(426, 276);
            ((System.ComponentModel.ISupportInitialize)(this.numSpriteNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSpriteNo;
        private System.Windows.Forms.NumericUpDown numSpriteNo;
        private System.Windows.Forms.Panel pnlSprite;
    }
}
