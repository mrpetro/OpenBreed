using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Sounds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class PcmSoundDataHandler : AssetDataHandlerBase<IDbSound>
    {
        #region Private Fields

        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public PcmSoundDataHandler(DataSourceProvider dataSourceProvider)
        {
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        protected override void Save(IDbSound dbEntry, object model)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override object Load(IDbSound dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds, dbEntry.SampleRate, dbEntry.BitsPerSample, dbEntry.Channels);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds, int sampleRate, int bitsPerSample, int channels)
        {
            if (sampleRate <= 0)
            {
                throw new Exception("SampleRate should be greater than zero.");
            }

            if (bitsPerSample <= 0)
            {
                throw new Exception("BitsPerSample should be greater than zero.");
            }

            if (channels <= 0)
            {
                throw new Exception("Channels should be greater than zero.");
            }

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

        #endregion Private Methods
    }
}