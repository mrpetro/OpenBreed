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
            this.panelR = new System.Windows.Forms.Panel();
            this.panelG = new System.Windows.Forms.Panel();
            this.panelB = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.sliderR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderB)).BeginInit();
            this.panelR.SuspendLayout();
            this.panelG.SuspendLayout();
            this.panelB.SuspendLayout();
            this.SuspendLayout();
            // 
            // sliderR
            // 
            this.sliderR.AutoSize = false;
            this.sliderR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderR.Location = new System.Drawing.Point(14, 0);
            this.sliderR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sliderR.Maximum = 255;
            this.sliderR.Name = "sliderR";
            this.sliderR.Size = new System.Drawing.Size(362, 21);
            this.sliderR.TabIndex = 0;
            this.sliderR.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderR.ValueChanged += new System.EventHandler(this.sliderR_ValueChanged);
            // 
            // sliderG
            // 
            this.sliderG.AutoSize = false;
            this.sliderG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderG.Location = new System.Drawing.Point(15, 0);
            this.sliderG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sliderG.Maximum = 255;
            this.sliderG.Name = "sliderG";
            this.sliderG.Size = new System.Drawing.Size(361, 21);
            this.sliderG.TabIndex = 1;
            this.sliderG.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderG.ValueChanged += new System.EventHandler(this.sliderG_ValueChanged);
            // 
            // sliderB
            // 
            this.sliderB.AutoSize = false;
            this.sliderB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderB.Location = new System.Drawing.Point(14, 0);
            this.sliderB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sliderB.Maximum = 255;
            this.sliderB.Name = "sliderB";
            this.sliderB.Size = new System.Drawing.Size(362, 21);
            this.sliderB.TabIndex = 2;
            this.sliderB.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderB.ValueChanged += new System.EventHandler(this.sliderB_ValueChanged);
            // 
            // dmR
            // 
            this.dmR.Dock = System.Windows.Forms.DockStyle.Right;
            this.dmR.Location = new System.Drawing.Point(376, 0);
            this.dmR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dmR.Name = "dmR";
            this.dmR.Size = new System.Drawing.Size(47, 23);
            this.dmR.TabIndex = 3;
            this.dmR.Text = "0";
            // 
            // dmG
            // 
            this.dmG.Dock = System.Windows.Forms.DockStyle.Right;
            this.dmG.Location = new System.Drawing.Point(376, 0);
            this.dmG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dmG.Name = "dmG";
            this.dmG.Size = new System.Drawing.Size(47, 23);
            this.dmG.TabIndex = 4;
            this.dmG.Text = "255";
            // 
            // dmB
            // 
            this.dmB.Dock = System.Windows.Forms.DockStyle.Right;
            this.dmB.Location = new System.Drawing.Point(376, 0);
            this.dmB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dmB.Name = "dmB";
            this.dmB.Size = new System.Drawing.Size(47, 23);
            this.dmB.TabIndex = 5;
            this.dmB.Text = "0";
            // 
            // lblR
            // 
            this.lblR.AutoSize = true;
            this.lblR.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblR.Location = new System.Drawing.Point(0, 0);
            this.lblR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblR.Name = "lblR";
            this.lblR.Size = new System.Drawing.Size(14, 15);
            this.lblR.TabIndex = 7;
            this.lblR.Text = "R";
            // 
            // lblG
            // 
            this.lblG.AutoSize = true;
            this.lblG.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblG.Location = new System.Drawing.Point(0, 0);
            this.lblG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(15, 15);
            this.lblG.TabIndex = 8;
            this.lblG.Text = "G";
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblB.Location = new System.Drawing.Point(0, 0);
            this.lblB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(14, 15);
            this.lblB.TabIndex = 9;
            this.lblB.Text = "B";
            // 
            // lblColor
            // 
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblColor.Location = new System.Drawing.Point(0, 0);
            this.lblColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(423, 19);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "#color_index#";
            // 
            // panelR
            // 
            this.panelR.Controls.Add(this.sliderR);
            this.panelR.Controls.Add(this.lblR);
            this.panelR.Controls.Add(this.dmR);
            this.panelR.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelR.Location = new System.Drawing.Point(0, 19);
            this.panelR.Name = "panelR";
            this.panelR.Size = new System.Drawing.Size(423, 21);
            this.panelR.TabIndex = 10;
            // 
            // panelG
            // 
            this.panelG.Controls.Add(this.sliderG);
            this.panelG.Controls.Add(this.lblG);
            this.panelG.Controls.Add(this.dmG);
            this.panelG.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelG.Location = new System.Drawing.Point(0, 40);
            this.panelG.Name = "panelG";
            this.panelG.Size = new System.Drawing.Size(423, 21);
            this.panelG.TabIndex = 11;
            // 
            // panelB
            // 
            this.panelB.Controls.Add(this.sliderB);
            this.panelB.Controls.Add(this.lblB);
            this.panelB.Controls.Add(this.dmB);
            this.panelB.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelB.Location = new System.Drawing.Point(0, 61);
            this.panelB.Name = "panelB";
            this.panelB.Size = new System.Drawing.Size(423, 21);
            this.panelB.TabIndex = 12;
            // 
            // ColorEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelB);
            this.Controls.Add(this.panelG);
            this.Controls.Add(this.panelR);
            this.Controls.Add(this.lblColor);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ColorEditorCtrl";
            this.Size = new System.Drawing.Size(423, 97);
            ((System.ComponentModel.ISupportInitialize)(this.sliderR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderB)).EndInit();
            this.panelR.ResumeLayout(false);
            this.panelR.PerformLayout();
            this.panelG.ResumeLayout(false);
            this.panelG.PerformLayout();
            this.panelB.ResumeLayout(false);
            this.panelB.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Panel panelR;
        private System.Windows.Forms.Panel panelG;
        private System.Windows.Forms.Panel panelB;
    }
}
