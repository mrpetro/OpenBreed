using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Systems.Rendering.Helpers;

namespace OpenBreed.Core.Systems.Rendering.Components
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
        void Draw(Viewport viewport);

        #endregion Public Methods
    }
}