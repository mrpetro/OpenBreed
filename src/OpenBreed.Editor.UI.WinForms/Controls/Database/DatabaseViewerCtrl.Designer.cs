namespace OpenBreed.Editor.UI.WinForms.Controls.Database
{
    partial class DatabaseViewerCtrl
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
            this.TableSelector = new OpenBreed.Editor.UI.WinForms.Controls.Database.DatabaseTableSelectorCtrl();
            this.TableViewer = new OpenBreed.Editor.UI.WinForms.Controls.Database.DatabaseTableViewerCtrl();
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
            // TableViewer
            // 
            this.TableViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableViewer.Location = new System.Drawing.Point(0, 28);
            this.TableViewer.Name = "TableViewer";
            this.TableViewer.Size = new System.Drawing.Size(665, 341);
            this.TableViewer.TabIndex = 1;
            // 
            // DatabaseViewerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TableViewer);
            this.Controls.Add(this.TableSelector);
            this.Name = "DatabaseViewerCtrl";
            this.Size = new System.Drawing.Size(665, 369);
            this.ResumeLayout(false);

        }

        #endregion

        private DatabaseTableSelectorCtrl TableSelector;
        private DatabaseTableViewerCtrl TableViewer;
    }
}
