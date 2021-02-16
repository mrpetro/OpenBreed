using OpenBreed.Common.Tools;
using OpenBreed.Common.UI.WinForms.Controls;
using OpenBreed.Editor.UI.WinForms.Forms.States;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Logging;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {
        #region Public Fields

        public DbEditorView EditorView = new DbEditorView();

        #endregion Public Fields

        #region Internal Fields

        internal ToolStripMenuItem ViewToggleLoggerToolStripMenuItem = new ToolStripMenuItem();
        internal ToolStripMenuItem ABTAGamePasswordsToolStripMenuItem = new ToolStripMenuItem();
        internal ToolStripMenuItem ABTAGameRunToolStripMenuItem = new ToolStripMenuItem();
        internal ToolStripMenuItemEx LogConsoleShowToolStripMenuItem = new ToolStripMenuItemEx("Log Console");

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

            EditorView.Dock = System.Windows.Forms.DockStyle.Fill;
            EditorView.Name = "EditorView";
            Controls.Add(EditorView);
            EditorView.BringToFront();

            DatabaseOpenedState = new FormStateDatabaseOpened(this);
            InitialState = new FormStateInitial(this);

            State = new FormStateInitial(this);

            ViewToolStripMenuItem.DropDownItems.Add(ViewToggleLoggerToolStripMenuItem);

            ABTAGamePasswordsToolStripMenuItem.Name = "ABTAGamePasswordsToolStripMenuItem";
            ABTAGamePasswordsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            ABTAGamePasswordsToolStripMenuItem.Text = "ABTA Game passwords...";
            toolsToolStripMenuItem.DropDownItems.Add(ABTAGamePasswordsToolStripMenuItem);

            ABTAGameRunToolStripMenuItem.Name = "ABTAGameRunToolStripMenuItem";
            ABTAGameRunToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            ABTAGameRunToolStripMenuItem.Text = "Run ABTA Game";
            toolsToolStripMenuItem.DropDownItems.Add(ABTAGameRunToolStripMenuItem);

            LogConsoleShowToolStripMenuItem.CheckOnClick = true;
            LogConsoleShowToolStripMenuItem.Name = "LogConsoleShowToolStripMenuItem";
            LogConsoleShowToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            LogConsoleShowToolStripMenuItem.Text = "Log Console";
            toolsToolStripMenuItem.DropDownItems.Add(LogConsoleShowToolStripMenuItem);
            LogConsoleShowToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ShowLogConsoleToolStripMenuItem_CheckedChanged);
        }

        #endregion Public Constructors

        #region Public Properties

        public EditorApplicationVM VM { get; private set; }

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

        public void Initialize(EditorApplicationVM vm)
        {
            VM = vm ?? throw new ArgumentNullException(nameof(vm));

            EditorView.Initialize(VM.CreateDbEditorVM());

            BindProperties();
            BindEvents();
            BindActions();
        }

        #endregion Public Methods

        #region Private Methods

        private void BindActions()
        {
            VM.ToggleLoggerAction = OnToggleLogger;
            VM.ShowOptionsAction = OnShowOptions;
            VM.ExitAction = Close;
        }

        private void BindEvents()
        {
            VM.PropertyChanged += VM_PropertyChanged;
            ViewToggleLoggerToolStripMenuItem.Click += (s, a) => VM.ToggleLogger(true);
            OptionsToolStripMenuItem.Click += (s, a) => VM.ShowOptions();
            FormClosing += (s, a) => a.Cancel = !VM.TrySaveBeforeExiting(EditorView.VM);

            ABTAGamePasswordsToolStripMenuItem.Click += (s, a) => Other.TryAction(OpenABTAPasswordGenerator);
            ABTAGameRunToolStripMenuItem.Click += (s, a) => VM.TryRunABTAGame();
        }

        private void BindProperties()
        {
            DataBindings.Add(nameof(Text), VM, nameof(VM.Title), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ChangeCheckedState(ToolStripMenuItem menuItem, Control ctrl)
        {
            if (menuItem.Checked != ctrl.Visible)
                menuItem.Checked = ctrl.Visible;
        }

        private void InitLogConsole()
        {
            _logConsoleView = new LogConsoleView();
            _logConsoleView.VisibleChanged += (s, a) => ChangeCheckedState(LogConsoleShowToolStripMenuItem, s as Control);
            _logConsoleView.Initialize(VM.application.CreateLoggerVm());
            _logConsoleView.Show(EditorView, DockState.Float);
        }

        private void OnDatabaseChanged()
        {
            if (VM.DbName != null)
                State = DatabaseOpenedState;
            else
                State = InitialState;
        }

        private void OnToggleLogger(LoggerVM vm, bool toggle)
        {
            if (toggle)
            {
                if (_loggerView == null)
                {
                    _loggerView = new LoggerView();

                    _loggerView.Initialize(vm);
                    _loggerView.VisibleChanged += (s, a) => ChangeCheckedState(LogConsoleShowToolStripMenuItem, s as Control);
                    _loggerView.Show(EditorView, DockState.Float);
                }
                else
                    _loggerView.Show(EditorView);
            }
            else
                _loggerView.Hide();
        }

        private void OnShowOptions(SettingsMan settings)
        {
            using (OptionsForm optionsForm = new OptionsForm(settings))
            {
                DialogResult result = optionsForm.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    optionsForm.UpdateSettings();
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

        private void ShowLogConsoleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //ToolStripMenuItem item = sender as ToolStripMenuItem;
            //VM.ToggleLogger(item.Checked);

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ToggleLogConsole(item.Checked);
        }

        private void ToggleLogConsole(bool toogle)
        {
            if (toogle)
            {
                if (_logConsoleView == null)
                {
                    InitLogConsole();
                }
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
                case nameof(VM.DbName):
                    OnDatabaseChanged();
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}