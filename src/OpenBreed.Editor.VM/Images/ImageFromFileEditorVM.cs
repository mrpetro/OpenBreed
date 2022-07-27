using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageFromFileEditorVM : BaseViewModel, IEntryEditor<IDbImage>
    {
        #region Private Fields

        private string _assetRef;

        private Image image;
        private readonly IWorkspaceMan workspaceMan;
        private readonly IDialogProvider dialogProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ImageFromFileEditorVM(IWorkspaceMan workspaceMan, IDialogProvider dialogProvider, IModelsProvider dataProvider)
        {
            this.workspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            this.dataProvider = dataProvider;
            ImageAssetRefIdEditor = new EntryRefIdEditorVM(workspaceMan, typeof(IDbAsset));
            ImageAssetRefIdEditor.RefIdSelected = (newRefId) => { AssetRef = newRefId; };
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

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

        public void UpdateEntry(IDbImage entry)
        {
            entry.DataRef = AssetRef;
        }

        public void UpdateVM(IDbImage entry)
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
                if (!dataProvider.TryGetModel<Image>(AssetRef, out Image item, out string message))
                {
                    dialogProvider.ShowMessage(message, "Invalid asset");
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