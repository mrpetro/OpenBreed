﻿namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class LoggerView
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
            this.LoggerCtrl = new OpenBreed.Editor.UI.WinForms.Controls.Logging.LoggerCtrl();
            this.SuspendLayout();
            // 
            // LoggerCtrl
            // 
            this.LoggerCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoggerCtrl.Location = new System.Drawing.Point(0, 0);
            this.LoggerCtrl.Name = "LoggerCtrl";
            this.LoggerCtrl.Size = new System.Drawing.Size(535, 268);
            this.LoggerCtrl.TabIndex = 0;
            // 
            // LoggerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 268);
            this.Controls.Add(this.LoggerCtrl);
            this.Name = "LoggerView";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Logging.LoggerCtrl LoggerCtrl;
    }
}
