namespace OpenBreed.Rendering.Abstractions
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
        string Name { get; }
        int Width { get; }
        int Height { get; }
        int OriginX { get; }
        int OriginY { get; }

        #endregion Public Properties
    }
}