using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Sprites;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class SpriteSetsView : DockContent
    {
        private SpriteSetsVM _vm;

        public SpriteSetsView()
        {
            InitializeComponent();

        }

        public void Initialize(SpriteSetsVM vm)
        {
            _vm = vm;

            SpriteSets.Initialize(vm);
            SpriteViewer.Initialize(vm.SpriteViewer);

            _vm.PropertyChanged += _vm_PropertyChanged;

            //TabText = _vm.Title;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(_vm.Title):
                //    TabText = _vm.Title;
                //    break;
                default:
                    break;
            }
        }
    }
}
