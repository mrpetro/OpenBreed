namespace OpenBreed.Wecs.Systems
{
    public interface ISystemFinder
    {
        #region Public Methods

        T GetSystemByEntityId<T>(int entityId) where T : IMatchingSystem;

        T GetSystemByWorldId<T>(int worldId) where T : IMatchingSystem;

        #endregion Public Methods
    }
}