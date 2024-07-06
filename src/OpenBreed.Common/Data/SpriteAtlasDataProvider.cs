using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Sprites;
using System;

namespace OpenBreed.Common.Data
{
    public class SpriteAtlasDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IBitmapProvider bitmapProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlasDataProvider(
            IModelsProvider dataProvider,
            IRepositoryProvider repositoryProvider,
            IBitmapProvider bitmapProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
            this.bitmapProvider = bitmapProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById(id);
            if (entry == null)
                throw new Exception("SpriteSet error: " + id);

            return GetModel(entry);
        }

        public SpriteSetModel GetSpriteAtlas(IDbSpriteAtlas dbSpriteAtlas, bool refresh = false)
        {
            switch (dbSpriteAtlas)
            {
                case IDbSpriteAtlasFromSpr dbSpriteAtlasFromSpr:
                    return dataProvider.GetModel<IDbSpriteAtlasFromSpr, SpriteSetModel>(dbSpriteAtlasFromSpr, refresh);
                case IDbSpriteAtlasFromImage dbSpriteAtlasFromImage:
                    return dataProvider.GetModel<IDbSpriteAtlasFromImage, SpriteSetModel>(dbSpriteAtlasFromImage, refresh);
                default:
                    throw new NotImplementedException(dbSpriteAtlas.GetType().ToString());
            }
        }

        #endregion Public Methods

        #region Private Methods

        private SpriteSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        private SpriteSetModel GetModelImpl(IDbSpriteAtlasFromSpr entry)
        {
            return SpriteAtlasDataHelper.FromSprModel(dataProvider, entry);
        }

        private SpriteSetModel GetModelImpl(IDbSpriteAtlasFromImage entry)
        {
            return SpriteAtlasDataHelper.FromImageModel(dataProvider, bitmapProvider, entry);
        }

        #endregion Private Methods
    }
}