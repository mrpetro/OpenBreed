using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Sounds;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundEditorVM : EntryEditorBaseVM<ISoundEntry, SoundVM>
    {
        #region Public Constructors

        public SoundEditorVM() 
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Sound Editor"; } }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Private Methods

        public void Play()
        {
            var pcmPlayer = new PCMPlayer(Editable.Data,
                                          Editable.SampleRate, 
                                          Editable.BitsPerSample, 
                                          Editable.Channels);
            pcmPlayer.PlaySync();
        }

        protected override void UpdateEntry(SoundVM source, ISoundEntry target)
        {
            var model = DataProvider.GetSound(target.Name);

            model.BitsPerSample = source.BitsPerSample;
            model.Channels = source.Channels;
            model.SampleRate = source.SampleRate;
            model.Data = source.Data;
        }

        protected override void UpdateVM(ISoundEntry source, SoundVM target)
        {
            var model = DataProvider.GetSound(source.Name);
            target.Name = source.Name;
            target.BitsPerSample = model.BitsPerSample;
            target.Channels = model.Channels;
            target.SampleRate = model.SampleRate;
            target.Data = model.Data;
        }

        #endregion Private Methods
    }
}
