using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Managers;

namespace OpenBreed.Core.Managers
{
    internal class DefaultDataGridFactory : IDataGridFactory
    {
        #region Public Methods

        public IDataGrid<TObject> Create<TObject>(int width, int height)
        {
            return new DefaultDataGrid<TObject>(width, height);
        }

        #endregion Public Methods
    }
}