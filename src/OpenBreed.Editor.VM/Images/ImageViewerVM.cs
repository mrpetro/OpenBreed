using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageViewerVM : BaseViewModel
    {

        #region Private Fields

        private Image _image;
        private SourceBase _source;

        #endregion Private Fields

        #region Public Constructors

        public ImageViewerVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Public Constructors

        #region Public Properties

        public Image Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public EditorVM Root { get; private set; }

        public SourceBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            if (Image == null)
                return;

            int width = Image.Width * factor;
            int height = Image.Height * factor;

            gfx.DrawImage(Image, (int)x, (int)y, width, height);
        }

        public void TryClose()
        {
            if (Source != null)
            {
                Source.Dispose();
                Source = null;

                Image.Dispose();
                Image = null;
            }
        }

        public void TryLoad(string name)
        {
            Load(name);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Load(string name)
        {
            Image = Root.DataProvider.GetImage(name);
        }

        #endregion Internal Methods

    }
}
