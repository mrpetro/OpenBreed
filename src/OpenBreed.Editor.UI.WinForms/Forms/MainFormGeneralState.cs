﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.UI.WinForms.Views;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.UI.WinForms.Forms;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.UI.WinForms.Helpers;
using System.IO;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Common;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public class MainFormGeneralState
    {
        public readonly MainForm MainForm;

        internal ToolStripMenuItem NewLevelToolStripMenuItem = null;
        internal ToolStripMenuItem OpenLevelToolStripMenuItem = null;
        internal ToolStripMenuItem ExitToolStripMenuItem = null;

        public MainFormGeneralState(MainForm mainForm)
        {
            MainForm = mainForm;


            SetInitialState();
        }

        private void SetInitialState()
        {
            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Clear();

            NewLevelToolStripMenuItem = new ToolStripMenuItem("New Level");
            NewLevelToolStripMenuItem.Click += (s, a) => MainForm.VM.Project.TryNewLevel();
            OpenLevelToolStripMenuItem = new ToolStripMenuItem("Open Level...");
            OpenLevelToolStripMenuItem.Click += (s, a) => MainForm.VM.Project.TryOpenLevel();

            ExitToolStripMenuItem = new ToolStripMenuItem("Exit");
            ExitToolStripMenuItem.Click += (s, a) => MainForm.Close();

            MainForm.FileToolStripMenuItem.DropDownItems.Add(NewLevelToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(OpenLevelToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(ExitToolStripMenuItem);

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.DropDownItems.Clear();

            MainForm.EditToolStripMenuItem.Visible = false;
            MainForm.EditToolStripMenuItem.Enabled = false;
            MainForm.ViewToolStripMenuItem.Visible = false;
            MainForm.ViewToolStripMenuItem.Enabled = false;
        }
    }
}
