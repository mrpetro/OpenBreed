using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class UnknownCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int UNKNOWN_CODE = -1;

        #endregion Public Fields

        #region Private Fields

        private readonly GenericCellHelper genericCellHelper;

        #endregion Private Fields

        #region Public Constructors

        public UnknownCellEntityLoader(GenericCellHelper genericCellHelper)
        {
            this.genericCellHelper = genericCellHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public Entity Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var actionValue = int.Parse(flavor);

            var entity = genericCellHelper.AddUnknownCell(world, ix, iy, actionValue, mapAssets.Level, gfxValue);

            visited[ix, iy] = true;

            return entity;
        }

        #endregion Public Methods
    }
}