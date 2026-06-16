using OpenTK.Mathematics;
using System;

namespace OpenBreed.Core.Abstractions
{
    /// <summary>
    /// Interface for operating on data grid of generic type
    /// </summary>
    /// <typeparam name="TObject">Type of data to hold</typeparam>
    public interface IDataGrid<TObject>
    {
        #region Public Properties

        /// <summary>
        /// Width of this data grid
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Height of this data gird
        /// </summary>
        int Width { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Check if  index coordinates are not out of grid bounds.
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <returns>True if given index coordinates are in bounds of this grid, false otherwise.</returns>
        bool IsValid(Vector2i pos);

        /// <summary>
        /// Try to get data from given index coordinates
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <param name="value">Resulting data in case of valid coordinates.</param>
        /// <returns>True if given index coordinates are in bounds of this grid, false otherwise.</returns>
        bool TryGet(Vector2i pos, out TObject value);

        /// <summary>
        /// Get data from given index coordinates
        /// </summary>
        /// <param name="pos">Grid cell index coordinates</param>
        /// <returns>Resulting data</returns>
        TObject Get(Vector2i pos);

        /// <summary>
        /// Get data from given id
        /// </summary>
        /// <param name="id">Id of grid cell</param>
        /// <returns>Resulting data</returns>
        TObject Get(int id);

        /// <summary>
        /// Gets the grid coordinates identifier.
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <returns>Identifier</returns>
        int GetId(Vector2i pos);

        /// <summary>
        /// Set data under given index coordinates
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <param name="data">Data to set</param>
        void Set(Vector2i pos, TObject data);

        /// <summary>
        /// Set data using given cell action.
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <param name="data">Action to perform</param>
        void Set(Vector2i pos, Action<TObject> cellAction);

        /// <summary>
        /// Clears all data to initialization state.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Sets all data using same cell action.
        /// </summary>
        /// <param name="cellAction">Action to perform on all cells.</param>
        void SetAll(Action<TObject> cellAction);

        Vector2i GetPosition(int id);

        bool TryGetIdByOffset(int id, Vector2i offset, out int resultId);
            
        #endregion Public Methods
    }
}