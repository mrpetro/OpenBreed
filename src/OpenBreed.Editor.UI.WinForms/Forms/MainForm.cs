using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenBreed.Editor.VM;

using OpenBreed.Common;
using OpenBreed.Editor.UI.WinForms.Forms.States;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private FormStateDatabaseOpened DatabaseOpenedState;
        private FormStateInitial InitialState;

        #region Public Fields

        public const string APP_NAME = "Open Breed Map Editor";

        private FormState _state;

        internal FormState State
        {
            get { return _state; }
            set
            {
                if (_state != null)
                    _state.Cleanup();

                _state = value;
                _state.Setup();
            }
        }

        #endregion Public Fields

        //private LogConsoleView m_LogConsoleView = null;

        #region Public Constructors

        public MainForm()
        {
            InitializeComponent();

            DatabaseOpenedState = new FormStateDatabaseOpened(this);
            InitialState = new FormStateInitial(this);

            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            State = new FormStateInitial(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!VM.TryExit())
                e.Cancel = true;
        }

        #endregion Public Constructors

        #region Public Properties

        public EditorVM VM { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Initialize(EditorVM vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            VM = vm;

            VM.DbEditor.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VM.DbEditor.Editable):
                    OnDatabaseChanged();
                    break;
                default:
                    break;
            }
        }

        private void OnDatabaseChanged()
        {
            if (VM.DbEditor.Editable != null)
                State = DatabaseOpenedState;
            else
                State = InitialState;
        }

        #endregion Public Methods

        #region Private Methods

        private void ABTAGamePasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TryAction(OpenABTAPasswordGenerator);
        }

        private void ABTAGameRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM.TryRunABTAGame();
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
            using (OptionsForm optionsForm = new OptionsForm(VM.Settings))
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
