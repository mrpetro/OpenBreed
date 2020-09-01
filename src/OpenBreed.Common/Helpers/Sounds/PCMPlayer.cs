using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;

namespace OpenBreed.Common.Helpers.Sounds
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
            using (FileStream pcmSampleStream = File.Open(pcmFilePath, FileMode.Open))
            {
                byte[] pcmSampleBytes = null;
                BinaryReader br = new BinaryReader(pcmSampleStream);
                long numBytes = new FileInfo(pcmFilePath).Length;
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
}
