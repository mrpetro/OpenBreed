using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageFromAcbmEditorVM : ImageEditorBaseVM<IDbAcbmImage>
    {
        #region Private Fields

        private readonly ImagesDataProvider imagesDataProvider;
        private readonly IBitmapProvider bitmapProvider;

        private IImage image;

        #endregion Private Fields

        #region Public Constructors

        public ImageFromAcbmEditorVM(
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
                typeof(IDbAsset),
                (newRefId) => DataRef = newRefId);
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM ImageAssetRefIdEditor { get; }

        public int Width
        {
            get { return Entry.Width; }
            set { SetProperty(Entry, x => x.Width, value); }
        }

        public int Height
        {
            get { return Entry.Height; }
            set { SetProperty(Entry, x => x.Height, value); }
        }

        public int BitPlanesNo
        {
            get { return Entry.BitPlanesNo; }
            set { SetProperty(Entry, x => x.BitPlanesNo, value); }
        }

        public string PaletteMode
        {
            get { return Entry.PaletteMode; }
            set { SetProperty(Entry, x => x.PaletteMode, value); }
        }

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        public override string EditorName => "ACBM image editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbAcbmImage entry)
        {
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbAcbmImage entry)
        {
            DataRef = entry.DataRef;
            ImageAssetRefIdEditor.SelectedRefId = DataRef;

            UpdateImage();
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Width):
                case nameof(Height):
                case nameof(BitPlanesNo):
                case nameof(PaletteMode):
                    UpdateImage();
                    break;

                case nameof(DataRef):
                    ImageAssetRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
                    UpdateImage();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
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

        #endregion Private Methods
    }
}