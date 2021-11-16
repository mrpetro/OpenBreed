using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public class MapAssets
    {
        #region Public Fields

        public int AtlasId;

        #endregion Public Fields

        #region Private Fields

        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal MapAssets(ITileMan tileMan)
        {
            this.tileMan = tileMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = tileMan.GetByName(atlasAlias);
            this.AtlasId = atlas.Id;
        }

        public void SetTileAtlas(int atlasId)
        {
            this.AtlasId = atlasId;
        }

        #endregion Public Methods
    }
}