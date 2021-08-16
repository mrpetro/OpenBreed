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
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public MapCellHelper(WorldBlockBuilder worldBlockBuilder, ICommandsMan commandsMan)
        {
            this.worldBlockBuilder = worldBlockBuilder;
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddCell(World world, int ix, int iy, int gfxValue, bool hasBody)
        {
            worldBlockBuilder.SetPosition(ix * 16, iy * 16);
            worldBlockBuilder.SetTileId(gfxValue);
            worldBlockBuilder.HasBody = hasBody;

            var cellEntity = worldBlockBuilder.Build();
            commandsMan.Post(new AddEntityCommand(world.Id, cellEntity.Id));
        }

        #endregion Public Methods
    }
}