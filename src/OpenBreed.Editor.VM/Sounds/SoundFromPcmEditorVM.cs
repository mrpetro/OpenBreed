using OpenBreed.Common.Data;
using OpenBreed.Common.Tools.Sounds;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.VM.Base;
using System.Media;

namespace OpenBreed.Editor.VM.Sounds
{
    public class PCMPlayer
    {
        private readonly SoundPlayer _soundPlayer;

        public PCMPlayer(byte[] pcmSampleBytes, int samplingRate, int bitsPerSample, int channels)
        {
            using (WaveMemoryStream waveStream = new WaveMemoryStream(pcmSampleBytes, samplingRate, bitsPerSample, channels))
            {
                _soundPlayer = new SoundPlayer(waveStream);
            }
        }

        public PCMPlayer(string pcmFilePath, int samplingRate, ushort bitsPerSample, ushort channels)
        {
            using (System.IO.FileStream pcmSampleStream = System.IO.File.Open(pcmFilePath, System.IO.FileMode.Open))
            {
                byte[] pcmSampleBytes = null;
                System.IO.BinaryReader br = new System.IO.BinaryReader(pcmSampleStream);
                long numBytes = new System.IO.FileInfo(pcmFilePath).Length;
                pcmSampleBytes = br.ReadBytes((int)numBytes);
                using (WaveMemoryStream waveStream = new WaveMemoryStream(pcmSampleBytes, samplingRate, bitsPerSample, channels))
                {
                    _soundPlayer = new SoundPlayer(waveStream);
                }
            }
        }

        public void Play()
        {
            _soundPlayer.Play();
        }

        public void PlaySync()
        {
            _soundPlayer.PlaySync();
        }
    }


    public class SoundFromPcmEditorVM : BaseViewModel, IEntryEditor<IDbSound>
    {
        private readonly SoundsDataProvider soundsDataProvider;
        #region Private Fields

        private int _bitsPerSample;
        private int _sampleRate;
        private int _channels;
        private byte[] _data;

        #endregion Private Fields

        #region Public Constructors

        public SoundFromPcmEditorVM(SoundsDataProvider soundsDataProvider)
        {
            this.soundsDataProvider = soundsDataProvider;
        }

        #endregion Public Constructors

        #region Public Properties

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

        public void Play()
        {
            var pcmPlayer = new PCMPlayer(Data,
                                          SampleRate,
                                          BitsPerSample,
                                          Channels);
            pcmPlayer.PlaySync();
        }

        public void UpdateEntry(IDbSound entry)
        {
            var model = soundsDataProvider.GetSound(entry.Id);

            model.BitsPerSample = BitsPerSample;
            model.Channels = Channels;
            model.SampleRate = SampleRate;
            model.Data = Data;
        }

        public void UpdateVM(IDbSound entry)
        {
            var model = soundsDataProvider.GetSound(entry.Id);
            BitsPerSample = model.BitsPerSample;
            Channels = model.Channels;
            SampleRate = model.SampleRate;
            Data = model.Data;
        }

        #endregion Public Methods
    }
}