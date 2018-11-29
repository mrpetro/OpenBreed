using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.UI.WinForms.Views;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Database.Sources;
using System.IO;
using OpenBreed.Editor.UI.WinForms.Helpers;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM.Project;
using OpenBreed.Common;
using OpenBreed.Common.Commands;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public class MainFormLevelState
    {
        public readonly MainForm MainForm;

        private ProjectView _projectView;

        //File menu
        internal ToolStripSeparator FileSeparator1 = null;
        internal ToolStripMenuItem FileSaveToolStripMenuItem = null;
        internal ToolStripMenuItem FileSaveAsToolStripMenuItem = null;
        internal ToolStripMenuItem FileCloseToolStripMenuItem = null;
        internal ToolStripSeparator FileSeparator2 = null;

        //Edit menu
        internal ToolStripMenuItem EditUndoToolStripMenuItem = null;
        internal ToolStripMenuItem EditRedoToolStripMenuItem = null;

        //View menu
        internal ToolStripMenuItem ViewMapBodyMenuItem = null;
        internal ToolStripMenuItem ViewMapPropertiesMenuItem = null;
        internal ToolStripMenuItem ViewMapPalettesMenuItem = null;
        internal ToolStripMenuItem ViewTileSetMenuItem = null;
        internal ToolStripMenuItem ViewPropertySetMenuItem = null;
        internal ToolStripMenuItem ViewSpriteSetsMenuItem = null;
        internal ToolStripMenuItem ViewToolsMenuItem = null;


        private string m_ActiveName;

        public MainFormLevelState(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        void ProjectView_ActiveContentChanged(object sender, EventArgs e)
        {
            DockPanel dockPanel = (DockPanel)sender;

            if (dockPanel.ActiveContent == null)
                return;

            m_ActiveName = dockPanel.ActiveContent.ToString();

            if (FileSaveToolStripMenuItem != null)
                FileSaveToolStripMenuItem.Text = "Save " + m_ActiveName;

            if (FileSaveAsToolStripMenuItem != null)
                FileSaveAsToolStripMenuItem.Text = "Save " + m_ActiveName + " As...";
        }

        public void UnsetLevelEditState(ProjectVM projectPmod)
        {
            //Setup the File menu
            MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSeparator1);
            MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSaveToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSaveAsToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileCloseToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSeparator2);

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = false;

            projectPmod.Root.Map.Commands.CommandsUpdated -= Commands_CommandsUpdated;

            MainForm.EditToolStripMenuItem.DropDownItems.Remove(EditUndoToolStripMenuItem);
            MainForm.EditToolStripMenuItem.DropDownItems.Remove(EditRedoToolStripMenuItem);

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapBodyMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPropertiesMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPalettesMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewTileSetMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewSpriteSetsMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewPropertySetMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewToolsMenuItem);
        }

        private void Commands_CommandsUpdated(object sender, CommandsUpdatedEventArgs e)
        {
            var commandMan = (CommandMan)sender;

            EditUndoToolStripMenuItem.Enabled = commandMan.IsUndoAvailable;
            EditRedoToolStripMenuItem.Enabled = commandMan.IsRedoAvailable;
        }

        public void SetMapEditState(ProjectVM projectPmod)
        {
            if (_projectView != null)
                throw new InvalidOperationException("Previous ProjectView not closed!");

            MainForm.SuspendLayout();
            _projectView = new ProjectView();
            _projectView.Dock = DockStyle.Fill;
            MainForm.Controls.Add(_projectView);
            MainForm.Controls.SetChildIndex(_projectView, 0);
            _projectView.Initialize(projectPmod);
            MainForm.ResumeLayout();

            //Setup the File menu
            FileSeparator1 = new ToolStripSeparator();
            FileSaveToolStripMenuItem = new ToolStripMenuItem("Save " + projectPmod.Root.Map.Title);
            FileSaveToolStripMenuItem.Click += (s, a) => Tools.TryAction(SaveMap);
            FileSaveAsToolStripMenuItem = new ToolStripMenuItem("Save " + projectPmod.Root.Map.Title + " As...");
            FileSaveAsToolStripMenuItem.Click += (s, a) => Tools.TryAction(SaveMapAs);
            FileSeparator2 = new ToolStripSeparator();
            FileCloseToolStripMenuItem = new ToolStripMenuItem("Close");
            FileCloseToolStripMenuItem.Click += (s, a) => Tools.TryAction(CloseProject);

            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator1);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSaveToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSaveAsToolStripMenuItem);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator2);
            MainForm.FileToolStripMenuItem.DropDownItems.Add(FileCloseToolStripMenuItem);

            //Setup the Edit menu
            MainForm.EditToolStripMenuItem.Enabled = true;
            MainForm.EditToolStripMenuItem.Visible = true;

            EditUndoToolStripMenuItem = new ToolStripMenuItem("Undo");
            EditUndoToolStripMenuItem.ShowShortcutKeys = true;
            EditUndoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            EditUndoToolStripMenuItem.Click += (s, a) => projectPmod.Root.Map.Commands.Undo();

            EditRedoToolStripMenuItem = new ToolStripMenuItem("Redo");
            EditRedoToolStripMenuItem.ShowShortcutKeys = true;
            EditRedoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            EditRedoToolStripMenuItem.Click += (s, a) => projectPmod.Root.Map.Commands.Redo();

            EditUndoToolStripMenuItem.Enabled = projectPmod.Root.Map.Commands.IsUndoAvailable;
            EditRedoToolStripMenuItem.Enabled = projectPmod.Root.Map.Commands.IsRedoAvailable;

            projectPmod.Root.Map.Commands.CommandsUpdated += Commands_CommandsUpdated;

            MainForm.EditToolStripMenuItem.DropDownItems.Add(EditUndoToolStripMenuItem);
            MainForm.EditToolStripMenuItem.DropDownItems.Add(EditRedoToolStripMenuItem);

            //Setup the View menu
            MainForm.ViewToolStripMenuItem.Enabled = true;
            MainForm.ViewToolStripMenuItem.Visible = true;

            ViewMapBodyMenuItem = new ToolStripMenuItem("Map body");
            ViewMapBodyMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapBody);

            ViewMapPropertiesMenuItem = new ToolStripMenuItem("Map properties");
            ViewMapPropertiesMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapProperties);

            ViewMapPalettesMenuItem = new ToolStripMenuItem("Palettes");
            ViewMapPalettesMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.MapPalettes);

            ViewTileSetMenuItem = new ToolStripMenuItem("Tile set");
            ViewTileSetMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.TileSet);

            ViewSpriteSetsMenuItem = new ToolStripMenuItem("Sprite sets");
            ViewSpriteSetsMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.SpriteSets);

            ViewPropertySetMenuItem = new ToolStripMenuItem("Property set");
            ViewPropertySetMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.PropertySet);

            ViewToolsMenuItem = new ToolStripMenuItem("Tools");
            ViewToolsMenuItem.Click += (s, a) => _projectView.ShowView(ProjectViewType.Tools);

            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapBodyMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPropertiesMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPalettesMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewTileSetMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewSpriteSetsMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewPropertySetMenuItem);
            MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewToolsMenuItem);

            MainForm.Text = $"{MainForm.APP_NAME} - {projectPmod.CurrentLevel.Name}";

            MainForm.FormClosing += MainForm_FormClosing;

            _projectView.ActiveContentChanged += new EventHandler(ProjectView_ActiveContentChanged);

            _projectView.InitViews();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _projectView.StoreLayout();
        }

        public void SaveMap()
        {
            MainForm.VM.Project.Root.Map.Save();
        }

        public void SaveMapAs()
        {
            string proposedFileName = MainForm.VM.Project.Root.Map.Title.Replace("*", "");
            SaveFileDialogHelper.PrepareToSaveMAPFile(MainForm.SaveFileDialog, proposedFileName);

            DialogResult result = MainForm.SaveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = MainForm.SaveFileDialog.FileName;

                var sourceDef = new DirectoryFileSourceDef();
                sourceDef.DirectoryPath = Path.GetDirectoryName(filePath);
                sourceDef.Name = Path.GetFileName(filePath);
                sourceDef.Type = "ABTAMAP";


                var newSource = MainForm.VM.Sources.Create(sourceDef);
                MainForm.VM.Project.Root.Map.SaveAs(newSource);
            }
        }

        public void CloseProject()
        {
            MainForm.VM.Project.TryClose();
            UnsetLevelEditState(MainForm.VM.Project);
            _projectView.DeinitViews();
        }
    }
}
