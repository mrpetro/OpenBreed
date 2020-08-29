using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : EntryEditorBaseVM<IImageEntry, ImageVM>
    {
        #region Private Fields

        private Image image;

        #endregion Private Fields

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

        public Image Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
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

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEditablePropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Editable.AssetRef):
                    UpdateImage(Editable);
                    Refresh();
                    break;

                default:
                    break;
            }

            base.OnEditablePropertyChanged(propertyName);
        }

        protected override void UpdateEntry(ImageVM source, IImageEntry entry)
        {
            entry.DataRef = source.AssetRef;
            base.UpdateEntry(source, entry);
        }

        protected override void UpdateVM(IImageEntry entry, ImageVM target)
        {
            target.AssetRef = entry.DataRef;
            UpdateImage(target);
            base.UpdateVM(entry, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Refresh()
        {
            RefreshAction?.Invoke();
        }

        private void UpdateImage(ImageVM vm)
        {
            if (vm.AssetRef != null)
            {
                if (!DataProvider.TryGetData<Image>(vm.AssetRef, out Image item, out string message))
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
                case nameof(Editable):
                    ImageAssetRefIdEditor.RefId = (Editable.AssetRef == null) ? null : Editable.AssetRef;
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}