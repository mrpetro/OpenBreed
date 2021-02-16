using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using System;
using System.Collections.Generic;

namespace OpenBreed.Common.Data
{
    public class DataProvider : IDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private readonly ILogger logger;
        private readonly IVariableMan variables;
        private readonly DataSourceProvider dataSources;
        private readonly AssetsDataProvider assets;
        private Dictionary<string, object> _models = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public DataProvider(IWorkspaceMan workspaceMan, ILogger logger, IVariableMan variables, DataFormatMan formatMan)
        {
            this.workspaceMan = workspaceMan;
            this.logger = logger;
            this.variables = variables;

            dataSources = new DataSourceProvider(this.workspaceMan, logger, variables);
            assets = new AssetsDataProvider(this.workspaceMan, dataSources, formatMan);

            Palettes = new PalettesDataProvider(this, this.workspaceMan);
            TileSets = new TileSetsDataProvider(this, this.workspaceMan);
            SpriteSets = new SpriteSetsDataProvider(this, this.workspaceMan);
            ActionSets = new ActionSetsDataProvider(this, this.workspaceMan);
            Maps = new MapsDataProvider(this, this.workspaceMan, TileSets, Palettes, ActionSets);

            Sounds = new SoundsDataProvider(this, this.workspaceMan);
            Images = new ImagesDataProvider(this, this.workspaceMan);
            Texts = new TextsDataProvider(this, this.workspaceMan);
            Scripts = new ScriptsDataProvider(this, this.workspaceMan);
            EntityTemplates = new EntityTemplatesDataProvider(this, this.workspaceMan);
        }

        #endregion Public Constructors

        #region Public Properties

        public ActionSetsDataProvider ActionSets { get; }

        public ImagesDataProvider Images { get; }
        public MapsDataProvider Maps { get; }
        public PalettesDataProvider Palettes { get; }
        public TextsDataProvider Texts { get; }
        public ScriptsDataProvider Scripts { get; }
        public EntityTemplatesDataProvider EntityTemplates { get; }
        public SoundsDataProvider Sounds { get; }
        public SpriteSetsDataProvider SpriteSets { get; }
        public TileSetsDataProvider TileSets { get; }

        #endregion Public Properties

        #region Public Methods

        public bool TryGetData<T>(string id, out T item, out string message)
        {
            var data = GetData<T>(id);

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

        public T GetData<T>(string id)
        {
            object data;

            if (_models.TryGetValue(id, out data))
                return (T)data;

            var asset = assets.GetAsset(id);
            data = asset.Load();
            logger.Verbose($"Model loaded from asset '{asset.Id}'.");

            _models.Add(id, data);

            return (T)data;
        }

        public void Close()
        {
            dataSources.CloseAll();

            logger.Info($"All data closed.");
        }

        public void Save()
        {
            SaveModels();

            dataSources.Save();

            workspaceMan.UnitOfWork.Save();

            logger.Info($"All data saved.");
        }

        #endregion Public Methods

        #region Private Methods

        private void SaveModels()
        {
            foreach (var item in _models)
            {
                var entryId = item.Key;
                var data = item.Value;

                var asset = assets.GetAsset(entryId);

                try
                {
                    asset.Save(data);
                    logger.Verbose($"Model saved to asset '{asset.Id}'.");
                }
                catch (Exception ex)
                {
                    logger.Error($"Problems saving model to asset '{asset.Id}'. Reason: {ex.Message}");
                }
            }

            logger.Info($"All models saved.");
        }

        #endregion Private Methods
    }
}