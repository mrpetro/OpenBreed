using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Sprites;
using System;

namespace OpenBreed.Common.Data
{
    public class SpriteSetsDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        private readonly IDataProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetsDataProvider(IDataProvider dataProvider, IWorkspaceMan workspaceMan)
        {
            this.dataProvider = dataProvider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<ISpriteSetEntry>().GetById(id);
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