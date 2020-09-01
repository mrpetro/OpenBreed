using OpenBreed.Common.Model.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;
using System;

namespace OpenBreed.Common.Data
{
    public class SpriteSetsDataProvider
    {
        #region Public Constructors

        public SpriteSetsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<ISpriteSetEntry>().GetById(id);
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
            return SpriteSetsDataHelper.FromSprModel(Provider, entry);
        }

        private SpriteSetModel GetModelImpl(ISpriteSetFromImageEntry entry)
        {
            return SpriteSetsDataHelper.FromImageModel(Provider, entry);
        }

        #endregion Private Methods
    }
}