using OpenBreed.Common.UI.WinForms.Controls;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Forms.States
{
    internal class FormStateDatabaseOpened : FormState
    {

        #region Internal Fields

        //File menu
        internal ToolStripMenuItem ExitToolStripMenuItem = null;

        internal ToolStripMenuItem FileCloseDatabaseToolStripMenuItem = null;
        internal ToolStripMenuItem FileOpenDatabaseToolStripMenuItem = null;
        internal ToolStripMenuItem FileSaveDatabaseToolStripMenuItem = null;
        internal ToolStripSeparator FileSeparator = null;

        //View menu
        internal ToolStripMenuItemEx ViewDatabaseMenuItem = null;

        #endregion Internal Fields

        #region Internal Constructors

        internal FormStateDatabaseOpened(MainForm mainForm) : base(mainForm)
        {
            FileSeparator = new ToolStripSeparator();

            FileCloseDatabaseToolStripMenuItem = new ToolStripMenuItem("Close database");
            FileCloseDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.TryCloseDatabase(MainForm.EditorView.VM);
            FileOpenDatabaseToolStripMenuItem = new ToolStripMenuItem("Open Database...");
            FileOpenDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.TryOpenXmlDatabase(MainForm.EditorView.VM);
            FileSaveDatabaseToolStripMenuItem = new ToolStripMenuItem("Save Database");
            FileSaveDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.TrySaveDatabase();
            ExitToolStripMenuItem = new ToolStripMenuItem("Exit");
            ExitToolStripMenuItem.Click += (s, a) => MainForm.VM.TryExit();

            ViewDatabaseMenuItem = new ToolStripMenuItemEx("Database items");
            ViewDatabaseMenuItem.CheckOnClick = true;
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal override void Cleanup()
        {
            MainForm.EditorView.HideAllViews();

            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Clear();

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = false;
            MainForm.ViewToolStripMenuItem.Enabled = false;

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewDatabaseMenuItem);
        }

        internal override void Setup()
        {
            if (MainForm.VM.DbName == null)
                throw new InvalidOperationException("No current database!");

            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileOpenDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSaveDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileCloseDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(ExitToolStripMenuItem);

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = true;
            MainForm.EditToolStripMenuItem.Visible = true;

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.Enabled = true;
            MainForm.ViewToolStripMenuItem.Visible = true;


            ViewDatabaseMenuItem.DataBindings.Clear();
            ViewDatabaseMenuItem.DataBindings.Add(nameof(ViewDatabaseMenuItem.Checked),
                                                  MainForm.EditorView.VM,
                                                  nameof(MainForm.EditorView.VM.DbTablesEditorChecked),
                                                  false,
                                                  DataSourceUpdateMode.OnPropertyChanged);

            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewDatabaseMenuItem);

            MainForm.EditorView.VM.InitDbTablesEditorAction = OnInitDbTablesEditor;
            MainForm.EditorView.VM.DbTablesEditorChecked = true;
        }

        #endregion Internal Methods


        #region Private Methods

        private void OnInitDbTablesEditor(DbTablesEditorVM dbTablesEditorVm)
        {
            var databaseView = new DbTablesEditorView();
            databaseView.Initialize(dbTablesEditorVm);
            databaseView.Show(MainForm.EditorView, DockState.DockLeft);
        }

        #endregion Private Methods

    }
}