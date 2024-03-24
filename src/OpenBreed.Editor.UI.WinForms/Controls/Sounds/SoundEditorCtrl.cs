using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sounds;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sounds
{
    public partial class SoundEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private SoundEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SoundEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            this.vm = vm as SoundEditorVM ?? throw new InvalidOperationException(nameof(vm));

            OnSubeditorChanged(this.vm.Subeditor);

            this.vm.PropertyChanged += _vm_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(vm.Subeditor):
                    OnSubeditorChanged(vm.Subeditor);
                    break;

                default:
                    break;
            }
        }

        private void OnSubeditorChanged(IEntryEditor<IDbSound> subeditor)
        {
            Panel.Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is SoundFromPcmEditorVM)
            {

                var ctrlHost = new ElementHost();
                ctrlHost.Dock = DockStyle.Fill;

                var control = new Wpf.Sounds.SoundFromPcmEditorCtrl();
                control.DataContext = (SoundFromPcmEditorVM)vm.Subeditor;
                ctrlHost.Child = control;
                //control.Initialize((SoundFromPcmEditorVM)vm.Subeditor);
                Panel.Controls.Add(ctrlHost);
            }
        }

        #endregion Private Methods
    }
}