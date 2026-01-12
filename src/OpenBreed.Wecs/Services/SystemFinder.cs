namespace OpenBreed.Wecs.Services
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

        public T GetSystemByEntityId<T>(int entityId) where T : IMatchingSystem
        {
            var entity = entityMan.GetById(entityId);
            if (entity.WorldId == -1)
                return default;

            var world = worldMan.GetById(entity.WorldId);

            var system = world.GetSystem<T>();
            if (system == null)
                return default;

            return system;
        }

        public T GetSystemByWorldId<T>(int worldId) where T : IMatchingSystem
        {
            var world = worldMan.GetById(worldId);
            if (world == null)
                return default;
            var system = world.GetSystem<T>();
            if (system == null)
                return default;

            return system;
        }

        #endregion Public Methods
    }
}