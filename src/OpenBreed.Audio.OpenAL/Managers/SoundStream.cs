using OpenBreed.Audio.Abstractions.Managers;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Managers
{
    internal class SoundStream
    {
        #region Public Fields

        public const int NUM_BUFFERS = 4;
        public const int BUFFER_SIZE = 48000;

        public int[] buffers;

        #endregion Public Fields

        #region Private Fields

        private readonly SoundStreamReader soundStreamReader;
        private short[] readerBuffer = new short[BUFFER_SIZE];

        #endregion Private Fields

        #region Public Constructors

        public SoundStream(int[] buffers, SoundStreamReader soundStreamReader)
        {
            this.buffers = buffers;
            this.soundStreamReader = soundStreamReader;

            //Preload some data
            for (int i = 0; i < buffers.Length; ++i)
                FillBuffer(buffers[i]);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id { get; init; }

        #endregion Public Properties

        #region Public Methods

        public int FillBuffer(int bufferId)
        {
            var dataLength = soundStreamReader.Invoke(BUFFER_SIZE, readerBuffer);

            AL.BufferData(bufferId, Format.Stereo16, ref readerBuffer[0], dataLength * 2, 48000);

            return dataLength;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void PlayAtSource(SoundSource soundSource)
        {
            soundSource.CurrentStream = this;
            AL.SourceQueueBuffers(soundSource.ALSourceId, buffers.Length, buffers);
        }

        #endregion Internal Methods
    }
}