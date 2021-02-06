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

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IVariableMan variables;
        private Dictionary<string, object> _models = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public DataProvider(IUnitOfWork unitOfWork, ILogger logger, IVariableMan variables, DataFormatMan formatMan)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.variables = variables;

            DataSources = new DataSourceProvider(this, unitOfWork, logger, variables);
            Palettes = new PalettesDataProvider(this, unitOfWork);
            TileSets = new TileSetsDataProvider(this, unitOfWork);
            SpriteSets = new SpriteSetsDataProvider(this, unitOfWork);
            ActionSets = new ActionSetsDataProvider(this, unitOfWork);
            Maps = new MapsDataProvider(this, unitOfWork, TileSets, Palettes, ActionSets);
            Assets = new AssetsDataProvider(unitOfWork, DataSources, formatMan);
            Sounds = new SoundsDataProvider(this, unitOfWork);
            Images = new ImagesDataProvider(this, unitOfWork);
            Texts = new TextsDataProvider(this, unitOfWork);
            Scripts = new ScriptsDataProvider(this, unitOfWork);
            EntityTemplates = new EntityTemplatesDataProvider(this, unitOfWork);
        }

        #endregion Public Constructors

        #region Public Properties

        public DataSourceProvider DataSources { get; }
        public ActionSetsDataProvider ActionSets { get; }
        public AssetsDataProvider Assets { get; }
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

            var asset = Assets.GetAsset(id);
            data = asset.Load();
            logger.Verbose($"Model loaded from asset '{asset.Id}'.");

            _models.Add(id, data);

            return (T)data;
        }

        public void Close()
        {
            DataSources.CloseAll();

            logger.Info($"All data closed.");
        }

        public void Save()
        {
            SaveModels();

            DataSources.Save();

            unitOfWork.Save();

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

                var asset = Assets.GetAsset(entryId);

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