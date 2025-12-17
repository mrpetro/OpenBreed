namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IEventSystemManager
    {
        #region Public Methods

        void RegisterSystem(IEventSystem system);

        #endregion Public Methods
    }
}