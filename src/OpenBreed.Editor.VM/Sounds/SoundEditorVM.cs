using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Sounds;
using OpenBreed.Editor.VM.Sounds;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundEditorVM : EntryEditorBaseVM<SoundModel, SoundVM>
    {
        #region Public Constructors

        public SoundEditorVM(EditorVM root) : base(root)
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

        protected override SoundModel GetModel(string name)
        {
            return Root.DataProvider.GetSound(name);
        }

        protected override void UpdateModel(SoundVM source, SoundModel target)
        {
            target.BitsPerSample = source.BitsPerSample;
            target.Channels = source.Channels;
            target.SampleRate = source.SampleRate;
            target.Data = source.Data;
        }

        protected override void UpdateVM(SoundModel source, SoundVM target)
        {
            target.BitsPerSample = source.BitsPerSample;
            target.Channels = source.Channels;
            target.SampleRate = source.SampleRate;
            target.Data = source.Data;
        }

        #endregion Private Methods
    }
}
