using OpenBreed.Game.Common.Components;

namespace OpenBreed.Game.Common
{
    public interface IWorldSystem
    {
        #region Public Methods

        void AddComponent(IEntityComponent component);

        void Deinitialize(World world);

        void Initialize(World world);

        void Update(double dt);

        void RemoveComponent(IEntityComponent component);

        #endregion Public Methods
    }
}