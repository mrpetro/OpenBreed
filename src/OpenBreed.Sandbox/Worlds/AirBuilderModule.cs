using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Worlds
{
    public class AirBuilderModule : IBuilderModule
    {
        private readonly ICommandsMan commandsMan;
        private readonly WorldBlockBuilder worldBlockBuilder;

        public AirBuilderModule(ICommandsMan commandsMan, WorldBlockBuilder worldBlockBuilder)
        {
            this.commandsMan = commandsMan;
            this.worldBlockBuilder = worldBlockBuilder;
        }

        public void Build(int code, World world, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var gfxCode = (int)args[2];

            worldBlockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");
            worldBlockBuilder.HasBody = false;
            worldBlockBuilder.SetPosition(new Vector2(x * 16, y * 16));
            worldBlockBuilder.SetTileId(WorldBuilderHelper.ToTileId(gfxCode));

            var entity = worldBlockBuilder.Build();
            commandsMan.Post(new AddEntityCommand(world.Id, entity.Id));
        }

    }
}
