using OpenBreed.Editor.VM.Sprites;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteFromImageEditorCtrl : UserControl
    {
        #region Private Fields

        private SpriteFromImageEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SpriteFromImageEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SpriteFromImageEditorVM vm)
        {
            this.vm = vm;

            btnUpdate.DataBindings.Add(nameof(btnUpdate.Enabled), vm, nameof(vm.UpdateEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            btnUndo.DataBindings.Add(nameof(btnUndo.Enabled), vm, nameof(vm.UndoEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            lblSpriteCoords.DataBindings.Add(nameof(lblSpriteCoords.Text), vm, nameof(vm.SpriteRectangleText), false, DataSourceUpdateMode.OnPropertyChanged);

            btnUpdate.Click += (s, a) => vm.Update();
            btnUndo.Click += (s, a) => vm.Undo();

            ImageView.Initialize(vm);
        }

        #endregion Public Methods
    }
}