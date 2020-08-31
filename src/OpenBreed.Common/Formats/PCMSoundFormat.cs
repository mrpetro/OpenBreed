using OpenBreed.Common.DataSources;
using OpenBreed.Common.Model.Sounds;
using OpenBreed.Common.Sounds;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBreed.Common.Formats
{
    public class PCMSoundFormat : IDataFormatType
    {
        #region Public Constructors

        public PCMSoundFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            var sampleRate = (int)parameters.FirstOrDefault(item => item.Name == "SAMPLE_RATE").Value;
            var bitsPerSample = (int)parameters.FirstOrDefault(item => item.Name == "BITS_PER_SAMPLE").Value;
            var channels = (int)parameters.FirstOrDefault(item => item.Name == "CHANNELS").Value;

            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var reader = new BinaryReader(ds.Stream);
            var pcmSampleBytes = reader.ReadBytes((int)ds.Stream.Length);

            var sound = new SoundModel();
            sound.BitsPerSample = bitsPerSample;
            sound.SampleRate = sampleRate;
            sound.Channels = channels;
            sound.Data = pcmSampleBytes;
            return sound;
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}