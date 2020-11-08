using OpenBreed.Common.UI.WinForms.Controls;
using OpenBreed.Editor.UI.WinForms.Views;
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
            FileCloseDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.DbEditor.TryCloseDatabase();
            FileOpenDatabaseToolStripMenuItem = new ToolStripMenuItem("Open Database...");
            FileOpenDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.DbEditor.TryOpenXmlDatabase();
            FileSaveDatabaseToolStripMenuItem = new ToolStripMenuItem("Save Database");
            FileSaveDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.DbEditor.TrySaveDatabase();
            ExitToolStripMenuItem = new ToolStripMenuItem("Exit");
            ExitToolStripMenuItem.Click += (s, a) => MainForm.Close();

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
            if (MainForm.VM.DbEditor.DbName == null)
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


            ViewDatabaseMenuItem = new ToolStripMenuItemEx("Database items");
            ViewDatabaseMenuItem.CheckOnClick = true;
            ViewDatabaseMenuItem.DataBindings.Add(nameof(ViewDatabaseMenuItem.Checked),
                                                  MainForm.VM.DbEditor,
                                                  nameof(MainForm.VM.DbEditor.DbTablesEditorChecked),
                                                  false,
                                                  DataSourceUpdateMode.OnPropertyChanged);
            //ViewDatabaseMenuItem.Click += (s, a) => MainForm.VM.ToggleDbTablesEditor(true);

            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewDatabaseMenuItem);

            MainForm.Text = $"{MainForm.APP_NAME} - {MainForm.VM.DbEditor.DbName}";

            MainForm.VM.DbEditor.InitDbTablesEditorAction = OnInitDbTablesEditor;
            MainForm.VM.DbEditor.DbTablesEditorChecked = true;
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