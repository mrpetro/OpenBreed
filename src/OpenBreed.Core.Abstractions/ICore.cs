using OpenBreed.Common.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenTK;
using System.Drawing;

namespace OpenBreed.Core.Abstractions
{
    /// <summary>
    /// Engine core interface
    /// </summary>
    public interface ICore
    {
        #region Public Properties

        void Run();

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Perform exit
        /// </summary>
        void Exit();

        #endregion Public Methods
    }
}