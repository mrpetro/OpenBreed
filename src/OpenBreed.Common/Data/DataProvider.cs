using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Common.Data
{
    public class DataProvider : IDataProvider
    {
        #region Private Fields

        private readonly IUnitOfWork unitOfWork;
        private Dictionary<string, object> _models = new Dictionary<string, object>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public DataProvider(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;

            DataSources = new DataSourceProvider(this);
            TileSets = new TileSetsDataProvider(this);
            SpriteSets = new SpriteSetsDataProvider(this);
            ActionSets = new ActionSetsDataProvider(this);
            Maps = new MapsDataProvider(this);
            Assets = new AssetsDataProvider(this);
            Sounds = new SoundsDataProvider(this);
            Images = new ImagesDataProvider(this);
            Palettes = new PalettesDataProvider(this);
            Texts = new TextsDataProvider(this);
            Scripts = new ScriptsDataProvider(this);
            EntityTemplates = new EntityTemplatesDataProvider(this);
            Initialize();
        }

        #endregion Public Constructors

        #region Public Properties

        public DataSourceProvider DataSources { get; }
        public ActionSetsDataProvider ActionSets { get; }
        public AssetsDataProvider Assets { get; }
        public DataFormatMan FormatMan { get; } = new DataFormatMan();
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

        public IRepository<T> GetRepository<T>() where T : IEntry
        {
            return unitOfWork.GetRepository<T>();
        }

        public IRepository GetRepository(Type entryType)
        {
            return unitOfWork.GetRepository(entryType);
        }

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
            _models.Add(id, data);

            return (T)data;
        }

        public void Save()
        {
            SaveModels();

            DataSources.Save();

            unitOfWork.Save();
        }

        #endregion Public Methods

        #region Private Methods

        private void Initialize()
        {
            FormatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat());
            FormatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat());
            FormatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat());
            FormatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
            FormatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
            FormatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
            FormatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat());
            FormatMan.RegisterFormat("IFF_IMAGE", new IFFImageFormat());
            FormatMan.RegisterFormat("BINARY", new BinaryFormat());
            FormatMan.RegisterFormat("PCM_SOUND", new PCMSoundFormat());
            FormatMan.RegisterFormat("TEXT", new TextFormat());
        }

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
                }
                catch (Exception ex)
                {
                    logger.Error($"Problems saving model to asset '{asset.Id}'. Reason: {ex.Message}");
                }
            }
        }

        #endregion Private Methods
    }
}