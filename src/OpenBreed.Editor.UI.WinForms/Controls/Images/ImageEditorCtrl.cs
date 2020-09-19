using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Images;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    public partial class ImageEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private ImageEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public ImageEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            this.vm = vm as ImageEditorVM ?? throw new InvalidOperationException(nameof(vm));

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

        private void OnSubeditorChanged(IEntryEditor<IImageEntry> subeditor)
        {
            Panel.Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is ImageFromFileEditorVM)
            {
                var control = new ImageFromFileEditorCtrl();
                control.Initialize((ImageFromFileEditorVM)vm.Subeditor);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
        }

        #endregion Private Methods
    }
}