namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    partial class MapEditorCtrl
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
            this.MapView = new OpenBreed.Editor.UI.WinForms.Controls.Maps.MapEditorViewCtrl();
            this.ToolTabs = new System.Windows.Forms.TabControl();
            this.TabTiles = new System.Windows.Forms.TabPage();
            this.TabActions = new System.Windows.Forms.TabPage();
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.TabLayers = new System.Windows.Forms.TabPage();
            this.SideSplit = new System.Windows.Forms.SplitContainer();
            this.TabTemplates = new System.Windows.Forms.TabPage();
            this.GrpTools = new System.Windows.Forms.GroupBox();
            this.GrpLayers = new System.Windows.Forms.GroupBox();
            this.ToolTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel1.SuspendLayout();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SideSplit)).BeginInit();
            this.SideSplit.Panel1.SuspendLayout();
            this.SideSplit.Panel2.SuspendLayout();
            this.SideSplit.SuspendLayout();
            this.GrpTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapView
            // 
            this.MapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapView.Location = new System.Drawing.Point(0, 0);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(467, 456);
            this.MapView.TabIndex = 0;
            // 
            // ToolTabs
            // 
            this.ToolTabs.Controls.Add(this.TabTiles);
            this.ToolTabs.Controls.Add(this.TabActions);
            this.ToolTabs.Controls.Add(this.TabLayers);
            this.ToolTabs.Controls.Add(this.TabTemplates);
            this.ToolTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolTabs.Location = new System.Drawing.Point(3, 16);
            this.ToolTabs.Name = "ToolTabs";
            this.ToolTabs.SelectedIndex = 0;
            this.ToolTabs.Size = new System.Drawing.Size(327, 304);
            this.ToolTabs.TabIndex = 1;
            // 
            // TabTiles
            // 
            this.TabTiles.Location = new System.Drawing.Point(4, 22);
            this.TabTiles.Name = "TabTiles";
            this.TabTiles.Padding = new System.Windows.Forms.Padding(3);
            this.TabTiles.Size = new System.Drawing.Size(319, 278);
            this.TabTiles.TabIndex = 0;
            this.TabTiles.Text = "Tiles";
            this.TabTiles.UseVisualStyleBackColor = true;
            // 
            // TabActions
            // 
            this.TabActions.Location = new System.Drawing.Point(4, 22);
            this.TabActions.Name = "TabActions";
            this.TabActions.Padding = new System.Windows.Forms.Padding(3);
            this.TabActions.Size = new System.Drawing.Size(315, 297);
            this.TabActions.TabIndex = 1;
            this.TabActions.Text = "Actions";
            this.TabActions.UseVisualStyleBackColor = true;
            // 
            // MainSplit
            // 
            this.MainSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.MainSplit.Location = new System.Drawing.Point(0, 0);
            this.MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            this.MainSplit.Panel1.Controls.Add(this.MapView);
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.Controls.Add(this.SideSplit);
            this.MainSplit.Panel2MinSize = 330;
            this.MainSplit.Size = new System.Drawing.Size(812, 460);
            this.MainSplit.SplitterDistance = 471;
            this.MainSplit.TabIndex = 2;
            // 
            // TabLayers
            // 
            this.TabLayers.Location = new System.Drawing.Point(4, 22);
            this.TabLayers.Name = "TabLayers";
            this.TabLayers.Size = new System.Drawing.Size(315, 297);
            this.TabLayers.TabIndex = 2;
            this.TabLayers.Text = "Layers";
            this.TabLayers.UseVisualStyleBackColor = true;
            // 
            // SideSplit
            // 
            this.SideSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SideSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SideSplit.Location = new System.Drawing.Point(0, 0);
            this.SideSplit.Name = "SideSplit";
            this.SideSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SideSplit.Panel1
            // 
            this.SideSplit.Panel1.Controls.Add(this.GrpLayers);
            // 
            // SideSplit.Panel2
            // 
            this.SideSplit.Panel2.Controls.Add(this.GrpTools);
            this.SideSplit.Size = new System.Drawing.Size(333, 456);
            this.SideSplit.SplitterDistance = 129;
            this.SideSplit.TabIndex = 2;
            // 
            // TabTemplates
            // 
            this.TabTemplates.Location = new System.Drawing.Point(4, 22);
            this.TabTemplates.Name = "TabTemplates";
            this.TabTemplates.Padding = new System.Windows.Forms.Padding(3);
            this.TabTemplates.Size = new System.Drawing.Size(315, 297);
            this.TabTemplates.TabIndex = 3;
            this.TabTemplates.Text = "Templates";
            this.TabTemplates.UseVisualStyleBackColor = true;
            // 
            // GrpTools
            // 
            this.GrpTools.Controls.Add(this.ToolTabs);
            this.GrpTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpTools.Location = new System.Drawing.Point(0, 0);
            this.GrpTools.Name = "GrpTools";
            this.GrpTools.Size = new System.Drawing.Size(333, 323);
            this.GrpTools.TabIndex = 2;
            this.GrpTools.TabStop = false;
            this.GrpTools.Text = "Tools";
            // 
            // GrpLayers
            // 
            this.GrpLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpLayers.Location = new System.Drawing.Point(0, 0);
            this.GrpLayers.Name = "GrpLayers";
            this.GrpLayers.Size = new System.Drawing.Size(333, 129);
            this.GrpLayers.TabIndex = 0;
            this.GrpLayers.TabStop = false;
            this.GrpLayers.Text = "Layers";
            // 
            // MapEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainSplit);
            this.Name = "MapEditorCtrl";
            this.Size = new System.Drawing.Size(812, 460);
            this.ToolTabs.ResumeLayout(false);
            this.MainSplit.Panel1.ResumeLayout(false);
            this.MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.SideSplit.Panel1.ResumeLayout(false);
            this.SideSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SideSplit)).EndInit();
            this.SideSplit.ResumeLayout(false);
            this.GrpTools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MapEditorViewCtrl MapView;
        private System.Windows.Forms.TabControl ToolTabs;
        private System.Windows.Forms.TabPage TabTiles;
        private System.Windows.Forms.TabPage TabActions;
        private System.Windows.Forms.TabPage TabLayers;
        private System.Windows.Forms.SplitContainer MainSplit;
        private System.Windows.Forms.TabPage TabTemplates;
        private System.Windows.Forms.SplitContainer SideSplit;
        private System.Windows.Forms.GroupBox GrpLayers;
        private System.Windows.Forms.GroupBox GrpTools;
    }
}
