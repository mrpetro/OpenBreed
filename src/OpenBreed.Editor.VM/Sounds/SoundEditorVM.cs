using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using System;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundEditorVM : ParentEntryEditor<ISoundEntry>
    {
        #region Public Constructors

        static SoundEditorVM()
        {
            RegisterSubeditor<ISoundEntry, ISoundEntry>();
        }

        public SoundEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Sound Editor")
        {
        }

        #endregion Public Constructors
    }
}