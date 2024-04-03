using OpenBreed.Editor.UI.WinForms.Controls;
using OpenBreed.Editor.UI.WinForms.Helpers;
using OpenBreed.Editor.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class EntryEditorViewWpf : DockContent
    {
        #region Public Constructors

        public EntryEditorViewWpf()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Protected Properties

        protected EntryEditorVM VM { get; private set; }

        #endregion Protected Properties

        #region Public Methods

        public virtual void Initialize(EntryEditorVM vm)
        {
            VM = vm ?? throw new ArgumentNullException(nameof(vm));

            VM.ActivatingAction = () => this.InvokeIfRequired(() => base.Activate());
            VM.ClosingAction = () => this.InvokeIfRequired(() => base.Close());

            DataBindings.Add(nameof(Text), VM, nameof(VM.Title), false, DataSourceUpdateMode.OnPropertyChanged);

            var entryEditor = WpfHelper.CreateWpfControl<Wpf.EntryEditorCtrl>(VM);
            entryEditor.Dock = DockStyle.Fill;
            Controls.Add(entryEditor);

        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            VM.ClosedAction();
        }

        #endregion Protected Methods
    }
}