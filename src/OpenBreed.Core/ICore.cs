using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Systems;
using OpenTK;
using System.Drawing;
using OpenBreed.Core.Components;

namespace OpenBreed.Core
{
    /// <summary>
    /// Engine core interface
    /// </summary>
    public interface ICore
    {
        #region Public Properties

        void Run();

        /// <summary>
        /// Object for logging functionality
        /// </summary>
        ILogger Logging { get; }

        /// <summary>
        /// Jobs manager
        /// </summary>
        JobMan Jobs { get; }

        /// <summary>
        /// Entities manager
        /// </summary>
        IEntityMan Entities { get; }

        //EntityFactory
        EntityFactory EntityFactory { get; }

        /// <summary>
        /// State machine manager
        /// </summary>
        IFsmMan StateMachines { get; }

        /// <summary>
        /// Worlds manager
        /// </summary>
        IWorldMan Worlds { get; }

        /// <summary>
        /// Commands manager
        /// </summary>
        ICommandsMan Commands { get; }

        /// <summary>
        /// Events manager
        /// </summary>
        IEventsMan Events { get; }

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        /// <summary>
        /// Client display rectangle
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        /// Core client instance
        /// </summary>
        ICoreClient Client { get; }

        /// <summary>
        /// Client display aspect ratio
        /// </summary>
        float ClientRatio { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets manager of specific type
        /// </summary>
        /// <typeparam name="TManager">Type of manager to get</typeparam>
        /// <returns>Return manager instance</returns>
        TManager GetManager<TManager>();

        /// <summary>
        /// Get core module of specific type
        /// </summary>
        /// <typeparam name="T">Type of module to get</typeparam>
        /// <returns>Core module</returns>
        T GetModule<T>() where T : ICoreModule;

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        T GetSystemByEntityId<T>(int entityId) where T : IWorldSystem;

        T GetSystemByWorldId<T>(int worldId) where T : IWorldSystem;

        void Update(float dt);

        void Load();

        #endregion Public Methods
    }
}