
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
            this.grpPaletteFromMapSelection = new System.Windows.Forms.GroupBox();
            this.grpScriptAssetRefIdEditor = new System.Windows.Forms.GroupBox();
            this.ScriptAssetRefIdEditor = new OpenBreed.Editor.UI.WinForms.Controls.Common.EntryRefIdEditorCtrl();
            this.grpScript = new System.Windows.Forms.GroupBox();
            this.tbxText = new System.Windows.Forms.TextBox();
            this.grpPaletteFromMapSelection.SuspendLayout();
            this.grpScriptAssetRefIdEditor.SuspendLayout();
            this.grpScript.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPaletteFromMapSelection
            // 
            this.grpPaletteFromMapSelection.Controls.Add(this.grpScriptAssetRefIdEditor);
            this.grpPaletteFromMapSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPaletteFromMapSelection.Location = new System.Drawing.Point(0, 0);
            this.grpPaletteFromMapSelection.Name = "grpPaletteFromMapSelection";
            this.grpPaletteFromMapSelection.Size = new System.Drawing.Size(640, 66);
            this.grpPaletteFromMapSelection.TabIndex = 3;
            this.grpPaletteFromMapSelection.TabStop = false;
            this.grpPaletteFromMapSelection.Text = "Source data";
            // 
            // grpScriptAssetRefIdEditor
            // 
            this.grpScriptAssetRefIdEditor.Controls.Add(this.ScriptAssetRefIdEditor);
            this.grpScriptAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpScriptAssetRefIdEditor.Location = new System.Drawing.Point(3, 16);
            this.grpScriptAssetRefIdEditor.Name = "grpScriptAssetRefIdEditor";
            this.grpScriptAssetRefIdEditor.Size = new System.Drawing.Size(634, 47);
            this.grpScriptAssetRefIdEditor.TabIndex = 4;
            this.grpScriptAssetRefIdEditor.TabStop = false;
            this.grpScriptAssetRefIdEditor.Text = "Script asset reference ID";
            // 
            // ScriptAssetRefIdEditor
            // 
            this.ScriptAssetRefIdEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptAssetRefIdEditor.Location = new System.Drawing.Point(3, 16);
            this.ScriptAssetRefIdEditor.Name = "ScriptAssetRefIdEditor";
            this.ScriptAssetRefIdEditor.Size = new System.Drawing.Size(628, 28);
            this.ScriptAssetRefIdEditor.TabIndex = 0;
            // 
            // grpScript
            // 
            this.grpScript.Controls.Add(this.tbxText);
            this.grpScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpScript.Location = new System.Drawing.Point(0, 66);
            this.grpScript.Name = "grpScript";
            this.grpScript.Size = new System.Drawing.Size(640, 414);
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
            this.tbxText.Size = new System.Drawing.Size(634, 395);
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
            this.grpScriptAssetRefIdEditor.ResumeLayout(false);
            this.grpScript.ResumeLayout(false);
            this.grpScript.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpPaletteFromMapSelection;
        private System.Windows.Forms.GroupBox grpScript;
        private System.Windows.Forms.TextBox tbxText;
        private System.Windows.Forms.GroupBox grpScriptAssetRefIdEditor;
        private Common.EntryRefIdEditorCtrl ScriptAssetRefIdEditor;
    }
}
