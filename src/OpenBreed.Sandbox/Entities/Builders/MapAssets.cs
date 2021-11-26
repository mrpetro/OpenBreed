using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public class MapAssets
    {
        #region Private Fields

        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal MapAssets(ITileMan tileMan)
        {
            this.tileMan = tileMan;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string TileAtlasName { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void SetTileAtlas(string atlasName)
        {
            TileAtlasName = atlasName;
        }

        #endregion Public Methods
    }
}