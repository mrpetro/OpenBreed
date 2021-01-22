using OpenBreed.Ecsw.Systems;

namespace OpenBreed.Ecsw
{
    public class SystemFinder : ISystemFinder
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public SystemFinder(IEntityMan entityMan,
                            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public T GetSystemByEntityId<T>(int entityId) where T : IWorldSystem
        {
            var entity = entityMan.GetById(entityId);
            if (entity.World == null)
                return default(T);
            var system = entity.World.GetSystem<T>();
            if (system == null)
                return default(T);

            return system;
        }

        public T GetSystemByWorldId<T>(int worldId) where T : IWorldSystem
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