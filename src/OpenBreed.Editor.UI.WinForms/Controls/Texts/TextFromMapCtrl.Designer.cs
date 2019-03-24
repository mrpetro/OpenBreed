
namespace OpenBreed.Editor.UI.WinForms.Controls.Texts
{
    partial class TextFromMapCtrl
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
            this.tbxMapDataRef = new System.Windows.Forms.TextBox();
            this.cbxMapBlockName = new System.Windows.Forms.ComboBox();
            this.grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            this.lblMapBlockName = new System.Windows.Forms.Label();
            this.lblMapDataRef = new System.Windows.Forms.Label();
            this.grpText = new System.Windows.Forms.GroupBox();
            this.tbxText = new System.Windows.Forms.TextBox();
            this.grpPaletteFromMapSelection.SuspendLayout();
            this.grpText.SuspendLayout();
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
            // tbxMapDataRef
            // 
            this.tbxMapDataRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMapDataRef.Location = new System.Drawing.Point(98, 19);
            this.tbxMapDataRef.Name = "tbxMapDataRef";
            this.tbxMapDataRef.Size = new System.Drawing.Size(504, 20);
            this.tbxMapDataRef.TabIndex = 1;
            // 
            // cbxMapBlockName
            // 
            this.cbxMapBlockName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxMapBlockName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMapBlockName.FormattingEnabled = true;
            this.cbxMapBlockName.Location = new System.Drawing.Point(98, 53);
            this.cbxMapBlockName.Name = "cbxMapBlockName";
            this.cbxMapBlockName.Size = new System.Drawing.Size(504, 21);
            this.cbxMapBlockName.TabIndex = 2;
            // 
            // grpPaletteFromMapSelection
            // 
            this.grpPaletteFromMapSelection.Controls.Add(this.lblMapBlockName);
            this.grpPaletteFromMapSelection.Controls.Add(this.lblMapDataRef);
            this.grpPaletteFromMapSelection.Controls.Add(this.cbxMapBlockName);
            this.grpPaletteFromMapSelection.Controls.Add(this.btnDataRefBrowser);
            this.grpPaletteFromMapSelection.Controls.Add(this.tbxMapDataRef);
            this.grpPaletteFromMapSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPaletteFromMapSelection.Location = new System.Drawing.Point(0, 0);
            this.grpPaletteFromMapSelection.Name = "grpPaletteFromMapSelection";
            this.grpPaletteFromMapSelection.Size = new System.Drawing.Size(640, 82);
            this.grpPaletteFromMapSelection.TabIndex = 3;
            this.grpPaletteFromMapSelection.TabStop = false;
            this.grpPaletteFromMapSelection.Text = "Source data";
            // 
            // lblMapBlockName
            // 
            this.lblMapBlockName.AutoSize = true;
            this.lblMapBlockName.Location = new System.Drawing.Point(6, 56);
            this.lblMapBlockName.Name = "lblMapBlockName";
            this.lblMapBlockName.Size = new System.Drawing.Size(89, 13);
            this.lblMapBlockName.TabIndex = 4;
            this.lblMapBlockName.Text = "Map block name:";
            // 
            // lblMapDataRef
            // 
            this.lblMapDataRef.AutoSize = true;
            this.lblMapDataRef.Location = new System.Drawing.Point(6, 23);
            this.lblMapDataRef.Name = "lblMapDataRef";
            this.lblMapDataRef.Size = new System.Drawing.Size(79, 13);
            this.lblMapDataRef.TabIndex = 3;
            this.lblMapDataRef.Text = "Map reference:";
            // 
            // grpText
            // 
            this.grpText.Controls.Add(this.tbxText);
            this.grpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpText.Location = new System.Drawing.Point(0, 82);
            this.grpText.Name = "grpText";
            this.grpText.Size = new System.Drawing.Size(640, 398);
            this.grpText.TabIndex = 15;
            this.grpText.TabStop = false;
            this.grpText.Text = "Text";
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
            // TextFromMapCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpText);
            this.Controls.Add(this.grpPaletteFromMapSelection);
            this.Name = "TextFromMapCtrl";
            this.Size = new System.Drawing.Size(640, 480);
            this.grpPaletteFromMapSelection.ResumeLayout(false);
            this.grpPaletteFromMapSelection.PerformLayout();
            this.grpText.ResumeLayout(false);
            this.grpText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDataRefBrowser;
        private System.Windows.Forms.TextBox tbxMapDataRef;
        private System.Windows.Forms.ComboBox cbxMapBlockName;
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.Label lblMapBlockName;
        private System.Windows.Forms.Label lblMapDataRef;
        private System.Windows.Forms.GroupBox grpText;
        private System.Windows.Forms.TextBox tbxText;
    }
}
