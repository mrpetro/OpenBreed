
namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IRenderableFactory
    {
        #region Public Methods

        IRenderableBatch CreateRenderableBatch();

        IRenderablePalette CreateRenderablePalette();

        #endregion Public Methods
    }
}