using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageFromFileEditorVM : BaseViewModel, IEntryEditor<IImageEntry>
    {
        #region Private Fields

        private string _assetRef;

        private Image image;

        #endregion Private Fields

        #region Public Constructors

        public ImageFromFileEditorVM(ParentEntryEditor<IImageEntry> parent)
        {
            Parent = parent;

            ImageAssetRefIdEditor = new EntryRefIdEditorVM(typeof(IAssetEntry));
            ImageAssetRefIdEditor.RefIdSelected = (newRefId) => { AssetRef = newRefId; };
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ParentEntryEditor<IImageEntry> Parent { get; }
        public EntryRefIdEditorVM ImageAssetRefIdEditor { get; }

        public Image Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        public string AssetRef
        {
            get { return _assetRef; }
            set { SetProperty(ref _assetRef, value); }
        }

        public Action RefreshAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Draw(Graphics gfx)
        {
            if (Image == null)
                return;

            gfx.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        public void UpdateEntry(IImageEntry entry)
        {
            entry.DataRef = AssetRef;
        }

        public void UpdateVM(IImageEntry entry)
        {
            AssetRef = entry.DataRef;
            UpdateImage();
        }

        #endregion Public Methods

        #region Private Methods

        private void Refresh()
        {
            RefreshAction?.Invoke();
        }

        private void UpdateImage()
        {
            if (AssetRef != null)
            {
                var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

                if (!dataProvider.TryGetData<Image>(AssetRef, out Image item, out string message))
                {
                    ServiceLocator.Instance.GetService<IDialogProvider>().ShowMessage(message, "Invalid asset");
                    Image = System.Drawing.SystemIcons.Error.ToBitmap();
                }
                else
                    Image = item;
            }
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AssetRef):
                    ImageAssetRefIdEditor.RefId = (AssetRef == null) ? null : AssetRef;
                    UpdateImage();
                    Refresh();
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}