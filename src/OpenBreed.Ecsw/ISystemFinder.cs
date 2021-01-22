using OpenBreed.Ecsw.Systems;

namespace OpenBreed.Ecsw
{
    public interface ISystemFinder
    {
        #region Public Methods

        T GetSystemByEntityId<T>(int entityId) where T : IWorldSystem;

        T GetSystemByWorldId<T>(int worldId) where T : IWorldSystem;

        #endregion Public Methods
    }
}