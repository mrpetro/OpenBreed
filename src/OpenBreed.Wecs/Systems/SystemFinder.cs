using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems
{
    public class SystemFinder : ISystemFinder
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SystemFinder(IEntityMan entityMan,
                            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public T GetSystemByEntityId<T>(int entityId) where T : ISystem
        {
            var entity = entityMan.GetById(entityId);
            if (entity.WorldId == -1)
                return default(T);

            var world = worldMan.GetById(entity.WorldId);

            var system = world.GetSystem<T>();
            if (system == null)
                return default(T);

            return system;
        }

        public T GetSystemByWorldId<T>(int worldId) where T : ISystem
        {
            var world = worldMan.GetById(worldId);
            if (world == null)
                return default(T);
            var system = world.GetSystem<T>();
            if (system == null)
                return default(T);

            return system;
        }

        #endregion Public Methods
    }
}