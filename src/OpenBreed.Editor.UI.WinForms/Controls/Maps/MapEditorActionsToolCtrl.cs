using OpenBreed.Editor.VM.Maps;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorActionsToolCtrl : UserControl
    {
        #region Private Fields

        private MapEditorActionsToolVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorActionsToolVM vm)
        {
            _vm = vm;

            EntryRef.Initialize(vm.RefIdEditor);
            ActionsSelector.Initialize(vm.ActionsSelector);
        }

        #endregion Public Methods
    }
}