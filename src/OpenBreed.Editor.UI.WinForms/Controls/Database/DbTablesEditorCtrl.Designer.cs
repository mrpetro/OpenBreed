namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    partial class DbTablesEditorCtrl
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
            this.TableSelector = new OpenBreed.Editor.UI.WinForms.Controls.Database.DbTableSelectorCtrl();
            this.TableEditor = new OpenBreed.Editor.UI.WinForms.Controls.Database.DbTableEditorCtrl();
            this.SuspendLayout();
            // 
            // TableSelector
            // 
            this.TableSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.TableSelector.Location = new System.Drawing.Point(0, 0);
            this.TableSelector.Name = "TableSelector";
            this.TableSelector.Size = new System.Drawing.Size(665, 28);
            this.TableSelector.TabIndex = 0;
            // 
            // TableEditor
            // 
            this.TableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableEditor.Location = new System.Drawing.Point(0, 28);
            this.TableEditor.Name = "TableEditor";
            this.TableEditor.Size = new System.Drawing.Size(665, 341);
            this.TableEditor.TabIndex = 1;
            // 
            // DbTablesEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TableEditor);
            this.Controls.Add(this.TableSelector);
            this.Name = "DbTablesEditorCtrl";
            this.Size = new System.Drawing.Size(665, 369);
            this.ResumeLayout(false);

        }

        #endregion

        private DbTableSelectorCtrl TableSelector;
        private DbTableEditorCtrl TableEditor;
    }
}
