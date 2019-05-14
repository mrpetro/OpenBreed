﻿using OpenBreed.Core.Modules;
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
        /// Reference to rendering core module
        /// </summary>
        IRenderModule Rendering { get; }

        /// <summary>
        /// Reference to sounds core module
        /// </summary>
        ISoundModule Sounds { get; }

        /// <summary>
        /// Reference to physics core module
        /// </summary>
        IPhysicsModule Physics { get; }

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