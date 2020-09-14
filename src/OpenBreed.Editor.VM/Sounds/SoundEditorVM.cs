using OpenBreed.Common.Helpers.Sounds;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using System;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundEditorVM : EntryEditorBaseExVM<ISoundEntry>
    {
        #region Private Fields

        private IEntryEditor<ISoundEntry> subeditor;

        #endregion Private Fields

        #region Public Constructors

        public SoundEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Sound Editor"; } }

        public IEntryEditor<ISoundEntry> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        #endregion Public Properties

        #region Public Methods

        internal Action PlayAction { get; private set; }

        public void Play()
        {
            PlayAction?.Invoke();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(ISoundEntry target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateEntry(target);
        }

        protected override void UpdateVM(ISoundEntry source)
        {
            if (source is ISoundEntry)
            {
                var pcmEditor = new SoundFromPcmEditorVM(this);
                PlayAction = pcmEditor.Play;
                Subeditor = pcmEditor;
            }
            else
                throw new NotImplementedException();


            base.UpdateVM(source);
            Subeditor.UpdateVM(source);
        }

        #endregion Protected Methods
    }
}