using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Tiles.Helpers;
using System;
using System.Threading.Channels;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM.Sounds
{
    public class PcmSoundEditorVM : EntrySpecificEditorVM<IDbSound>
    {
        #region Private Fields

        private readonly SoundsDataProvider soundsDataProvider;
        private readonly IPcmPlayer pcmPlayer;
        private string _dataSourceRefId;
        private int _bitsPerSample;
        private int _sampleRate;
        private int _channels;

        #endregion Private Fields

        #region Public Constructors

        public PcmSoundEditorVM(
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            SoundsDataProvider soundsDataProvider,
            IPcmPlayer pcmPlayer) : base(logger, workspaceMan, dialogProvider)
        {
            this.soundsDataProvider = soundsDataProvider;
            this.pcmPlayer = pcmPlayer;

            DataSourceRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbDataSource),
                (newRefId) => DataSourceRefId = newRefId);

            PlayCommand = new Command(() => Play());
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand PlayCommand { get; }

        public override string EditorName => "PCM Sound Editor";

        public EntryRefIdEditorVM DataSourceRefIdEditor { get; }

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

        public string DataSourceRefId
        {
            get { return _dataSourceRefId; }
            set { SetProperty(ref _dataSourceRefId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Play()
        {
            var soundModel = soundsDataProvider.GetSound(Entry);

            pcmPlayer.Play(
                soundModel.Data,
                SampleRate,
                BitsPerSample,
                Channels);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataSourceRefId):

                    DataSourceRefIdEditor.CurrentRefId = (DataSourceRefId == null) ? null : DataSourceRefId;
                    //UpdateImage();
                    //Refresh();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        protected override void UpdateEntry(IDbSound entry)
        {
            entry.BitsPerSample = BitsPerSample;
            entry.Channels = Channels;
            entry.SampleRate = SampleRate;
            entry.DataRef = DataSourceRefIdEditor.SelectedRefId;

            base.UpdateEntry(entry);
        }

        protected override void UpdateVM(IDbSound entry)
        {
            base.UpdateVM(entry);

            DataSourceRefId = entry.DataRef;
            DataSourceRefIdEditor.SelectedRefId = entry.DataRef;
            BitsPerSample = entry.BitsPerSample;
            Channels = entry.Channels;
            SampleRate = entry.SampleRate;
        }

        #endregion Protected Methods
    }
}