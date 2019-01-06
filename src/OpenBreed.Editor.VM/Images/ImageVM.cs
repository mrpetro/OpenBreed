using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageVM : EditableEntryVM
    {
        #region Private Fields

        private Image _image;
        private int _height;
        private int _width;

        public ImageVM()
        {
            PropertyChanged += ImageVM_PropertyChanged;
        }

        private void ImageVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Image):
                    Width = Image.Width;
                    Height = Image.Height;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Fields

        #region Public Properties

        public int Height
        {
            get { return _height; }
            private set { SetProperty(ref _height, value); }
        }

        public int Width
        {
            get { return _width; }
            private set { SetProperty(ref _width, value); }
        }

        public Image Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        #endregion Public Properties
    }
}
