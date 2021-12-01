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

        public void Load(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            int entryId;

            switch (templateName)
            {
                case "MapEntry1":
                    entryId = 0;
                    break;

                case "MapEntry3":
                    entryId = 2;
                    break;

                case "MapEntry2":
                    entryId = 1;
                    break;
                default:
                    throw new NotImplementedException("Entry type not implemented");
            }

            entriesHelper.AddMapEntry(world, ix, iy, entryId, mapAssets.Level, gfxValue);
            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}