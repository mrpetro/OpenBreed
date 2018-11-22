using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using OpenABEd.Modules.MapEditor;
using OpenABEd.Modules;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.UI.WinForms.Forms;
using System.Diagnostics;
using OpenBreed.Editor.VM.Project;
using OpenBreed.Common;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {

        #region Public Fields

        public const string APP_NAME = "Alien Breed Map Editor";

        #endregion Public Fields

        #region Private Fields

        private readonly MainFormMapEditHandler m_LevelEditHandler;
        private readonly MainFormGeneralHandler m_MapsHandler;
        //private LogConsoleView m_LogConsoleView = null;

        #endregion Private Fields

        #region Public Constructors

        public MainForm()
        {
            InitializeComponent();

            m_MapsHandler = new MainFormGeneralHandler(this);
            m_LevelEditHandler = new MainFormMapEditHandler(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public EditorVM _vm { get; private set; }
        public MainFormGeneralHandler GeneralHandler { get { return m_MapsHandler; } }
        public MainFormMapEditHandler LevelEditHandler { get { return m_LevelEditHandler; } }

        #endregion Public Properties

        #region Public Methods

        public void Initialize(EditorVM vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;
        }

        #endregion Public Methods

        #region Private Methods

        private void ABTAGamePasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TryAction(OpenABTAPasswordGenerator);
        }

        private void ABTAGameRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _vm.TryRunABTAGame();
        }

        void ChangeCheckedState(ToolStripMenuItem menuItem, Control ctrl)
        {
            if (menuItem.Checked != ctrl.Visible)
                menuItem.Checked = ctrl.Visible;
        }

        private void InitLogConsole()
        {
            //m_LogConsoleView = new LogConsoleView();
            //m_LogConsoleView.VisibleChanged += (s, a) => ChangeCheckedState(LogConsoleShowToolStripMenuItem, s as Control);
            //m_LogConsoleView.Show(DockPanel, DockState.Float);
        }

        private void OpenABTAPasswordGenerator()
        {
            ABTAPasswordGeneratorForm abtaPasswordGeneratorForm = new ABTAPasswordGeneratorForm();

            DialogResult result = abtaPasswordGeneratorForm.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
            }
        }

        private void OpenOptionsForm()
        {
            using (OptionsForm optionsForm = new OptionsForm(_vm.Settings))
            {
                DialogResult result = optionsForm.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    optionsForm.UpdateSettings();
            }
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TryAction(OpenOptionsForm);
        }

        private void ShowLogConsoleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ToggleLogConsole(item.Checked);
        }

        private void ToggleLogConsole(bool toogle)
        {
            //if (toogle)
            //{
            //    if (m_LogConsoleView == null)
            //        InitLogConsole();
            //    else
            //        m_LogConsoleView.Show(DockPanel);
            //}
            //else
            //    m_LogConsoleView.Hide();
        }

        #endregion Private Methods

    }

}
