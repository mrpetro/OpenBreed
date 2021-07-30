namespace OpenBreed.Rendering.Interface
{
    public interface ITileStampCell
    {
        int AtlasId { get; }
        int ImageId { get; }
    }

    public interface ITileStamp
    {
        #region Public Properties

        ITileStampCell[] Cells { get; }
        int Id { get; }
        int Width { get; }
        int Height { get; }
        int OriginX { get; }
        int OriginY { get; }

        #endregion Public Properties
    }
}