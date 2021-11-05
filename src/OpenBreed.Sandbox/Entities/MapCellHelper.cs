using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Entities
{
    public class MapCellHelper
    {
        #region Private Fields

        private readonly WorldBlockBuilder worldBlockBuilder;

        #endregion Private Fields

        #region Public Constructors

        public MapCellHelper(WorldBlockBuilder worldBlockBuilder)
        {
            this.worldBlockBuilder = worldBlockBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddCell(World world, int ix, int iy, int gfxValue, bool hasBody)
        {
            worldBlockBuilder.SetPosition(ix * 16, iy * 16);
            worldBlockBuilder.SetTileId(gfxValue);
            worldBlockBuilder.HasBody = hasBody;

            var cellEntity = worldBlockBuilder.Build();
            cellEntity.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}