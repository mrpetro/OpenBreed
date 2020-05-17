using System.Drawing;
using System.Reflection.Emit;
using NLua;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
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
        /// Reference to animation manager
        /// </summary>
        AnimMan Animations { get; }

        /// <summary>
        /// Logging manager
        /// </summary>
        ILogMan Logging { get; }

        /// <summary>
        /// Jobs manager
        /// </summary>
        JobMan Jobs { get; }

        /// <summary>
        /// Entities manager
        /// </summary>
        EntityMan Entities { get; }

        /// <summary>
        /// State machine manager
        /// </summary>
        FsmMan StateMachines { get; }

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
        /// Commands manager
        /// </summary>
        CommandsMan Commands{ get; }

        /// <summary>
        /// Events manager
        /// </summary>
        EventsMan Events { get; }

        /// <summary>
        /// Scripts manager
        /// </summary>
        IScriptMan Scripts { get; }

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

        /// <summary>
        /// Get core module of specific type
        /// </summary>
        /// <typeparam name="TModule">Type of module to get</typeparam>
        /// <returns>Core module</returns>
        TModule GetModule<TModule>() where TModule : ICoreModule;

        /// <summary>
        /// Get component builder
        /// </summary>
        /// <typeparam name="TBuilder">Component builder type to get</typeparam>
        /// <returns>Component builder of specified type</returns>
        TBuilder GetBuilder<TBuilder>() where TBuilder : IComponentBuilder;

        #endregion Public Properties

            #region Public Methods

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        #endregion Public Methods
    }
}