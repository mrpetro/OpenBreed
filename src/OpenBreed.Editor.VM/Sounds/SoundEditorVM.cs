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
            RegisterSubeditor<ISoundEntry>((parent) => new SoundFromPcmEditorVM(parent.DataProvider.Sounds));
        }

        public SoundEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Sound Editor")
        {
        }

        #endregion Public Constructors
    }
}