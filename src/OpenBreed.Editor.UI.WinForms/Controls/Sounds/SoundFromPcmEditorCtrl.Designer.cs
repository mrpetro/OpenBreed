namespace OpenBreed.Editor.UI.WinForms.Controls.Sounds
{
    partial class SoundFromPcmEditorCtrl
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
            this.btnPlaySound = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlaySound
            // 
            this.btnPlaySound.Location = new System.Drawing.Point(17, 27);
            this.btnPlaySound.Name = "btnPlaySound";
            this.btnPlaySound.Size = new System.Drawing.Size(75, 23);
            this.btnPlaySound.TabIndex = 1;
            this.btnPlaySound.Text = "Play";
            this.btnPlaySound.UseVisualStyleBackColor = true;
            // 
            // SoundFromPcmEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPlaySound);
            this.Name = "SoundFromPcmEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPlaySound;
    }
}
