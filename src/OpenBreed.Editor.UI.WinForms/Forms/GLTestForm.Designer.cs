namespace OpenBreed.Editor.UI.WinForms.Forms
{
    partial class GLTestForm
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
            this.MapView = new OpenBreed.Editor.UI.WinForms.Controls.Levels.MapViewExCtrl();
            this.tbxCoords = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // MapView
            // 
            this.MapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapView.Location = new System.Drawing.Point(12, 38);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(776, 400);
            this.MapView.TabIndex = 0;
            // 
            // tbxCoords
            // 
            this.tbxCoords.Location = new System.Drawing.Point(12, 12);
            this.tbxCoords.Name = "tbxCoords";
            this.tbxCoords.Size = new System.Drawing.Size(100, 20);
            this.tbxCoords.TabIndex = 1;
            // 
            // GLTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbxCoords);
            this.Controls.Add(this.MapView);
            this.Name = "GLTestForm";
            this.Text = "GLTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Levels.MapViewExCtrl MapView;
        private System.Windows.Forms.TextBox tbxCoords;
    }
}