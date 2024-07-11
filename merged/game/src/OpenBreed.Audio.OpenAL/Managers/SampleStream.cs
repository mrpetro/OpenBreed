using OpenBreed.Audio.Interface.Managers;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Managers
{
    internal class SampleStream
    {
        #region Public Fields

        public const int NUM_BUFFERS = 4;
        public const int BUFFER_SIZE = 48000;

        #endregion Public Fields

        #region Private Fields

        private readonly SoundStreamReader soundStreamReader;
        public int[] buffers;
        private short[] readerBuffer = new short[BUFFER_SIZE];

        #endregion Private Fields

        #region Public Constructors

        public SampleStream(int[] buffers, SoundStreamReader soundStreamReader)
        {
            this.buffers = buffers;
            this.soundStreamReader = soundStreamReader;

            //Preload some data
            for (int i = 0; i < buffers.Length; ++i)
                FillBuffer(buffers[i]);
        }

        public int FillBuffer(int bufferId)
        {
            var dataLength = soundStreamReader.Invoke(BUFFER_SIZE, readerBuffer);
  
            AL.BufferData(bufferId, ALFormat.Stereo16, ref readerBuffer[0], dataLength * 2, 48000);

            return dataLength;
        }

        internal void PlayAtSource(SoundSource soundSource)
        {
            soundSource.CurrentStream = this;
            AL.SourceQueueBuffers(soundSource.ALSourceId, buffers.Length, buffers);
        }

        #endregion Public Constructors
    }
}
