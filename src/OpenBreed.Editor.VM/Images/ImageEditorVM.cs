using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : EntryEditorBaseVM<IImageEntry, ImageVM>
    {

        #region Public Constructors

        public ImageEditorVM(IRepository repository) : base(repository)
        {
            DataSource = new ImageDataSourceVM();

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ImageDataSourceVM DataSource { get; }
        public override string EditorName { get { return "Image Editor"; } }

        #endregion Public Properties

        #region Public Methods

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            if (Editable == null)
                return;

            if (Editable.Image == null)
                return;

            int width = Editable.Width * factor;
            int height = Editable.Height * factor;

            gfx.DrawImage(Editable.Image, (int)x, (int)y, width, height);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(ImageVM source, IImageEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(IImageEntry source, ImageVM target)
        {
            target.AssetRef = source.AssetRef;

            if(source.AssetRef != null)
                target.Image = DataProvider.GetImage(source.Id);

            base.UpdateVM(source, target);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    DataSource.AssetEntryRef.RefId = (Editable.AssetRef == null) ? null : Editable.AssetRef;
                    break;
                default:
                    break;
            }
        }


        #endregion Protected Methods

    }
}
