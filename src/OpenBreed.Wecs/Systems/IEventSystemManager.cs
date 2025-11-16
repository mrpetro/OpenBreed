using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems
{
    public interface IEventSystemManager
    {
        #region Public Methods

        void RegisterSystem(IEventSystem system);

        #endregion Public Methods
    }
}