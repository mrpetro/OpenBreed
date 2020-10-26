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
            RegisterSubeditor<ISoundEntry>((parent) => new SoundFromPcmEditorVM(parent));
        }

        public SoundEditorVM(IRepository repository) : base(repository, "Sound Editor")
        {
        }

        #endregion Public Constructors
    }
}