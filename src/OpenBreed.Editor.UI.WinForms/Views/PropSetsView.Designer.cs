namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class PropSetsView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PropSets = new OpenBreed.Editor.UI.WinForms.Controls.Props.PropSetsCtrl();
            this.PropSelector = new OpenBreed.Editor.UI.WinForms.Controls.Props.PropSelectorCtrl();
            this.SuspendLayout();
            // 
            // PropSets
            // 
            this.PropSets.Dock = System.Windows.Forms.DockStyle.Top;
            this.PropSets.Location = new System.Drawing.Point(0, 0);
            this.PropSets.Name = "PropSets";
            this.PropSets.Size = new System.Drawing.Size(802, 28);
            this.PropSets.TabIndex = 0;
            // 
            // PropSelector
            // 
            this.PropSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropSelector.Location = new System.Drawing.Point(0, 28);
            this.PropSelector.Name = "PropSelector";
            this.PropSelector.Size = new System.Drawing.Size(802, 382);
            this.PropSelector.TabIndex = 1;
            // 
            // PropSetsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 410);
            this.Controls.Add(this.PropSelector);
            this.Controls.Add(this.PropSets);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "PropSetsView";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Props.PropSetsCtrl PropSets;
        private Controls.Props.PropSelectorCtrl PropSelector;
    }
}