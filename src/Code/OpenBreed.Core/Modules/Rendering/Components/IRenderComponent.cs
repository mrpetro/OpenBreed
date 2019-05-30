using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Component interface which is dedicated for graphics rendering
    /// </summary>
    public interface IRenderComponent : IEntityComponent
    {
        #region Public Methods

        /// <summary>
        /// Draw this component to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this component will be rendered to</param>
        void Draw(IViewport viewport);

        #endregion Public Methods
    }
}