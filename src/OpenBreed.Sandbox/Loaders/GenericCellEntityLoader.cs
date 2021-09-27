using OpenBreed.Core.Managers;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int OBSTACLE_CODE = 63;
        public const int VOID_CODE = 0;

        #endregion Public Fields

        #region Protected Fields

        #endregion Protected Fields

        #region Private Fields

        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellEntityLoader(ICommandsMan commandsMan)
        {
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case OBSTACLE_CODE:
                    PutGenericCell(worldBlockBuilder, layout, world, ix, iy, gfxValue, actionValue, hasBody: true, unknown: false);
                    break;

                case VOID_CODE:
                    PutGenericCell(worldBlockBuilder, layout, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: false);
                    break;

                //default:
                //    PutGenericCell(worldBlockBuilder, layout, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: true);
                //    break;
            }

            visited[ix, iy] = true;
        }

        #endregion Public Methods

        #region Private Methods

        private void PutGenericCell(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, World world, int ix, int iy, int gfxValue, int actionValue, bool hasBody, bool unknown)
        {
            var groupLayerIndex = layout.GetLayerIndex(MapLayerType.Group);

            worldBlockBuilder.SetPosition(ix * layout.CellSize, iy * layout.CellSize);
            worldBlockBuilder.SetTileId(gfxValue);
            worldBlockBuilder.SetGroupId(layout.GetCellValue(groupLayerIndex, ix, iy));
            worldBlockBuilder.HasBody = hasBody;

            var cellEntity = worldBlockBuilder.Build();

            if (unknown)
                cellEntity.Tag = actionValue;

            commandsMan.Post(new AddEntityCommand(world.Id, cellEntity.Id));
        }

        #endregion Private Methods
    }
}