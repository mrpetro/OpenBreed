using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    internal class LevelExitCellLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int EXIT_1 = 54;
        public const int EXIT_2 = 37;
        public const int EXIT_3 = 38;

        #endregion Public Fields

        #region Private Fields

        private readonly ActorHelper actorHelper;
        private readonly WorldGateHelper worldGateHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal LevelExitCellLoader(ActorHelper actorHelper, WorldGateHelper worldGateHelper)
        {
            this.actorHelper = actorHelper;
            this.worldGateHelper = worldGateHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Load(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            var missonBlock = map.Blocks.OfType<MapMissionBlock>().FirstOrDefault();

            int exitId;

            switch (actionValue)
            {
                case EXIT_1:
                    exitId = missonBlock.EXC1;
                    break;
                case EXIT_2:
                    exitId = missonBlock.EXC2;
                    break;
                case EXIT_3:
                    exitId = missonBlock.EXC3;
                    break;
                default:
                    throw new NotImplementedException("Exit type not implemented");
            }

            worldGateHelper.AddWorldExit(world, ix, iy, exitId, mapAssets.AtlasId, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}