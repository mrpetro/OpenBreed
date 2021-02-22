using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
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
        /// Commands manager
        /// </summary>
        ICommandsMan Commands { get; }

        /// <summary>
        /// Events manager
        /// </summary>
        IEventsMan Events { get; }

        /// <summary>
        /// Core client instance
        /// </summary>
        ICoreClient Client { get; }

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

        void Load();

        #endregion Public Methods
    }
}