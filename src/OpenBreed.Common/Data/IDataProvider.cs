using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;

namespace OpenBreed.Common.Data
{
    public interface IDataProvider
    {
        #region Public Methods

        bool TryGetData<T>(string id, out T item, out string message);
        T GetData<T>(string id);
        #endregion Public Methods
    }
}