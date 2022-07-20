using OpenTK.Mathematics;

namespace OpenBreed.Core
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
        /// Get data from given index coordinates
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <returns>Resulting data</returns>
        TObject Get(Vector2i pos);

        /// <summary>
        /// Set data under given index coordinates
        /// </summary>
        /// <param name="pos">Index coordinates</param>
        /// <param name="data">Data to set</param>
        void Set(Vector2i pos, TObject data);

        #endregion Public Methods
    }
}