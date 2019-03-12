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
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Editor.UI.WinForms.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {

        #region Public Fields

        public const string APP_NAME = "Open Breed Map Editor";

        #endregion Public Fields

        #region Internal Fields

        internal ToolStripMenuItem ViewToggleLoggerToolStripMenuItem = new ToolStripMenuItem();

        #endregion Internal Fields

        #region Private Fields

        private LogConsoleView _logConsoleView = null;
        private LoggerView _loggerView;
        private FormState _state;
        private FormStateDatabaseOpened DatabaseOpenedState;
        private FormStateInitial InitialState;

        #endregion Private Fields

        #region Public Constructors

        public MainForm()
        {
            InitializeComponent();

            DatabaseOpenedState = new FormStateDatabaseOpened(this);
            InitialState = new FormStateInitial(this);

            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            State = new FormStateInitial(this);

            ViewToolStripMenuItem.DropDownItems.Add(ViewToggleLoggerToolStripMenuItem);
        }

        #endregion Public Constructors

        #region Public Properties

        public EditorVM VM { get; private set; }

        #endregion Public Properties

        #region Internal Properties

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

        #endregion Internal Properties

        #region Public Methods

        public void Initialize(EditorVM vm)
        {
            VM = vm ?? throw new ArgumentNullException(nameof(vm));

            EditorView.Initialize(VM.DbEditor);

            VM.DbEditor.PropertyChanged += VM_PropertyChanged;

            ViewToggleLoggerToolStripMenuItem.Click += (s, a) => VM.ShowLogger();
            VM.ShowLoggerAction = OnShowLogger;
        }

        #endregion Public Methods

        #region Private Methods

        private void ABTAGamePasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Other.TryAction(OpenABTAPasswordGenerator);
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
            _logConsoleView = new LogConsoleView();
            _logConsoleView.VisibleChanged += (s, a) => ChangeCheckedState(LogConsoleShowToolStripMenuItem, s as Control);
            _logConsoleView.Show(EditorView, DockState.Float);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!VM.TryExit())
                e.Cancel = true;
        }
        private void OnDatabaseChanged()
        {
            if (VM.DbEditor.Editable != null)
                State = DatabaseOpenedState;
            else
                State = InitialState;
        }

        private void OnShowLogger(LoggerVM vm)
        {
            if (_loggerView == null)
            {
                _loggerView = new LoggerView();
                _loggerView.Initialize(vm);
            }
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
            Other.TryAction(OpenOptionsForm);
        }

        private void ShowLogConsoleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ToggleLogConsole(item.Checked);
        }

        private void ToggleLogConsole(bool toogle)
        {
            if (toogle)
            {
                if (_logConsoleView == null)
                    InitLogConsole();
                else
                    _logConsoleView.Show(EditorView);
            }
            else
                _logConsoleView.Hide();
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

        #endregion Private Methods
    }

}
