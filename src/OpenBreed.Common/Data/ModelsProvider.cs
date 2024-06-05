using Microsoft.Extensions.Logging;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Common.Data
{
    public class ModelsProvider : IModelsProvider
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly AssetsDataProvider assets;
        private Dictionary<string, object> loadedModels = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public ModelsProvider(ILogger logger, AssetsDataProvider assets)
        {
            this.logger = logger;
            this.assets = assets;
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public bool TryGetModel<TDbEntry, TModel>(TDbEntry dbEntry, out TModel item, out string message) where TDbEntry : IDbEntry
        {
            var data = GetModel<TDbEntry, TModel>(dbEntry);

            if (data is null)
            {
                item = default(TModel);
                message = $"No asset with ID '{dbEntry.Id}' found.";
                return false;
            }

            if (data is TModel)
            {
                item = (TModel)data;
                message = null;
                return true;
            }

            item = default(TModel);
            message = $"Asset with ID '{dbEntry.Id}' is not of type '{typeof(TModel)}'.";
            return false;
        }

        public TModel GetModel<TDbEntry, TModel>(TDbEntry dbEntry) where TDbEntry : IDbEntry
        {
            object data;

            if (loadedModels.TryGetValue(dbEntry.Id, out data))
                return (TModel)data;

            data = assets.LoadModel<TDbEntry>(dbEntry);

            logger.LogTrace($"Model loaded from dbEntry '{dbEntry.Id}'.");

            loadedModels.Add(dbEntry.Id, data);

            return (TModel)data;
        }

        public bool TryGetModel<TModel>(string id, out TModel item, out string message)
        {
            var data = GetModel<TModel>(id);

            if (data == null)
            {
                item = default(TModel);
                message = $"No asset with ID '{id}' found.";
                return false;
            }

            if (data is TModel)
            {
                item = (TModel)data;
                message = null;
                return true;
            }

            item = default(TModel);
            message = $"Asset with ID '{id}' is not of type '{typeof(TModel)}'.";
            return false;
        }

        public TModel GetModel<TModel>(string id)
        {
            object data;

            if (loadedModels.TryGetValue(id, out data))
                return (TModel)data;

            data = assets.LoadModel(id);

            logger.LogTrace($"Model loaded from asset '{id}'.");

            loadedModels.Add(id, data);

            return (TModel)data;
        }

        public void Save()
        {
            foreach (var item in loadedModels)
            {
                var entryId = item.Key;
                var data = item.Value;

                try
                {
                    assets.SaveModel(entryId, data);
                    logger.LogTrace($"Model saved to asset '{entryId}'.");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Problems saving model to asset '{entryId}'. Reason: {ex.Message}");
                }
            }

            logger.LogInformation($"All models saved.");
        }

        #endregion Public Methods
    }
}