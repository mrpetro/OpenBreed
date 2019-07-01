using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public interface ITileSystem : IRenderableSystem
    {
        #region Public Properties

        bool GridVisible { get; set; }

        #endregion Public Properties
    }
}