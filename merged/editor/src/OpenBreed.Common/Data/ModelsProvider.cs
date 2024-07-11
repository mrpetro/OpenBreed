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
        private readonly Dictionary<Type, Dictionary<string, object>> loadedModels = new Dictionary<Type, Dictionary<string, object>>();

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

        private bool TryGetLoadedModel(Type entryType, string entryId, out object data)
        {
            if (!loadedModels.TryGetValue(entryType, out Dictionary<string, object> typeLoadedModels))
            {
                data = null;
                return false;
            }

            return typeLoadedModels.TryGetValue(entryId, out data);
        }

        public TModel GetModelById<TDbEntry, TModel>(string entryId) where TDbEntry : IDbEntry
        {
            object data;

            var entryType = typeof(TDbEntry);

            if (loadedModels.TryGetValue(entryType, out Dictionary<string, object> typeLoadedModels))
            {
                if (typeLoadedModels.TryGetValue(entryId, out data))
                {
                    return (TModel)data;
                }
            }

            data = assets.LoadModel<TDbEntry>(entryId);

            logger.LogTrace($"Model loaded from asset '{entryId}'.");

            if (typeLoadedModels is null)
            {
                typeLoadedModels = new Dictionary<string, object>();
                loadedModels.Add(entryType, typeLoadedModels);
            }

            typeLoadedModels.Add(entryId, data);

            return (TModel)data;
        }

        public TModel GetModel<TDbEntry, TModel>(TDbEntry dbEntry, bool refresh = false) where TDbEntry : IDbEntry
        {
            object data;

            var entryType = typeof(TDbEntry);

            if (!loadedModels.TryGetValue(entryType, out Dictionary<string, object> typeLoadedModels))
            {
                typeLoadedModels = new Dictionary<string, object>();
                loadedModels.Add(entryType, typeLoadedModels);
            }

            if (refresh)
            {
                typeLoadedModels.Remove(dbEntry.Id);
            }

            if (typeLoadedModels.TryGetValue(dbEntry.Id, out data))
            {
                return (TModel)data;
            }

            data = assets.LoadModel<TDbEntry>(dbEntry);

            logger.LogTrace($"Model loaded from dbEntry '{dbEntry.Id}'.");

            typeLoadedModels.Add(dbEntry.Id, data);

            return (TModel)data;
        }

        public void Save()
        {
            foreach (var item in loadedModels)
            {
                var entryType = item.Key;

                foreach (var subItem in item.Value)
                {
                    var entryId = subItem.Key;
                    var data = subItem.Value;

                    try
                    {
                        assets.SaveModel(entryType, entryId, data);
                        logger.LogTrace($"Model saved to asset '{entryId}'.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Problems saving model to asset '{entryId}'. Reason: {ex.Message}");
                    }
                }
            }

            logger.LogInformation($"All models saved.");
        }

        #endregion Public Methods
    }
}