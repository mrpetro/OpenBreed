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
    public partial class SpriteSetsCtrl : UserControl
    {
        private SpriteSetsVM _vm;

        public SpriteSetsCtrl()
        {
            InitializeComponent();

        }

        public void Initialize(SpriteSetsVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            cbxSpriteSets.SelectedIndexChanged += (s, a) => _vm.CurrentItem = _vm.Items.FirstOrDefault(item => item == cbxSpriteSets.SelectedItem);
            cbxSpriteSets.DataSource = _vm.Items;
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
            if (_vm.Items.Count == 0)
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
