using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets manager of specific type
        /// </summary>
        /// <typeparam name="TManager">Type of manager to get</typeparam>
        /// <returns>Return manager instance</returns>
        TManager GetManager<TManager>();

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        void Load();

        #endregion Public Methods
    }
}