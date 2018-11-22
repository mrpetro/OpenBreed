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
    public class MainFormMapEditHandler
    {
        private readonly MainForm m_MainForm;

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

        public MainFormMapEditHandler(MainForm mainForm)
        {
            m_MainForm = mainForm;
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
            m_MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSeparator1);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSaveToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSaveAsToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileCloseToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Remove(FileSeparator2);

            //Setup the Edit menu
            m_MainForm.EditToolStripMenuItem.Enabled = false;

            projectPmod.Root.Map.Commands.CommandsUpdated -= Commands_CommandsUpdated;

            m_MainForm.EditToolStripMenuItem.DropDownItems.Remove(EditUndoToolStripMenuItem);
            m_MainForm.EditToolStripMenuItem.DropDownItems.Remove(EditRedoToolStripMenuItem);

            //Setup the View menu
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapBodyMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPropertiesMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewMapPalettesMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewTileSetMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewSpriteSetsMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewPropertySetMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Remove(ViewToolsMenuItem);
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


            m_MainForm.SuspendLayout();
            _projectView = new ProjectView();
            _projectView.Initialize(projectPmod);
            _projectView.Dock = DockStyle.Fill;
            m_MainForm.Controls.Add(_projectView);
            m_MainForm.Controls.SetChildIndex(_projectView, 0);
            m_MainForm.ResumeLayout();

            //Setup the File menu
            FileSeparator1 = new ToolStripSeparator();
            FileSaveToolStripMenuItem = new ToolStripMenuItem("Save " + projectPmod.Root.Map.Title);
            FileSaveToolStripMenuItem.Click += (s, a) => Tools.TryAction(SaveMap);
            FileSaveAsToolStripMenuItem = new ToolStripMenuItem("Save " + projectPmod.Root.Map.Title + " As...");
            FileSaveAsToolStripMenuItem.Click += (s, a) => Tools.TryAction(SaveMapAs);
            FileSeparator2 = new ToolStripSeparator();
            FileCloseToolStripMenuItem = new ToolStripMenuItem("Close");
            FileCloseToolStripMenuItem.Click += (s, a) => Tools.TryAction(CloseProject);

            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator1);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSaveToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSaveAsToolStripMenuItem);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(FileSeparator2);
            m_MainForm.FileToolStripMenuItem.DropDownItems.Add(FileCloseToolStripMenuItem);

            //Setup the Edit menu
            m_MainForm.EditToolStripMenuItem.Enabled = true;
            m_MainForm.EditToolStripMenuItem.Visible = true;

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

            m_MainForm.EditToolStripMenuItem.DropDownItems.Add(EditUndoToolStripMenuItem);
            m_MainForm.EditToolStripMenuItem.DropDownItems.Add(EditRedoToolStripMenuItem);

            //Setup the View menu
            m_MainForm.ViewToolStripMenuItem.Enabled = true;
            m_MainForm.ViewToolStripMenuItem.Visible = true;

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

            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapBodyMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPropertiesMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewMapPalettesMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewTileSetMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewSpriteSetsMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewPropertySetMenuItem);
            m_MainForm.ViewToolStripMenuItem.DropDownItems.Add(ViewToolsMenuItem);

            m_MainForm.Text = $"{MainForm.APP_NAME} - {projectPmod.CurrentProject.Name}";

            m_MainForm.FormClosing += MainForm_FormClosing;

            _projectView.ActiveContentChanged += new EventHandler(ProjectView_ActiveContentChanged);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _projectView.StoreLayout();
        }

        public void SaveMap()
        {
            m_MainForm._vm.Project.Root.Map.Save();
        }

        public void SaveMapAs()
        {
            string proposedFileName = m_MainForm._vm.Project.Root.Map.Title.Replace("*", "");
            SaveFileDialogHelper.PrepareToSaveMAPFile(m_MainForm.SaveFileDialog, proposedFileName);

            DialogResult result = m_MainForm.SaveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = m_MainForm.SaveFileDialog.FileName;

                var sourceDef = new DirectoryFileSourceDef();
                sourceDef.DirectoryPath = Path.GetDirectoryName(filePath);
                sourceDef.Name = Path.GetFileName(filePath);
                sourceDef.Type = "ABTAMAP";


                var newSource = m_MainForm._vm.Sources.Create(sourceDef);
                m_MainForm._vm.Project.Root.Map.SaveAs(newSource);
            }
        }

        public void CloseProject()
        {
            UnsetLevelEditState(m_MainForm._vm.Project);
            _projectView.DeinitViews();
        }

        internal void OpenProject()
        {
            if (m_MainForm._vm.CurrentDatabase == null)
                m_MainForm._vm.OpenABTADatabase();

            string resourcesDir = Path.Combine(ProgramTools.AppDir, "Resources", "ABTA", "Maps");
            OpenFileDialogHelper.PrepareToOpenABEdProjFile(m_MainForm.OpenFileDialog, resourcesDir);

            DialogResult result = m_MainForm.OpenFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = m_MainForm.OpenFileDialog.FileName;

                var sourceDef = new DirectoryFileSourceDef();
                sourceDef.DirectoryPath = Path.GetDirectoryName(filePath);
                sourceDef.Name = Path.GetFileName(filePath);
                sourceDef.Type = "LevelXML";

                m_MainForm._vm.Project.Load(sourceDef);

                SetMapEditState(m_MainForm._vm.Project);
                _projectView.InitViews();
            }
        }
    }
}
