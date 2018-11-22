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
        }

        #endregion Public Methods
    }
}
