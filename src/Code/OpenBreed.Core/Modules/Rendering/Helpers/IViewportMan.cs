using System.Collections.ObjectModel;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Viewport manager interface which handles render module viewports 
    /// </summary>
    public interface IViewportMan
    {
        #region Public Properties

        /// <summary>
        /// Read-only collection of viewports
        /// </summary>
        ReadOnlyCollection<IViewport> Items { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Add viewport given as parameter to render module
        /// </summary>
        /// <param name="viewport">Viewport to add</param>
        void Add(IViewport viewport);

        /// <summary>
        /// Remove viewport given as parameter to render module
        /// </summary>
        /// <param name="viewport">Viewport to remove</param>
        void Remove(IViewport viewport);

        #endregion Public Methods
    }
}