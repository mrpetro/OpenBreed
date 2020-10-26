using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorTilesToolCtrl : UserControl
    {
        #region Private Fields

        private MapEditorTilesToolVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesToolCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorTilesToolVM vm)
        {
            _vm = vm ?? throw new InvalidOperationException(nameof(vm));

            EntryRef.Initialize(vm.RefIdEditor);
            TilesSelector.Initialize(_vm.TilesSelector);

        }

        #endregion Public Methods
    }
}
