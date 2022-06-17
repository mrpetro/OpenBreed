namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Factory for data grids
    /// </summary>
    public interface IDataGridFactory
    {
        #region Public Methods

        /// <summary>
        /// Create data grid of <typeparamref name="TObject"/> type
        /// </summary>
        /// <typeparam name="TObject">Type of data</typeparam>
        /// <param name="width">Width of the grid</param>
        /// <param name="height">Height of the grid</param>
        /// <returns>Data grid</returns>
        IDataGrid<TObject> Create<TObject>(int width, int height);

        #endregion Public Methods
    }
}