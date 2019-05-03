using OpenBreed.Game.Common.Components;

namespace OpenBreed.Game.Common
{
    public interface IWorldSystem
    {
        #region Public Methods

        void AddComponent(IEntityComponent component);

        void RemoveComponent(IEntityComponent component);

        void Deinitialize(World world);

        void Initialize(World world);

        void Update(float dt);

        #endregion Public Methods
    }
}