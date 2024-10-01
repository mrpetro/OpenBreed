using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Tiles.Helpers;
using OpenBreed.Model.Sounds;
using System;
using System.ComponentModel;
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
        private SoundModel soundModel;

        #endregion Private Fields

        #region Public Constructors

        public PcmSoundEditorVM(
            IDbSound dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            SoundsDataProvider soundsDataProvider,
            IPcmPlayer pcmPlayer) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.soundsDataProvider = soundsDataProvider;
            this.pcmPlayer = pcmPlayer;

            DataSourceRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbDataSource),
            (newRefId) => DataRef = newRefId);

            DataSourceRefIdEditor.SelectedRefId = Entry.DataRef;

            PlayCommand = new Command(() => Play());

            UpdateSound();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand PlayCommand { get; }

        public override string EditorName => "PCM Sound Editor";

        public EntryRefIdEditorVM DataSourceRefIdEditor { get; }

        public int BitsPerSample
        {
            get { return Entry.BitsPerSample; }
            set { SetProperty(Entry, x => x.BitsPerSample, value); }
        }

        public int Channels
        {
            get { return Entry.Channels; }
            set { SetProperty(Entry, x => x.Channels, value); }
        }

        public int SampleRate
        {
            get { return Entry.SampleRate; }
            set { SetProperty(Entry, x => x.SampleRate, value); }
        }

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Play()
        {
            pcmPlayer.Play(
                soundModel.Data,
                soundModel.SampleRate,
                soundModel.BitsPerSample,
                soundModel.Channels);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(BitsPerSample):
                case nameof(Channels):
                case nameof(SampleRate):
                    UpdateSound();
                    break;

                case nameof(DataRef):
                    DataSourceRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
                    UpdateSound();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateSound()
        {
            soundModel = soundsDataProvider.GetSound(Entry, refresh: true);
        }

        #endregion Private Methods
    }
}