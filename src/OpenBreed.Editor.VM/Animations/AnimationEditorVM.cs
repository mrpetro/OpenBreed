using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationEditorVM : ParentEntryEditor<IDbAnimation>
    {
        #region Public Constructors

        public AnimationEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Animation Editor")
        {
        }

        #endregion Public Constructors
    }
}