using OpenBreed.Core.States;
using OpenBreed.Core.Systems;
using OpenTK;

namespace OpenBreed.Core
{
    /// <summary>
    /// Engine core interface
    /// </summary>
    public interface ICore
    {
        #region Public Properties

        /// <summary>
        /// Entities manager
        /// </summary>
        EntityMan Entities { get; }

        /// <summary>
        /// Viewports manager
        /// </summary>
        ViewportMan Viewports { get; }

        /// <summary>
        /// Worlds manager
        /// </summary>
        WorldMan Worlds { get; }

        /// <summary>
        /// States manager
        /// </summary>
        StateMan States { get; }

        /// <summary>
        /// Gets current cursor position in window coordinates
        /// </summary>
        Vector2 CursorPos { get; }

        /// <summary>
        /// Gets cursor position delta (difference between current and previous)
        /// </summary>
        Vector2 CursorDelta { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates render system and return it
        /// </summary>
        /// <returns>Render system interface</returns>
        IRenderSystem CreateRenderSystem();

        /// <summary>
        /// Creates sound system and return it
        /// </summary>
        /// <returns>Sound system interface</returns>
        ISoundSystem CreateSoundSystem();

        /// <summary>
        /// Creates movement system and return it
        /// </summary>
        /// <returns>Movement system interface</returns>
        IMovementSystem CreateMovementSystem();

        /// <summary>
        /// Creates animation system and return it
        /// </summary>
        /// <returns>Animation system interface</returns>
        IAnimationSystem CreateAnimationSystem();

        /// <summary>
        /// Creates Physics system and return it
        /// </summary>
        /// <returns>Physics system interface</returns>
        IPhysicsSystem CreatePhysicsSystem();

        /// <summary>
        /// Creates control system and return it
        /// </summary>
        /// <returns>Control system interface</returns>
        IControlSystem CreateControlSystem();

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        #endregion Public Methods
    }
}