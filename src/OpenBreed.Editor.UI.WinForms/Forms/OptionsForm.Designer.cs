namespace OpenBreed.Editor.UI.WinForms.Forms
{
    partial class OptionsForm
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabGeneral = new System.Windows.Forms.TabPage();
            this.TabABSE = new System.Windows.Forms.TabPage();
            this.TabABHC = new System.Windows.Forms.TabPage();
            this.TabABTA = new System.Windows.Forms.TabPage();
            this.OptionsABTA = new OpenBreed.Editor.UI.WinForms.Controls.EditorOptionsABTA();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.TabABTA.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.TabGeneral);
            this.Tabs.Controls.Add(this.TabABSE);
            this.Tabs.Controls.Add(this.TabABHC);
            this.Tabs.Controls.Add(this.TabABTA);
            this.Tabs.Location = new System.Drawing.Point(9, 12);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(563, 201);
            this.Tabs.TabIndex = 0;
            // 
            // TabGeneral
            // 
            this.TabGeneral.Location = new System.Drawing.Point(4, 22);
            this.TabGeneral.Name = "TabGeneral";
            this.TabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.TabGeneral.Size = new System.Drawing.Size(555, 175);
            this.TabGeneral.TabIndex = 0;
            this.TabGeneral.Text = "General";
            this.TabGeneral.UseVisualStyleBackColor = true;
            // 
            // TabABSE
            // 
            this.TabABSE.Location = new System.Drawing.Point(4, 22);
            this.TabABSE.Name = "TabABSE";
            this.TabABSE.Size = new System.Drawing.Size(555, 175);
            this.TabABSE.TabIndex = 2;
            this.TabABSE.Text = "AB: Special Edition";
            this.TabABSE.UseVisualStyleBackColor = true;
            // 
            // TabABHC
            // 
            this.TabABHC.Location = new System.Drawing.Point(4, 22);
            this.TabABHC.Name = "TabABHC";
            this.TabABHC.Size = new System.Drawing.Size(555, 175);
            this.TabABHC.TabIndex = 3;
            this.TabABHC.Text = "AB: The Horror Continues";
            this.TabABHC.UseVisualStyleBackColor = true;
            // 
            // TabABTA
            // 
            this.TabABTA.Controls.Add(this.OptionsABTA);
            this.TabABTA.Location = new System.Drawing.Point(4, 22);
            this.TabABTA.Name = "TabABTA";
            this.TabABTA.Padding = new System.Windows.Forms.Padding(3);
            this.TabABTA.Size = new System.Drawing.Size(555, 175);
            this.TabABTA.TabIndex = 1;
            this.TabABTA.Text = "AB: Tower Assault";
            this.TabABTA.UseVisualStyleBackColor = true;
            // 
            // OptionsABTA
            // 
            this.OptionsABTA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionsABTA.Location = new System.Drawing.Point(3, 3);
            this.OptionsABTA.Name = "OptionsABTA";
            this.OptionsABTA.Size = new System.Drawing.Size(549, 169);
            this.OptionsABTA.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(9, 219);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(71, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(501, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 251);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "OptionsForm";
            this.Text = "Editor options";
            this.Tabs.ResumeLayout(false);
            this.TabABTA.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabGeneral;
        private System.Windows.Forms.TabPage TabABSE;
        private System.Windows.Forms.TabPage TabABHC;
        private System.Windows.Forms.TabPage TabABTA;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Controls.EditorOptionsABTA OptionsABTA;
    }
}