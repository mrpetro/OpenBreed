﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
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
        IAudioModule Sounds { get; }

        /// <summary>
        /// Reference to physics core module
        /// </summary>
        IPhysicsModule Physics { get; }

        /// <summary>
        /// Reference to animation core module
        /// </summary>
        IAnimationModule Animations { get; }

        /// <summary>
        /// Create group system
        /// </summary>
        /// <returns>Group system</returns>
        GroupSystem CreateGroupSystem();

        /// <summary>
        /// Entities manager
        /// </summary>
        EntityMan Entities { get; }

        /// <summary>
        /// Inputs manager
        /// </summary>
        InputsMan Inputs { get; }

        /// <summary>
        /// Worlds manager
        /// </summary>
        WorldMan Worlds { get; }

        /// <summary>
        /// States machine
        /// </summary>
        StateMan StateMachine { get; }

        /// <summary>
        /// Message bus
        /// </summary>
        CoreMessageBus MessageBus{ get; }

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        #endregion Public Methods
    }
}