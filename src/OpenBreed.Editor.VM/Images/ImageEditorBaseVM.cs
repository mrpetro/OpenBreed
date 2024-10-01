using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Images
{
    public abstract class ImageEditorBaseVM<TDbImage> : EntrySpecificEditorVM<IDbImage> where TDbImage : IDbImage
    {
        #region Private Fields

        private IImage image;

        #endregion Private Fields

        #region Public Constructors

        public ImageEditorBaseVM(
            TDbImage dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new TDbImage Entry => (TDbImage)base.Entry;

        public IImage Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Draw(IDrawingContext context)
        {
            if (Image is null)
            {
                return;
            }

            context.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntry(TDbImage target);

        protected abstract void UpdateVM(TDbImage source);

        protected override void UpdateVM(IDbImage source)
        {
            base.UpdateVM(source);

            UpdateVM((TDbImage)source);
        }

        protected override void UpdateEntry(IDbImage target)
        {
            UpdateEntry((TDbImage)target);

            base.UpdateEntry(target);
        }

        #endregion Protected Methods
    }
}