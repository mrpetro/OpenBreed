using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;

namespace OpenBreed.Editor.VM.Animations
{
    public abstract class AnimationEditorBaseVM<TDbAnimation> : EntryEditorBaseVM<IDbAnimation> where TDbAnimation : IDbAnimation
    {
        #region Public Constructors

        public AnimationEditorBaseVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
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