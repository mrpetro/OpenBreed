using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools.Sounds;
using System.Media;

namespace OpenBreed.Common.Windows
{
    public class PcmPlayer : IPcmPlayer
    {
        public PcmPlayer()
        {
        }

        public void Play(byte[] pcmSampleBytes, int samplingRate, int bitsPerSample, int channels)
        {
            using (WaveMemoryStream waveStream = new WaveMemoryStream(pcmSampleBytes, samplingRate, bitsPerSample, channels))
            {
                var soundPlayer = new SoundPlayer(waveStream);

                soundPlayer.Play();
            }
        }
    }
}
