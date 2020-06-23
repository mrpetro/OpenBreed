using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenTK;
using System.Drawing;

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
        CommandsMan Commands { get; }

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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Get core module of specific type
        /// </summary>
        /// <typeparam name="T">Type of module to get</typeparam>
        /// <returns>Core module</returns>
        T GetModule<T>() where T : ICoreModule;

        /// <summary>
        /// Get component builder
        /// </summary>
        /// <typeparam name="TBuilder">Component builder type to get</typeparam>
        /// <returns>Component builder of specified type</returns>
        TBuilder GetBuilder<TBuilder>() where TBuilder : IComponentBuilder;

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        bool HandleCmd(IMsg msg);

        #endregion Public Methods
    }
}