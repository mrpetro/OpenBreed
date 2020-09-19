using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sounds;
using System;
using System.ComponentModel;
using System.Windows.Forms;

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

        private void OnSubeditorChanged(IEntryEditor<ISoundEntry> subeditor)
        {
            Panel.Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is SoundFromPcmEditorVM)
            {
                var control = new SoundFromPcmEditorCtrl();
                control.Initialize((SoundFromPcmEditorVM)vm.Subeditor);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
        }

        #endregion Private Methods
    }
}