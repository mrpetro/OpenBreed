using System;
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
    public class MainFormGeneralHandler
    {
        private readonly MainForm m_MainForm;

        internal ToolStripMenuItem OpenProjectToolStripMenuItem = null;
        internal ToolStripMenuItem ExitToolStripMenuItem = null;

        public MainFormGeneralHandler(MainForm mainForm)
        {
            m_MainForm = mainForm;

            m_MainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            SetInitialState();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_MainForm._vm.CurrentDatabase != null)
            {
                if (m_MainForm._vm.Project.IsProjectOpened)
                    m_MainForm.LevelEditHandler.CloseProject();

                m_MainForm._vm.CloseDatabase();
            }
        }

        private void SetInitialState()
        {
            //Setup the File menu
            m_MainForm.FileToolStripMenuItem.DropDownItems.Clear();

            OpenProjectToolStripMenuItem = new ToolStripMenuItem("Open Project...");
            OpenProjectToolStripMenuItem.Click += (s, a) => Tools.TryAction(OpenProject);

            ExitToolStripMenuItem = new ToolStripMenuItem("Exit");
            ExitToolStripMenuItem.Click += (s, a) => Tools.TryAction(Exit);

            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(OpenProjectToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(ExitToolStripMenuItem);

            //Setup the View menu
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Clear();

            m_MainForm.EditToolStripMenuItem.Visible = false;
            m_MainForm.EditToolStripMenuItem.Enabled = false;
            m_MainForm.ViewToolStripMenuItem.Visible = false;
            m_MainForm.ViewToolStripMenuItem.Enabled = false;
        }

        private void Exit()
        {
            m_MainForm.Close();
        }

        void OpenProject()
        {
            m_MainForm.LevelEditHandler.OpenProject();
        }
    }
}
