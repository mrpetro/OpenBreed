using OpenBreed.Model.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Data
{
    public class SpriteSetsDataProvider
    {
        private readonly IUnitOfWork unitOfWork;
        #region Public Constructors

        public SpriteSetsDataProvider(DataProvider provider, IUnitOfWork unitOfWork)
        {
            Provider = provider;
            this.unitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = unitOfWork.GetRepository<ISpriteSetEntry>().GetById(id);
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