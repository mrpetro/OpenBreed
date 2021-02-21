using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Sprites;
using System;

namespace OpenBreed.Common.Data
{
    public class SpriteSetsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = repositoryProvider.GetRepository<ISpriteSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("SpriteSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private SpriteSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        private SpriteSetModel GetModelImpl(ISpriteSetFromSprEntry entry)
        {
            return SpriteSetsDataHelper.FromSprModel(dataProvider, entry);
        }

        private SpriteSetModel GetModelImpl(ISpriteSetFromImageEntry entry)
        {
            return SpriteSetsDataHelper.FromImageModel(dataProvider, entry);
        }

        #endregion Private Methods
    }
}