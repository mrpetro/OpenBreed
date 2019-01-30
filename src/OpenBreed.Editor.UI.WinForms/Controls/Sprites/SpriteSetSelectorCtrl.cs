using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sprites;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetSelectorCtrl : UserControl
    {
        private SpriteSetSelectorVM _vm;

        public SpriteSetSelectorCtrl()
        {
            InitializeComponent();

        }

        public void Initialize(SpriteSetSelectorVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            cbxSpriteSets.DataBindings.Add(nameof(cbxSpriteSets.SelectedIndex), _vm, nameof(_vm.CurrentIndex), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxSpriteSets.DataSource = _vm.Parent.Editable.SpriteSets;
            cbxSpriteSets.DisplayMember = "Name";

            UpdateSpriteSets();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case (nameof(_vm.CurrentItem)):
                    //SpriteSetCtrl.Initialize(_vm.CurrentItem);
                    break;
                default:
                    break;
            }
        }

        void UpdateSpriteSets()
        {
            if (_vm.Parent.Editable.SpriteSets.Count == 0)
                SetNoSpriteSetsState();
            else
                SetSpriteSetsState();
        }

        private void SetNoSpriteSetsState()
        {
           //TabText = "No sprite sets";
        }

        private void SetSpriteSetsState()
        {
            //TabText = "Sprite sets";


        }
    }
}
