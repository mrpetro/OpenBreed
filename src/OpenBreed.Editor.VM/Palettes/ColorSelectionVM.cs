using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Palettes
{
    public class ColorSelectionVM : BaseViewModel
    {
        #region Private Fields

        private Color _color = Color.Empty;

        #endregion Private Fields

        #region Public Constructors

        public ColorSelectionVM(Color color, Action<ColorSelectionVM> selectColorCallback)
        {
            Color = color;
            SelectCommand = new Command(() => selectColorCallback.Invoke(this));
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public ICommand SelectCommand { get; }

        #endregion Public Properties
    }
}
