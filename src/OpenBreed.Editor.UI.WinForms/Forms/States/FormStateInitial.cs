using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Forms.States
{
    internal class FormStateInitial : FormState
    {
        #region Internal Fields

        internal ToolStripMenuItem ExitToolStripMenuItem = null;
        internal ToolStripMenuItem FileOpenDatabaseToolStripMenuItem = null;

        //internal ToolStripMenuItem NewLevelToolStripMenuItem = null;
        internal ToolStripSeparator FileSeparator = new ToolStripSeparator();

        #endregion Internal Fields

        #region Internal Constructors

        internal FormStateInitial(MainForm mainForm) : base(mainForm)
        {
            //NewLevelToolStripMenuItem = new ToolStripMenuItem("New Level");
            //NewLevelToolStripMenuItem.Click += (s, a) => MainForm.VM.Project.TryNewLevel();
            FileOpenDatabaseToolStripMenuItem = new ToolStripMenuItem("Open Database...");
            FileOpenDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.TryOpenDatabase();
            ExitToolStripMenuItem = new ToolStripMenuItem("Exit");
            ExitToolStripMenuItem.Click += (s, a) => MainForm.Close();
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal override void Cleanup()
        {
            MainForm.FileToolStripMenuItem.DropDownItems.Clear();
            MainForm.ViewToolStripMenuItem.DropDownItems.Clear();
        }

        internal override void Setup()
        {
            //Setup the File menu
            //MainForm.FileToolStripMenuItem.DropDownItems.Add(NewLevelToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileOpenDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(ExitToolStripMenuItem);

            //Setup the View menu
            MainForm.EditToolStripMenuItem.Visible = false;
            MainForm.EditToolStripMenuItem.Enabled = false;
            MainForm.ViewToolStripMenuItem.Visible = false;
            MainForm.ViewToolStripMenuItem.Enabled = false;
        }

        #endregion Internal Methods
    }
}
