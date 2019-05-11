using OpenBreed.Core.States;
using OpenBreed.Core.Systems;

namespace OpenBreed.Core
{
    public interface ICore
    {
        #region Public Properties

        EntityMan EntityMan { get; }
        StateMan StateMan { get; }

        #endregion Public Properties

        #region Public Methods

        IRenderSystem CreateRenderSystem();

        void Exit();

        ISoundSystem CreateSoundSystem();
        IMovementSystem CreateMovementSystem();
        IAnimationSystem CreateAnimationSystem();
        IPhysicsSystem CreatePhysicsSystem();
        IControlSystem CreateControlSystem();

        void AddViewport(IViewport viewport);

        #endregion Public Methods
    }
}