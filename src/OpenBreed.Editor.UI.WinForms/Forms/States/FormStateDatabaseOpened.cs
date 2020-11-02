using OpenBreed.Common;
using OpenBreed.Editor.UI.WinForms.Forms.States;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        internal ToolStripMenuItem ViewDatabaseMenuItem = null;

        #endregion Internal Fields

        #region Private Fields

        #endregion Private Fields

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
            //ViewMapBodyMenuItem = new ToolStripMenuItem("Map body");
            //ViewMapBodyMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapBody);
            //ViewMapPropertiesMenuItem = new ToolStripMenuItem("Map properties");
            //ViewMapPropertiesMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapProperties);
            //ViewMapPalettesMenuItem = new ToolStripMenuItem("Palettes");
            //ViewMapPalettesMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapPalettes);
            //ViewTileSetMenuItem = new ToolStripMenuItem("Tile set");
            //ViewTileSetMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.TileSet);
            //ViewSpriteSetsMenuItem = new ToolStripMenuItem("Sprite sets");
            //ViewSpriteSetsMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.SpriteSets);
            //ViewPropertySetMenuItem = new ToolStripMenuItem("Property set");
            //ViewPropertySetMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.PropertySet);
            //ViewImagesMenuItem = new ToolStripMenuItem("Images");
            //ViewImagesMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.Images);
            //ViewToolsMenuItem = new ToolStripMenuItem("Tools");
            //ViewToolsMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.Tools);
            ViewDatabaseMenuItem = new ToolStripMenuItem("Database items");
            ViewDatabaseMenuItem.Click += (s, a) => MainForm.EditorView.ShowDatabaseView();
        }

        private void OnOpenedItemChanged(DbEntryVM databaseItemVM)
        {
            if (databaseItemVM == null)
                return;
            else if (databaseItemVM is DbMapEntryVM)
                MainForm.EditorView.ShowLevelView();
            else if (databaseItemVM is DbSpriteSetEntryVM)
                MainForm.EditorView.ShowSpriteSetEditorView();
            else
                throw new NotImplementedException();
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

            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewDatabaseMenuItem);

            MainForm.Text = $"{MainForm.APP_NAME} - {MainForm.VM.DbEditor.DbName}";

            //_projectView.ActiveContentChanged += new EventHandler(ProjectView_ActiveContentChanged);

            MainForm.EditorView.ShowDatabaseView();

        }

    #endregion Internal Methods
}
}
