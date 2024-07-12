using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayerModel
    {
        #region Public Fields

        public const int UNSET_CELL = -1;

        #endregion Public Fields

        #region Private Fields

        private readonly int[] cellValues;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerModel(MapLayerBuilder builder)
        {
            cellValues = builder.CellValues.ToArray();
            LayerType = builder.LayerType;
            Width = builder.Width;
            Height = builder.Height;
            IsVisible = builder.IsVisible;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Height { get; }

        /// <summary>
        /// NOTE: Editor specific property
        /// </summary>
        public bool IsVisible { get; set; }

        public MapLayerType LayerType { get; }
        public int Width { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Get Layer cell using single index
        /// </summary>
        /// <param name="cellIndex">Index of cell from list</param>
        /// <returns>Tile reference object</returns>
        internal int GetCellValue(int cellIndex)
        {
            return cellValues[cellIndex];
        }

        internal void SetCellValue(int cellIndex, int value)
        {
            if (cellValues[cellIndex] == value)
                return;

            cellValues[cellIndex] = value;
        }

        #endregion Public Methods
    }
}