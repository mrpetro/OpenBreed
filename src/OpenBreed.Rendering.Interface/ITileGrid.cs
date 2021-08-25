namespace OpenBreed.Rendering.Interface
{
    public interface ITileGrid
    {
        #region Public Properties

        int Width { get; }

        int Height { get; }

        int LayersNo { get; }

        int CellSize { get; }

        TileCell[] Cells { get; }

        #endregion Public Properties
    }
}