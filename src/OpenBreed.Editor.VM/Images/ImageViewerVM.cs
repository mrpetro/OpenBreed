using OpenBreed.Common.Database.Items.Images;
using OpenBreed.Common.Database.Items.Sources;
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

        public void TryLoad(ImageDef imageDef)
        {
            var imageSourceDef = Root.Database.GetSourceDef(imageDef.SourceRef);
            if (imageSourceDef == null)
                throw new Exception("No ImageSourceDef definition found!");

            var source = Root.SourceMan.GetSource(imageSourceDef);

            if (source == null)
                throw new Exception("Image source error: " + imageDef.SourceRef);

            Load(imageSourceDef);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Load(SourceDef sourceDef)
        {
            if (Source != null)
            {
                Source.Dispose();
                Source = null;
            }

            var source = Root.SourceMan.GetSource(sourceDef);

            var format = Root.FormatMan.GetFormatMan(sourceDef.Type);
            if (format == null)
                throw new Exception($"Unknown format {sourceDef.Type}");

            Image = source.Load(format) as Image;
            Source = source;
        }

        #endregion Internal Methods

    }
}
