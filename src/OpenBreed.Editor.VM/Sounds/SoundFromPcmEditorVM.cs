using OpenBreed.Common.Helpers.Sounds;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundFromPcmEditorVM : BaseViewModel, IEntryEditor<ISoundEntry>
    {
        #region Private Fields

        private int _bitsPerSample;
        private int _sampleRate;
        private int _channels;
        private byte[] _data;

        #endregion Private Fields

        #region Public Constructors

        public SoundFromPcmEditorVM(SoundEditorVM parent)
        {
            Parent = parent;
        }

        #endregion Public Constructors

        #region Public Properties

        public SoundEditorVM Parent { get; }

        public int BitsPerSample
        {
            get { return _bitsPerSample; }
            set { SetProperty(ref _bitsPerSample, value); }
        }

        public int Channels
        {
            get { return _channels; }
            set { SetProperty(ref _channels, value); }
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set { SetProperty(ref _sampleRate, value); }
        }

        public byte[] Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateEntry(ISoundEntry entry)
        {
            var model = Parent.DataProvider.Sounds.GetSound(entry.Id);

            model.BitsPerSample = BitsPerSample;
            model.Channels = Channels;
            model.SampleRate = SampleRate;
            model.Data = Data;
        }

        internal void Play()
        {
            var pcmPlayer = new PCMPlayer(Data,
                                          SampleRate,
                                          BitsPerSample,
                                          Channels);
            pcmPlayer.PlaySync();
        }

        public void UpdateVM(ISoundEntry entry)
        {
            var model = Parent.DataProvider.Sounds.GetSound(entry.Id);
            BitsPerSample = model.BitsPerSample;
            Channels = model.Channels;
            SampleRate = model.SampleRate;
            Data = model.Data;
        }

        #endregion Public Methods
    }
}