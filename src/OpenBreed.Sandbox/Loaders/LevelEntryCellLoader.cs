using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Loaders
{
    internal class LevelEntryCellLoader : IMapWorldEntityLoader
    {
        #region Public Fields


        public const int ENTRY_3 = 56;
        public const int ENTRY_1 = 45;
        public const int ENTRY_2 = 46;

        #endregion Public Fields

        #region Private Fields

        private readonly ActorHelper actorHelper;
        private readonly EntriesHelper entriesHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal LevelEntryCellLoader(ActorHelper actorHelper, EntriesHelper entriesHelper)
        {
            this.actorHelper = actorHelper;
            this.entriesHelper = entriesHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Load(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            int entryId;

            switch (actionValue)
            {
                case ENTRY_1:
                    entryId = 0;
                    break;

                case ENTRY_3:
                    entryId = 2;
                    break;

                case ENTRY_2:
                    entryId = 1;
                    break;
                default:
                    throw new NotImplementedException("Entry type not implemented");
            }

            entriesHelper.AddMapEntry(world, ix, iy, entryId, mapAssets.TileAtlasName, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}