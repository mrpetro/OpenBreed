using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : EntryEditorBaseVM<IImageEntry, ImageVM>
    {
        #region Public Constructors

        public ImageEditorVM(IRepository repository) : base(repository)
        {
        }

        public override string EditorName { get { return "Image Editor"; } }

        #endregion Public Constructors

        #region Public Properties


        #endregion Public Properties

        #region Public Methods

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            if (Editable == null)
                return;

            int width = Editable.Width * factor;
            int height = Editable.Height * factor;

            gfx.DrawImage(Editable.Image, (int)x, (int)y, width, height);
        }

        protected override void UpdateEntry(ImageVM source, IImageEntry target)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateVM(IImageEntry source, ImageVM target)
        {
            target.Image = DataProvider.GetImage(source.Name);
            target.Name = source.Name;
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods

    }
}
