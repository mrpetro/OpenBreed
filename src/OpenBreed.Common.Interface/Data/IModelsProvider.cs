using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;

namespace OpenBreed.Common.Interface.Data
{
    public interface IModelsProvider
    {
        #region Public Methods

        bool TryGetModel<T>(string id, out T item, out string message);
        T GetModel<T>(string id);
        #endregion Public Methods
    }
}