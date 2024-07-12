using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface
{
    public interface IPcmPlayer
    {
        #region Public Methods

        void Play(byte[] pcmSampleBytes, int samplingRate, int bitsPerSample, int channels);

        #endregion Public Methods
    }
}