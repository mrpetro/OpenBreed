namespace OpenBreed.Ecsw.Systems
{
    public interface ISystemFinder
    {
        #region Public Methods

        T GetSystemByEntityId<T>(int entityId) where T : ISystem;

        T GetSystemByWorldId<T>(int worldId) where T : ISystem;

        #endregion Public Methods
    }
}