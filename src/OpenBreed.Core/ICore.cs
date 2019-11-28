using System.Drawing;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Inputs;
using OpenBreed.Core.Managers;
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
        /// Logging manager
        /// </summary>
        ILogMan Logging { get; }

        /// <summary>
        /// Blueprints manager
        /// </summary>
        BlueprintMan Blueprints { get; }

        /// <summary>
        /// Jobs manager
        /// </summary>
        JobMan Jobs { get; }

        /// <summary>
        /// Entities manager
        /// </summary>
        EntityMan Entities { get; }

        /// <summary>
        /// Players manager
        /// </summary>
        PlayersMan Players { get; }

        /// <summary>
        /// Items manager
        /// </summary>
        ItemsMan Items { get; }

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
        /// Event bus
        /// </summary>
        CoreEventBus EventBus { get; }

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        /// <summary>
        /// Client display rectangle
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        /// Client display aspect ratio
        /// </summary>
        float ClientRatio { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        #endregion Public Methods
    }
}