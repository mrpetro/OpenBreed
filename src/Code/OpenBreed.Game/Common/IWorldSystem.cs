using OpenBreed.Game.Entities.Components;

namespace OpenBreed.Game.Common
{
    public interface IWorldSystem
    {
        #region Public Methods

        void AddComponent(IEntityComponent component);

        void Deinitialize(World world);

        void Initialize(World world);

        void RemoveComponent(IEntityComponent component);

        #endregion Public Methods
    }
}