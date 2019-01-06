using OpenBreed.Common;
using OpenBreed.Editor.UI.WinForms.Forms.States;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Items;
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
        internal ToolStripSeparator FileSeparator = null;

        //View menu
        //internal ToolStripMenuItem ViewImagesMenuItem = null;
        //internal ToolStripMenuItem ViewMapBodyMenuItem = null;

        //internal ToolStripMenuItem ViewMapPalettesMenuItem = null;
        //internal ToolStripMenuItem ViewMapPropertiesMenuItem = null;
        //internal ToolStripMenuItem ViewPropertySetMenuItem = null;
        //internal ToolStripMenuItem ViewSpriteSetsMenuItem = null;
        //internal ToolStripMenuItem ViewTileSetMenuItem = null;
        //internal ToolStripMenuItem ViewToolsMenuItem = null;
        internal ToolStripMenuItem ViewDatabaseMenuItem = null;

        #endregion Internal Fields

        #region Private Fields

        private DbEditorView _dbEditorView;

        #endregion Private Fields

        #region Internal Constructors

        internal FormStateDatabaseOpened(MainForm mainForm) : base(mainForm)
        {
            FileSeparator = new ToolStripSeparator();

            _dbEditorView = new DbEditorView();
            _dbEditorView.Dock = DockStyle.Fill;

            FileCloseDatabaseToolStripMenuItem = new ToolStripMenuItem("Close database");
            FileCloseDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.DbEditor.TryCloseDatabase();
            FileOpenDatabaseToolStripMenuItem = new ToolStripMenuItem("Open Database...");
            FileOpenDatabaseToolStripMenuItem.Click += (s, a) => MainForm.VM.DbEditor.TryOpenDatabase();
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
            ViewDatabaseMenuItem.Click += (s, a) => _dbEditorView.ShowDatabaseView();
        }

        private void OnOpenedItemChanged(DbEntryVM databaseItemVM)
        {
            if (databaseItemVM == null)
                return;
            else if (databaseItemVM is DbImageEntryVM)
                _dbEditorView.ShowImageView();
            else if (databaseItemVM is DbLevelEntryVM)
                _dbEditorView.ShowLevelView();
            else if (databaseItemVM is DbPropSetEntryVM)
                _dbEditorView.ShowPropSetEditorView();
            else if (databaseItemVM is DbTileSetEntryVM)
                _dbEditorView.ShowTileSetEditorView();
            else if (databaseItemVM is DbSpriteSetEntryVM)
                _dbEditorView.ShowSpriteSetEditorView();
            else if (databaseItemVM is DbPaletteEntryVM)
                _dbEditorView.ShowPaletteEditorView();
            else if (databaseItemVM is DbSoundEntryVM)
                _dbEditorView.ShowSoundEditorView();
            else
                throw new NotImplementedException();
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal override void Cleanup()
        {
            _dbEditorView.HideAllViews();

            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Clear();

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = false;
            MainForm.ViewToolStripMenuItem.Enabled = false;

            //Setup the View menu
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapBodyMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPropertiesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPalettesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewTileSetMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewSpriteSetsMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewPropertySetMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewImagesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewToolsMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewDatabaseMenuItem);
        }

        internal override void Setup()
        {
            if (MainForm.VM.DbEditor.CurrentDb == null)
                throw new InvalidOperationException("No current database!");

            MainForm.VM.DbEditor.CurrentDb.PropertyChanged += Database_PropertyChanged;

            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileOpenDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileCloseDatabaseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(ExitToolStripMenuItem);

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = true;
            MainForm.EditToolStripMenuItem.Visible = true;

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.Enabled = true;
            MainForm.ViewToolStripMenuItem.Visible = true;

            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapBodyMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPropertiesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPalettesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewTileSetMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewSpriteSetsMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewPropertySetMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewImagesMenuItem);
            //MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewToolsMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewDatabaseMenuItem);

            MainForm.Text = $"{MainForm.APP_NAME} - {MainForm.VM.DbEditor.CurrentDb.Name}";

            MainForm.SuspendLayout();
            MainForm.Controls.Add(_dbEditorView);
            MainForm.Controls.SetChildIndex(_dbEditorView, 0);
            _dbEditorView.Initialize(MainForm.VM.DbEditor);
            MainForm.ResumeLayout();

            //_projectView.ActiveContentChanged += new EventHandler(ProjectView_ActiveContentChanged);

            _dbEditorView.ShowDatabaseView();

        }

        private void Database_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var database = sender as DatabaseVM;

            switch (e.PropertyName)
            {
                case nameof(database.OpenedItem):
                    OnOpenedItemChanged(database.OpenedItem);
                    break;
                default:
                    break;
            }
        }

    #endregion Internal Methods
}
}
