using OpenBreed.Common.Sounds;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class PCMSoundFormat : IDataFormatType
    {
        public PCMSoundFormat()
        {
        }

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            var sampleRate = (int)parameters.FirstOrDefault(item => item.Name == "SAMPLE_RATE").Value;
            var bitsPerSample = (int)parameters.FirstOrDefault(item => item.Name == "BITS_PER_SAMPLE").Value;
            var channels = (int)parameters.FirstOrDefault(item => item.Name == "CHANNELS").Value;

            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            var reader = new BinaryReader(asset.Stream);
            var pcmSampleBytes = reader.ReadBytes((int)asset.Stream.Length);

            var sound = new SoundModel();
            sound.BitsPerSample = bitsPerSample;
            sound.SampleRate = sampleRate;
            sound.Channels = channels;
            sound.Data = pcmSampleBytes;
            return sound;
        }

        public void Save(AssetBase asset, object model)
        {
            throw new NotImplementedException();
        }
    }
}