using System.Collections.ObjectModel;
using System.Drawing;

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
        /// Create viewport using given parameters
        /// </summary>
        /// <param name="x">X screen coordinate</param>
        /// <param name="y">Y screen coordinate</param>
        /// <param name="width">Viewport width</param>
        /// <param name="height">Viewport height</param>
        /// <returns></returns>
        IViewport Create(float x, float y, float width, float height);

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