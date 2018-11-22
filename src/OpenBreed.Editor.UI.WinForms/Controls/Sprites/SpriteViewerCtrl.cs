using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Sprites;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteViewerCtrl : UserControl
    {
        private SpriteViewerVM _vm;

        public SpriteViewerCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);


        }


        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case (nameof(_vm.CurrentSpriteSet)):
                    UpdateControl();
                    break;
                case (nameof(_vm.CurrentItem)):
                    pnlSprite.Invalidate();
                    break;
                default:
                    break;
            }
        }

        public void Initialize(SpriteViewerVM vm)
        {
            _vm = vm;

            numSpriteNo.DataBindings.Clear();
            numSpriteNo.DataBindings.Add(nameof(numSpriteNo.Value), _vm, nameof(_vm.CurrentIndex), false, DataSourceUpdateMode.OnPropertyChanged);

            _vm.PropertyChanged += _vm_PropertyChanged;
            pnlSprite.Paint += PnlSprite_Paint;

            UpdateControl();
        }

        private void PnlSprite_Paint(object sender, PaintEventArgs e)
        {
            if(_vm.CurrentItem != null)
                _vm.CurrentItem.Draw(e.Graphics, 0, 0, 2);
        }

        void UpdateControl()
        {
            if (_vm.CurrentSpriteSet == null)
                SetNoSpritesState();
            else
                SetSpritesState();

            Invalidate();
            pnlSprite.Invalidate();
        }

        private void SetNoSpritesState()
        {

        }

        private void SetSpritesState()
        {
            numSpriteNo.Minimum = 0;
            numSpriteNo.Maximum = _vm.CurrentSpriteSet.Items.Count - 1;
            numSpriteNo.Value = 0;
        }
    }
}
