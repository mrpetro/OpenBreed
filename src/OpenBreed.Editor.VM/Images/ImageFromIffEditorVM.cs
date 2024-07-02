using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageFromIffEditorVM : ImageEditorBaseVM<IDbIffImage>
    {
        #region Private Fields

        private readonly ImagesDataProvider imagesDataProvider;
        private readonly IBitmapProvider bitmapProvider;

        private IImage image;

        #endregion Private Fields

        #region Public Constructors

        public ImageFromIffEditorVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            ImagesDataProvider imagesDataProvider,
            IBitmapProvider bitmapProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.imagesDataProvider = imagesDataProvider;
            this.bitmapProvider = bitmapProvider;
            ImageAssetRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbDataSource),
                (newRefId) => DataRef = newRefId);
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM ImageAssetRefIdEditor { get; }

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        public override string EditorName => "IFF image editor";

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(IDbIffImage entry)
        {
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbIffImage entry)
        {
            ImageAssetRefIdEditor.SelectedRefId = DataRef;

            UpdateImage();
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateImage()
        {
            if (DataRef is null)
            {   
                Image = bitmapProvider.ErrorIcon; 
                return;
            }

            Image = imagesDataProvider.GetImage(Entry, refresh: true);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataRef):
                    ImageAssetRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
                    UpdateImage();
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Private Methods
    }
}