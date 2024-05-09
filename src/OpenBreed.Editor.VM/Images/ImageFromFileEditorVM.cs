using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageFromFileEditorVM : EntryEditorBaseVM<IDbImage>
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private readonly IDialogProvider dialogProvider;
        private readonly IModelsProvider dataProvider;
        private readonly IBitmapProvider bitmapProvider;
        private string _assetRef;

        private IImage image;

        #endregion Private Fields

        #region Public Constructors

        public ImageFromFileEditorVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IModelsProvider dataProvider,
            IControlFactory controlFactory,
            IBitmapProvider bitmapProvider) : base(workspaceMan, dialogProvider, controlFactory)
        {
            this.workspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            this.dataProvider = dataProvider;
            this.bitmapProvider = bitmapProvider;
            ImageAssetRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbAsset),
                (newRefId) => AssetRef = newRefId);

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM ImageAssetRefIdEditor { get; }

        public IImage Image
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

        public override string EditorName => "Image Editor";

        #endregion Public Properties

        #region Public Methods

        public void Draw(IDrawingContext gfx)
        {
            if (Image == null)
                return;

            gfx.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(IDbImage entry)
        {
            entry.DataRef = AssetRef;

            base.UpdateEntry(entry);
        }

        protected override void UpdateVM(IDbImage entry)
        {
            base.UpdateVM(entry);

            AssetRef = entry.DataRef;
            ImageAssetRefIdEditor.SelectedRefId = AssetRef;

            UpdateImage();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Refresh()
        {
            RefreshAction?.Invoke();
        }

        private void UpdateImage()
        {
            if (AssetRef != null)
            {
                if (!dataProvider.TryGetModel<IImage>(AssetRef, out IImage item, out string message))
                {
                    dialogProvider.ShowMessage(message, "Invalid asset");
                    Image = bitmapProvider.ErrorIcon; 
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
                    ImageAssetRefIdEditor.CurrentRefId = (AssetRef == null) ? null : AssetRef;
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