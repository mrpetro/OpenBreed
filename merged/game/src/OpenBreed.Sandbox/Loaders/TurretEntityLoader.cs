using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Sandbox.Loaders
{
    internal class TurretEntryLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        #endregion Public Fields

        #region Private Fields

        private readonly ActorHelper actorHelper;
        private readonly EntriesHelper entriesHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal TurretEntryLoader(ActorHelper actorHelper, EntriesHelper entriesHelper)
        {
            this.actorHelper = actorHelper;
            this.entriesHelper = entriesHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapMapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var entity = actorHelper.AddHeavyTurret(world, (ix + 1) * 16 + 8 , iy * 16 - 8);
            visited[ix, iy] = true;
            return entity;
        }

        #endregion Public Methods
    }
}
