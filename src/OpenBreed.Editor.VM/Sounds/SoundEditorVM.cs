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
    public class SoundEditorVM : BaseViewModel
    {

        #region Private Fields

        private SoundVM _editableSound;
        private SoundModel _editedSound;

        private SourceBase _source;

        #endregion Private Fields

        #region Public Constructors

        public SoundEditorVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Public Constructors

        #region Public Properties

        public SoundVM EditableSound
        {
            get { return _editableSound; }
            set { SetProperty(ref _editableSound, value); }
        }

        public EditorVM Root { get; private set; }

        public SourceBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void EditSound(SoundModel sound)
        {
            //Unsubscribe to previous edited item changes
            if (EditableSound != null)
                EditableSound.PropertyChanged -= EditableSound_PropertyChanged;

            _editedSound = sound;

            EditableSound = new SoundVM();
            UpdateVM(sound, EditableSound);
            EditableSound.PropertyChanged += EditableSound_PropertyChanged;
        }

        public void TryClose()
        {
            if (Source != null)
            {
                Source.Dispose();
                Source = null;
            }
        }

        public void TryLoad(string name)
        {
            Load(name);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Load(string name)
        {
            var model = Root.DataProvider.GetSound(name);
            EditSound(model);
        }

        #endregion Internal Methods

        #region Private Methods

        private void EditableSound_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Play()
        {
            var pcmPlayer = new PCMPlayer(EditableSound.Data,
                                          EditableSound.SampleRate, 
                                          EditableSound.BitsPerSample, 
                                          EditableSound.Channels);
            pcmPlayer.PlaySync();
        }

        private void UpdateVM(SoundModel source, SoundVM target)
        {
            target.BitsPerSample = source.BitsPerSample;
            target.Channels = source.Channels;
            target.SampleRate = source.SampleRate;
            target.Data = source.Data;
        }

        private void UpdateModel(SoundVM source, SoundModel target)
        {
            target.BitsPerSample = source.BitsPerSample;
            target.Channels = source.Channels;
            target.SampleRate = source.SampleRate;
            target.Data = source.Data;
        }

        #endregion Private Methods
    }
}
