using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;

namespace OpenBreed.Common.Sound
{
    public class PCMPlayer
    {
        private SoundPlayer m_SoundPlayer = null;

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
                    m_SoundPlayer = new SoundPlayer(waveStream);
                }
            }
        }

        public void Play()
        {
            m_SoundPlayer.Play();
        }

        public void PlaySync()
        {
            m_SoundPlayer.PlaySync();
        }
    }
}
