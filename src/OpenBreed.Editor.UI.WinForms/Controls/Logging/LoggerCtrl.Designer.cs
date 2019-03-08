namespace OpenBreed.Editor.UI.WinForms.Controls.Logging
{
    partial class LoggerCtrl
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
            this.DGV = new System.Windows.Forms.DataGridView();
            this.grpLogger = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.grpLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(3, 20);
            this.DGV.Name = "DGV";
            this.DGV.Size = new System.Drawing.Size(485, 217);
            this.DGV.TabIndex = 0;
            // 
            // grpLogger
            // 
            this.grpLogger.Controls.Add(this.btnClear);
            this.grpLogger.Controls.Add(this.DGV);
            this.grpLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLogger.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.grpLogger.Location = new System.Drawing.Point(0, 0);
            this.grpLogger.Name = "grpLogger";
            this.grpLogger.Size = new System.Drawing.Size(491, 240);
            this.grpLogger.TabIndex = 1;
            this.grpLogger.TabStop = false;
            this.grpLogger.Text = "Message log";
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnClear.Location = new System.Drawing.Point(444, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(44, 20);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // LoggerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpLogger);
            this.Name = "LoggerCtrl";
            this.Size = new System.Drawing.Size(491, 240);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.grpLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.GroupBox grpLogger;
        private System.Windows.Forms.Button btnClear;
    }
}
