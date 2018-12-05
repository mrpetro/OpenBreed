using OpenBreed.Common.Database.Sources;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Sources;
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
        private BaseSource _source;

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

        public BaseSource Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            int width = Image.Width * factor;
            int height = Image.Height * factor;

            gfx.DrawImage(Image, (int)x, (int)y, width, height);
        }

        public void TryLoad(string imageSourceRef)
        {
            var imageSourceDef = Root.Database.GetSourceDef(imageSourceRef);
            if (imageSourceDef == null)
                throw new Exception("No ImageSourceDef definition found!");

            var source = Root.Sources.GetSource(imageSourceDef);

            if (source == null)
                throw new Exception("Image source error: " + imageSourceRef);

            Load(imageSourceDef);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Load(SourceDef sourceDef)
        {
            if (Source != null)
                throw new InvalidOperationException("Other map already loaded!");

            var source = Root.Sources.GetSource(sourceDef);

            Image = source.Load() as Image;
            Source = source;
        }

        #endregion Internal Methods

    }
}
