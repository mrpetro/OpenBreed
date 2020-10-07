using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayerModel
    {
        #region Private Fields

        private readonly int[] cellValues;

        public MapLayerType LayerType { get; }

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerModel(MapLayerBuilder builder)
        {
            cellValues = builder.CellValues.ToArray();
            LayerType = builder.LayerType;
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Get Layer cell using single index
        /// </summary>
        /// <param name="cellIndex">Index of cell from list</param>
        /// <returns>Tile reference object</returns>
        public int GetCellValue(int cellIndex)
        {
            return cellValues[cellIndex];
        }

        public void SetCellValue(int cellIndex, int value)
        {
            if (cellValues[cellIndex] == value)
                return;

            cellValues[cellIndex] = value;
        }

        #endregion Public Methods
    }
}