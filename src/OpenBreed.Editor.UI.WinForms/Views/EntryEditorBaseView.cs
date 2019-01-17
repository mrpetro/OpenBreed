using OpenBreed.Editor.UI.WinForms.Controls;
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
    public partial class EntryEditorBaseView : DockContent
    {

        #region Public Constructors

        public EntryEditorBaseView()
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

            VM.ClosingAction = () => this.InvokeIfRequired(() => base.Close());

            DataBindings.Add(nameof(Text), VM, nameof(VM.Title), false, DataSourceUpdateMode.OnPropertyChanged);

            VM.PropertyChanged += VM_PropertyChanged;

            EntryEditor.Initialize(VM);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            //VM.ClosingAction();
            //if (e.CloseReason == CloseReason.UserClosing)
            //    e.Cancel = !VM.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            VM.ClosedAction();
        }

        #endregion Protected Methods

        #region Private Methods

        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
