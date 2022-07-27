using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using System;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundEditorVM : ParentEntryEditor<IDbSound>
    {
        #region Public Constructors

        public SoundEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Sound Editor")
        {
        }

        #endregion Public Constructors
    }
}