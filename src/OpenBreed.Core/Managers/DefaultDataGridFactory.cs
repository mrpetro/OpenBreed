using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using System;

namespace OpenBreed.Core.Managers
{
    internal class DefaultDataGridFactory : IDataGridFactory
    {
        #region Public Methods

        public IDataGrid<TObject> Create<TObject>(int width, int height, Func<int, int, TObject> cellInitializer = null)
        {
            if (cellInitializer is null)
            {
                cellInitializer = (x, y) => default(TObject);
            }

            return new DefaultDataGrid<TObject>(width, height, cellInitializer);
        }

        #endregion Public Methods
    }
}