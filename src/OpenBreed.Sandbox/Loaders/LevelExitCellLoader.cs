using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    internal class LevelExitCellLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        #endregion Public Fields

        #region Private Fields

        private readonly ActorHelper actorHelper;
        private readonly EntriesHelper entriesHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal LevelExitCellLoader(ActorHelper actorHelper, EntriesHelper entriesHelper)
        {
            this.actorHelper = actorHelper;
            this.entriesHelper = entriesHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            var missonBlock = map.Blocks.OfType<MapMissionBlock>().FirstOrDefault();

            int exitId;

            switch (templateName)
            {
                case "MapExit1":
                    exitId = missonBlock.EXC1;
                    break;
                case "MapExit2":
                    exitId = missonBlock.EXC2;
                    break;
                case "MapExit3":
                    exitId = missonBlock.EXC3;
                    break;
                default:
                    throw new NotImplementedException("Exit type not implemented");
            }

            entriesHelper.AddMapExit(world, ix, iy, exitId, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}