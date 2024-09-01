using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Data
{
    internal class SoundSampleDataLoader : ISoundSampleDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly IModelsProvider modelsProvider;
        private readonly ISoundMan soundMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public SoundSampleDataLoader(IRepositoryProvider repositoryProvider,
                                   AssetsDataProvider assetsDataProvider,
                                   IModelsProvider modelsProvider,
                                   ISoundMan soundMan,
                                   ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.modelsProvider = modelsProvider;
            this.soundMan = soundMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public int Load(IDbSound dbSound)
        {
            var soundSampleId = soundMan.GetByName(dbSound.Id);

            if (soundSampleId != -1)
            {
                return soundSampleId;
            }

            modelsProvider.TryGetModel<IDbSound, SoundModel>(dbSound, out SoundModel model, out string message);

            soundSampleId = soundMan.LoadSample(dbSound.Id, model.Data, model.SampleRate);

            logger.LogTrace("Sound sample '{0}' loaded.", dbSound.Id);

            return soundSampleId;
        }

        public int Load(string dbEntry)
        {
            var soundSampleId = soundMan.GetByName(dbEntry);

            if (soundSampleId != -1)
            {
                return soundSampleId;
            }

            var entry = repositoryProvider.GetRepository<IDbSound>().GetById(dbEntry);

            if (entry is null)
            {
                throw new Exception("Sound sample error: " + dbEntry);
            }

            return Load(entry);
        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}
