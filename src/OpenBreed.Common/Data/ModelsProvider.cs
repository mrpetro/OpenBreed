using OpenBreed.Common.Formats;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
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

        public bool TryGetModel<T>(string id, out T item, out string message)
        {
            var data = GetModel<T>(id);

            if (data == null)
            {
                item = default(T);
                message = $"No asset with ID '{id}' found.";
                return false;
            }

            if (data is T)
            {
                item = (T)data;
                message = null;
                return true;
            }

            item = default(T);
            message = $"Asset with ID '{id}' is not of type '{typeof(T)}'.";
            return false;
        }

        public T GetModel<T>(string id)
        {
            object data;

            if (loadedModels.TryGetValue(id, out data))
                return (T)data;

            data = assets.LoadModel(id);

            logger.Verbose($"Model loaded from asset '{id}'.");

            loadedModels.Add(id, data);

            return (T)data;
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
                    logger.Verbose($"Model saved to asset '{entryId}'.");
                }
                catch (Exception ex)
                {
                    logger.Error($"Problems saving model to asset '{entryId}'. Reason: {ex.Message}");
                }
            }

            logger.Info($"All models saved.");
        }

        #endregion Public Methods
    }
}