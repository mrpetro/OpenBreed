using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;

namespace OpenBreed.Editor.VM.Animations
{
    public abstract class AnimationEditorBaseVM<TDbAnimation> : EntrySpecificEditorVM<IDbAnimation> where TDbAnimation : IDbAnimation
    {
        #region Public Constructors

        public AnimationEditorBaseVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected abstract void UpdateVM(TDbAnimation entry);

        protected abstract void UpdateEntry(TDbAnimation entry);

        protected override void UpdateVM(IDbAnimation entry)
        {
            base.UpdateVM(entry);

            UpdateVM((TDbAnimation)entry);
        }

        protected override void UpdateEntry(IDbAnimation entry)
        {
            UpdateEntry((TDbAnimation)entry);

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods
    }
}