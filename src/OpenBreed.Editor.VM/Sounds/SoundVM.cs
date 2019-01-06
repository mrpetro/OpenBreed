using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sounds
{
    public class SoundVM : EditableEntryVM
    {

        #region Private Fields

        private int _bitsPerSample;
        private int _sampleRate;
        private int _channels;
        private byte[] _data;

        #endregion Private Fields

        #region Public Constructors

        public SoundVM()
        {

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

    }
}
