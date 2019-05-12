using OpenBreed.Core.States;
using OpenBreed.Core.Systems;
using OpenTK;

namespace OpenBreed.Core
{
    public interface ICore
    {
        #region Public Properties

        EntityMan Entities { get; }
        ViewportMan Viewports { get; }
        WorldMan Worlds { get; }
        StateMan States { get; }
        Vector2 CursorPos { get; }

        #endregion Public Properties

        #region Public Methods

        IRenderSystem CreateRenderSystem();

        void Exit();

        ISoundSystem CreateSoundSystem();
        IMovementSystem CreateMovementSystem();
        IAnimationSystem CreateAnimationSystem();
        IPhysicsSystem CreatePhysicsSystem();
        IControlSystem CreateControlSystem();

        #endregion Public Methods
    }
}