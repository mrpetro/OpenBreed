using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.VM.Base;
using System;
using System.Threading.Channels;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM.Sounds
{
    public class PcmSoundEditorVM : EntryEditorBaseVM<IDbSound>
    {
        #region Private Fields

        private readonly SoundsDataProvider soundsDataProvider;
        private readonly IPcmPlayer pcmPlayer;
        private int _bitsPerSample;
        private int _sampleRate;
        private int _channels;
        private byte[] _data;

        #endregion Private Fields

        #region Public Constructors

        public PcmSoundEditorVM(IWorkspaceMan workspaceMan, IDialogProvider dialogProvider, SoundsDataProvider soundsDataProvider, IPcmPlayer pcmPlayer) : base(workspaceMan, dialogProvider)
        {
            this.soundsDataProvider = soundsDataProvider;
            this.pcmPlayer = pcmPlayer;
            PlayCommand = new Command(() => Play());
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand PlayCommand { get; }

        public override string EditorName => "PCM Sound Editor";

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
            pcmPlayer.Play(
                Data,
                SampleRate,
                BitsPerSample,
                Channels);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(IDbSound entry)
        {
            var model = soundsDataProvider.GetSound(entry.Id);

            model.BitsPerSample = BitsPerSample;
            model.Channels = Channels;
            model.SampleRate = SampleRate;
            model.Data = Data;

            base.UpdateEntry(entry);
        }

        protected override void UpdateVM(IDbSound entry)
        {
            base.UpdateVM(entry);

            var model = soundsDataProvider.GetSound(entry.Id);
            BitsPerSample = model.BitsPerSample;
            Channels = model.Channels;
            SampleRate = model.SampleRate;
            Data = model.Data;
        }

        #endregion Protected Methods
    }
}