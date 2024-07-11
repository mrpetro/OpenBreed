using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.TileStamps
{
    public interface IDbTileStamp : IDbEntry
    {
        #region Public Properties

        /// <summary>
        /// Width of this tile stamp
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of this tile stamp
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Stamp center X index coordinate
        /// </summary>
        int CenterX { get; }

        /// <summary>
        /// Stamp center Y index coordinate
        /// </summary>
        int CenterY { get; }

        /// <summary>
        /// Cells of this tile stamp
        /// </summary>
        ReadOnlyCollection<IDbTileStampCell> Cells { get; }

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
        string TsId { get; }

        /// <summary>
        /// Tile atlas tile index for this cell
        /// </summary>
        int TsTi { get; }

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