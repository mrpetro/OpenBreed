using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.TileStamps
{
    public interface IDbTileStamp : IDbEntry
    {
        #region Public Properties

        /// <summary>
        /// Width of this tile stamp
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Height of this tile stamp
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Stamp center X index coordinate
        /// </summary>
        int CenterX { get; set; }

        /// <summary>
        /// Stamp center Y index coordinate
        /// </summary>
        int CenterY { get; set; }

        /// <summary>
        /// Cells of this tile stamp
        /// </summary>
        ReadOnlyCollection<IDbTileStampCell> Cells { get; }

        IDbTileStampCell AddNewCell(int x, int y);

        bool RemoveCell(IDbTileStampCell cell);

        #endregion Public Properties
    }

    public interface IDbTileStampCell
    {
        #region Public Properties

        /// <summary>
        /// X index coordinate of this cell
        /// </summary>
        int X { get; }

        /// <summary>
        /// Y index coordinate of this cell
        /// </summary>
        int Y { get; }

        /// <summary>
        /// ID of tile atlas for this cell
        /// </summary>
        string TsId { get; set; }

        /// <summary>
        /// Tile atlas tile index for this cell
        /// </summary>
        int TsTi { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Copy this object
        /// </summary>
        /// <returns>Copy of this object</returns>
        IDbTileStampCell Copy();

        #endregion Public Methods
    }
}