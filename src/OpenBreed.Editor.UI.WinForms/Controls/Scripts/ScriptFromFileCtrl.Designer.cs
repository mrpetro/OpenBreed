
namespace OpenBreed.Editor.UI.WinForms.Controls.Scripts
{
    partial class ScriptFromFileCtrl
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
            this.btnDataRefBrowser = new System.Windows.Forms.Button();
            this.tbxAssetDataRef = new System.Windows.Forms.TextBox();
            this.grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            this.lblAssetDataRef = new System.Windows.Forms.Label();
            this.grpScript = new System.Windows.Forms.GroupBox();
            this.tbxText = new System.Windows.Forms.TextBox();
            this.grpPaletteFromMapSelection.SuspendLayout();
            this.grpScript.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDataRefBrowser
            // 
            this.btnDataRefBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataRefBrowser.Location = new System.Drawing.Point(608, 19);
            this.btnDataRefBrowser.Name = "btnDataRefBrowser";
            this.btnDataRefBrowser.Size = new System.Drawing.Size(26, 20);
            this.btnDataRefBrowser.TabIndex = 0;
            this.btnDataRefBrowser.Text = "...";
            this.btnDataRefBrowser.UseVisualStyleBackColor = true;
            // 
            // tbxAssetDataRef
            // 
            this.tbxAssetDataRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxAssetDataRef.Location = new System.Drawing.Point(98, 19);
            this.tbxAssetDataRef.Name = "tbxAssetDataRef";
            this.tbxAssetDataRef.Size = new System.Drawing.Size(504, 20);
            this.tbxAssetDataRef.TabIndex = 1;
            // 
            // grpPaletteFromMapSelection
            // 
            this.grpPaletteFromMapSelection.Controls.Add(this.lblAssetDataRef);
            this.grpPaletteFromMapSelection.Controls.Add(this.btnDataRefBrowser);
            this.grpPaletteFromMapSelection.Controls.Add(this.tbxAssetDataRef);
            this.grpPaletteFromMapSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPaletteFromMapSelection.Location = new System.Drawing.Point(0, 0);
            this.grpPaletteFromMapSelection.Name = "grpPaletteFromMapSelection";
            this.grpPaletteFromMapSelection.Size = new System.Drawing.Size(640, 82);
            this.grpPaletteFromMapSelection.TabIndex = 3;
            this.grpPaletteFromMapSelection.TabStop = false;
            this.grpPaletteFromMapSelection.Text = "Source data";
            // 
            // lblAssetDataRef
            // 
            this.lblAssetDataRef.AutoSize = true;
            this.lblAssetDataRef.Location = new System.Drawing.Point(6, 23);
            this.lblAssetDataRef.Name = "lblAssetDataRef";
            this.lblAssetDataRef.Size = new System.Drawing.Size(74, 13);
            this.lblAssetDataRef.TabIndex = 3;
            this.lblAssetDataRef.Text = "File reference:";
            // 
            // grpScript
            // 
            this.grpScript.Controls.Add(this.tbxText);
            this.grpScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpScript.Location = new System.Drawing.Point(0, 82);
            this.grpScript.Name = "grpScript";
            this.grpScript.Size = new System.Drawing.Size(640, 398);
            this.grpScript.TabIndex = 15;
            this.grpScript.TabStop = false;
            this.grpScript.Text = "Script";
            // 
            // tbxText
            // 
            this.tbxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxText.Location = new System.Drawing.Point(3, 16);
            this.tbxText.Multiline = true;
            this.tbxText.Name = "tbxText";
            this.tbxText.Size = new System.Drawing.Size(634, 379);
            this.tbxText.TabIndex = 14;
            // 
            // ScriptFromFileCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpScript);
            this.Controls.Add(this.grpPaletteFromMapSelection);
            this.Name = "ScriptFromFileCtrl";
            this.Size = new System.Drawing.Size(640, 480);
            this.grpPaletteFromMapSelection.ResumeLayout(false);
            this.grpPaletteFromMapSelection.PerformLayout();
            this.grpScript.ResumeLayout(false);
            this.grpScript.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.TextBox tbxAssetDataRef;
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.Label lblAssetDataRef;
        private System.Windows.Forms.GroupBox grpScript;
        private System.Windows.Forms.TextBox tbxText;
    }
}
