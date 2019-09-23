namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public interface ITileStamp
    {
        #region Public Properties

        int[] Data { get; }
        int Id { get; }
        int Width { get; }
        int Height { get; }
        int OriginX { get; }
        int OriginY { get; }

        #endregion Public Properties
    }
}