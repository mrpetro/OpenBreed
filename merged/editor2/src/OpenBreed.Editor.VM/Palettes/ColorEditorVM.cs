using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class ColorEditorVM : BaseViewModel
    {
        #region Private Fields

        private int _index = 0;
        private MyColor _color = MyColor.Empty;
        private MyColor _colorNegative = MyColor.Empty;
        private byte r;
        private byte g;
        private byte b;
        private readonly IList<ColorSelectionVM> colors;

        #endregion Private Fields

        #region Public Constructors

        public ColorEditorVM(IList<ColorSelectionVM> colors)
        {
            this.colors = colors;
        }

        #endregion Public Constructors

        #region Public Properties

        public MyColor Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public MyColor ColorNegative
        {
            get { return _colorNegative; }
            set { SetProperty(ref _colorNegative, value); }
        }

        public byte R
        {
            get { return r; }
            set { SetProperty(ref r, value); }
        }

        public byte G
        {
            get { return g; }
            set { SetProperty(ref g, value); }
        }

        public byte B
        {
            get { return b; }
            set { SetProperty(ref b, value); }
        }

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Edit(int index)
        {
            Index = index;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        { 
            switch (name)
            {
                case nameof(Index):
                    var color = colors[Index].Color;
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    break;
                case nameof(R):
                case nameof(G):
                case nameof(B):
                    Color = MyColor.FromArgb(R, G, B);

                    //TODO:
                    //ColorNegative = Color.FromArgb(Color.ToArgb() ^ 0xffffff);
                    colors[Index].Color = Color;
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods
    }
}
