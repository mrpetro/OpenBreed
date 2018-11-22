﻿
namespace OpenBreed.Editor.UI.WinForms.Forms
{
    partial class MainForm
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
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ABTAGamePasswordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ABTAGameRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EPFPackUnpackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogConsoleShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MessageLabelMainStatusStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenuStrip.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.EditToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1041, 24);
            this.MainMenuStrip.TabIndex = 2;
            this.MainMenuStrip.Text = "EditorMenu";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.EditToolStripMenuItem.Text = "Edit";
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.ViewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ABTAGamePasswordsToolStripMenuItem,
            this.ABTAGameRunToolStripMenuItem,
            this.EPFPackUnpackToolStripMenuItem,
            this.LogConsoleShowToolStripMenuItem,
            this.OptionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // ABTAGamePasswordsToolStripMenuItem
            // 
            this.ABTAGamePasswordsToolStripMenuItem.Name = "ABTAGamePasswordsToolStripMenuItem";
            this.ABTAGamePasswordsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.ABTAGamePasswordsToolStripMenuItem.Text = "ABTA Game passwords...";
            this.ABTAGamePasswordsToolStripMenuItem.Click += new System.EventHandler(this.ABTAGamePasswordsToolStripMenuItem_Click);
            // 
            // ABTAGameRunToolStripMenuItem
            // 
            this.ABTAGameRunToolStripMenuItem.Name = "ABTAGameRunToolStripMenuItem";
            this.ABTAGameRunToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.ABTAGameRunToolStripMenuItem.Text = "Run ABTA Game";
            this.ABTAGameRunToolStripMenuItem.Click += new System.EventHandler(this.ABTAGameRunToolStripMenuItem_Click);
            // 
            // EPFPackUnpackToolStripMenuItem
            // 
            this.EPFPackUnpackToolStripMenuItem.Name = "EPFPackUnpackToolStripMenuItem";
            this.EPFPackUnpackToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.EPFPackUnpackToolStripMenuItem.Text = "EPF pack/unpack...";
            // 
            // LogConsoleShowToolStripMenuItem
            // 
            this.LogConsoleShowToolStripMenuItem.CheckOnClick = true;
            this.LogConsoleShowToolStripMenuItem.Name = "LogConsoleShowToolStripMenuItem";
            this.LogConsoleShowToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.LogConsoleShowToolStripMenuItem.Text = "Log Console";
            this.LogConsoleShowToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ShowLogConsoleToolStripMenuItem_CheckedChanged);
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.OptionsToolStripMenuItem.Text = "Options...";
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MessageLabelMainStatusStrip});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 482);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(1041, 22);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 0;
            this.MainStatusStrip.Text = "MainStatusStrip";
            // 
            // MessageLabelMainStatusStrip
            // 
            this.MessageLabelMainStatusStrip.Name = "MessageLabelMainStatusStrip";
            this.MessageLabelMainStatusStrip.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 504);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MainMenuStrip);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "Alien Breed Map Editor";
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MessageLabelMainStatusStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ABTAGamePasswordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EPFPackUnpackToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ABTAGameRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogConsoleShowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}

