using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Sounds;
using System;

namespace OpenBreed.Common.Data
{
    public class SoundsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SoundsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public SoundModel GetSound(IDbSound dbSound, bool refresh = false)
        {
            return dataProvider.GetModel<IDbSound, SoundModel>(dbSound, refresh);
        }

        #endregion Public Methods
    }
}