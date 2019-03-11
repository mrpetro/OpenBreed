namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    partial class ColorEditorCtrl
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
            this.sliderR = new System.Windows.Forms.TrackBar();
            this.sliderG = new System.Windows.Forms.TrackBar();
            this.sliderB = new System.Windows.Forms.TrackBar();
            this.dmR = new System.Windows.Forms.DomainUpDown();
            this.dmG = new System.Windows.Forms.DomainUpDown();
            this.dmB = new System.Windows.Forms.DomainUpDown();
            this.lblR = new System.Windows.Forms.Label();
            this.lblG = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sliderR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderB)).BeginInit();
            this.SuspendLayout();
            // 
            // sliderR
            // 
            this.sliderR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderR.AutoSize = false;
            this.sliderR.Location = new System.Drawing.Point(24, 22);
            this.sliderR.Maximum = 255;
            this.sliderR.Name = "sliderR";
            this.sliderR.Size = new System.Drawing.Size(258, 20);
            this.sliderR.TabIndex = 0;
            this.sliderR.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderR.ValueChanged += new System.EventHandler(this.sliderR_ValueChanged);
            // 
            // sliderG
            // 
            this.sliderG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderG.AutoSize = false;
            this.sliderG.Location = new System.Drawing.Point(24, 48);
            this.sliderG.Maximum = 255;
            this.sliderG.Name = "sliderG";
            this.sliderG.Size = new System.Drawing.Size(258, 20);
            this.sliderG.TabIndex = 1;
            this.sliderG.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderG.ValueChanged += new System.EventHandler(this.sliderG_ValueChanged);
            // 
            // sliderB
            // 
            this.sliderB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderB.AutoSize = false;
            this.sliderB.Location = new System.Drawing.Point(23, 74);
            this.sliderB.Maximum = 255;
            this.sliderB.Name = "sliderB";
            this.sliderB.Size = new System.Drawing.Size(259, 20);
            this.sliderB.TabIndex = 2;
            this.sliderB.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderB.ValueChanged += new System.EventHandler(this.sliderB_ValueChanged);
            // 
            // dmR
            // 
            this.dmR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dmR.Location = new System.Drawing.Point(288, 20);
            this.dmR.Name = "dmR";
            this.dmR.Size = new System.Drawing.Size(40, 20);
            this.dmR.TabIndex = 3;
            this.dmR.Text = "0";
            // 
            // dmG
            // 
            this.dmG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dmG.Location = new System.Drawing.Point(288, 46);
            this.dmG.Name = "dmG";
            this.dmG.Size = new System.Drawing.Size(40, 20);
            this.dmG.TabIndex = 4;
            this.dmG.Text = "255";
            // 
            // dmB
            // 
            this.dmB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dmB.Location = new System.Drawing.Point(288, 72);
            this.dmB.Name = "dmB";
            this.dmB.Size = new System.Drawing.Size(40, 20);
            this.dmB.TabIndex = 5;
            this.dmB.Text = "0";
            // 
            // lblR
            // 
            this.lblR.AutoSize = true;
            this.lblR.Location = new System.Drawing.Point(3, 22);
            this.lblR.Name = "lblR";
            this.lblR.Size = new System.Drawing.Size(15, 13);
            this.lblR.TabIndex = 7;
            this.lblR.Text = "R";
            // 
            // lblG
            // 
            this.lblG.AutoSize = true;
            this.lblG.Location = new System.Drawing.Point(3, 48);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(15, 13);
            this.lblG.TabIndex = 8;
            this.lblG.Text = "G";
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Location = new System.Drawing.Point(3, 74);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(14, 13);
            this.lblB.TabIndex = 9;
            this.lblB.Text = "B";
            // 
            // lblColor
            // 
            this.lblColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(3, 0);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(325, 17);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "#color_index#";
            // 
            // ColorEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.lblB);
            this.Controls.Add(this.lblG);
            this.Controls.Add(this.lblR);
            this.Controls.Add(this.dmB);
            this.Controls.Add(this.dmG);
            this.Controls.Add(this.dmR);
            this.Controls.Add(this.sliderB);
            this.Controls.Add(this.sliderG);
            this.Controls.Add(this.sliderR);
            this.Name = "ColorEditorCtrl";
            this.Size = new System.Drawing.Size(331, 100);
            ((System.ComponentModel.ISupportInitialize)(this.sliderR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar sliderR;
        private System.Windows.Forms.TrackBar sliderG;
        private System.Windows.Forms.TrackBar sliderB;
        private System.Windows.Forms.DomainUpDown dmR;
        private System.Windows.Forms.DomainUpDown dmG;
        private System.Windows.Forms.DomainUpDown dmB;
        private System.Windows.Forms.Label lblR;
        private System.Windows.Forms.Label lblG;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblColor;
    }
}
