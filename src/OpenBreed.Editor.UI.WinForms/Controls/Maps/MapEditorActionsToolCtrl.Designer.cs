namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    partial class MapEditorActionsToolCtrl
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
            this.LayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.EntryRef = new OpenBreed.Editor.UI.WinForms.Controls.Common.EntryRefCtrl();
            this.ActionsSelector = new OpenBreed.Editor.UI.WinForms.Controls.Maps.MapEditorActionsSelectorCtrl();
            this.LayoutTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutTable
            // 
            this.LayoutTable.ColumnCount = 1;
            this.LayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LayoutTable.Controls.Add(this.EntryRef, 0, 0);
            this.LayoutTable.Controls.Add(this.ActionsSelector, 0, 1);
            this.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutTable.Location = new System.Drawing.Point(0, 0);
            this.LayoutTable.Name = "LayoutTable";
            this.LayoutTable.RowCount = 2;
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Size = new System.Drawing.Size(633, 315);
            this.LayoutTable.TabIndex = 0;
            // 
            // EntryRef
            // 
            this.EntryRef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntryRef.Location = new System.Drawing.Point(3, 3);
            this.EntryRef.Name = "EntryRef";
            this.EntryRef.Size = new System.Drawing.Size(627, 28);
            this.EntryRef.TabIndex = 0;
            // 
            // ActionsSelector
            // 
            this.ActionsSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsSelector.Location = new System.Drawing.Point(3, 37);
            this.ActionsSelector.Name = "ActionsSelector";
            this.ActionsSelector.Size = new System.Drawing.Size(627, 275);
            this.ActionsSelector.TabIndex = 1;
            // 
            // MapEditorActionsToolCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LayoutTable);
            this.Name = "MapEditorActionsToolCtrl";
            this.Size = new System.Drawing.Size(633, 315);
            this.LayoutTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutTable;
        private Common.EntryRefCtrl EntryRef;
        private MapEditorActionsSelectorCtrl ActionsSelector;
    }
}
