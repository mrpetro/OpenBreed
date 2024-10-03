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
            TDbAnimation dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors
    }
}