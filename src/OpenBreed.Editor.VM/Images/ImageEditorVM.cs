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
using OpenBreed.Editor.VM.Common;
using OpenBreed.Common.Assets;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : EntryEditorBaseVM<IImageEntry, ImageVM>
    {

        #region Public Constructors

        public ImageEditorVM(IRepository repository) : base(repository)
        {
            ImageAssetRefIdEditor = new EntryRefIdEditorVM(typeof(IAssetEntry));
            ImageAssetRefIdEditor.RefIdSelected = (newRefId) => { Editable.AssetRef = newRefId; };
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM ImageAssetRefIdEditor { get; }

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

        protected override void UpdateEntry(ImageVM source, IImageEntry entry)
        {
            entry.DataRef = source.AssetRef;

            base.UpdateEntry(source, entry);
        }

        protected override void UpdateVM(IImageEntry entry, ImageVM target)
        {
            target.AssetRef = entry.DataRef;

            if(entry.DataRef != null)
                target.Image = DataProvider.Images.GetImage(entry.Id);

            base.UpdateVM(entry, target);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    ImageAssetRefIdEditor.RefId = (Editable.AssetRef == null) ? null : Editable.AssetRef;
                    break;
                default:
                    break;
            }
        }


        #endregion Protected Methods

    }
}
