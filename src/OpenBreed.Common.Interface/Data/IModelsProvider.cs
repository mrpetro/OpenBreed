using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;

namespace OpenBreed.Common.Interface.Data
{
    public interface IModelsProvider
    {
        #region Public Methods

        bool TryGetModel<TModel>(string id, out TModel item, out string message);

        bool TryGetModel<TDbEntry, TModel>(TDbEntry dbEntry, out TModel item, out string message) where TDbEntry : IDbEntry;

        TModel GetModel<TModel>(string id);

        TModel GetModel<TDbEntry, TModel>(TDbEntry dbEntry, bool refresh = false) where TDbEntry : IDbEntry;

        TModel GetModelById<TDbEntry, TModel>(string entryId) where TDbEntry : IDbEntry;

        #endregion Public Methods
    }
}