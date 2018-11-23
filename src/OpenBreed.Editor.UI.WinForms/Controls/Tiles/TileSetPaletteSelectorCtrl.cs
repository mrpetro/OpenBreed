using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    public partial class TileSetPaletteSelectorCtrl : UserControl
    {
        #region Private Fields

        private TileSetPaletteSelectorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public TileSetPaletteSelectorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(TileSetPaletteSelectorVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            //cbxTileSetPalettes.SelectedIndexChanged += (s, a) => _vm.CurrentItem = _vm.Items.FirstOrDefault(item => item == cbxSpriteSets.SelectedItem);
            //cbxTileSetPalettes.DataSource = _vm.Items;
            //cbxTileSetPalettes.DisplayMember = "Name";
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //switch (e.PropertyName)
            //{
            //    case (nameof(_vm.CurrentItem)):
            //        //SpriteSetCtrl.Initialize(_vm.CurrentItem);
            //        break;
            //    default:
            //        break;
            //}
        }

        void UpdateSpriteSets()
        {
            //if (_vm.Items.Count == 0)
            //    SetNoSpriteSetsState();
            //else
            //    SetSpriteSetsState();
        }

        private void SetNoSpriteSetsState()
        {
            //TabText = "No sprite sets";
        }

        private void SetSpriteSetsState()
        {
            //TabText = "Sprite sets";


        }

        #endregion Public Methods
    }
}
