using OpenBreed.Common.Formats;

namespace OpenBreed.Common.Data
{
    public class DataProvider
    {
        #region Public Constructors

        public DataProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            TileSets = new TileSetsDataProvider(this);
            SpriteSets = new SpriteSetsDataProvider(this);
            ActionSets = new ActionSetsDataProvider(this);
            Maps = new MapsDataProvider(this);
            Assets = new AssetsDataProvider(this);
            Sounds = new SoundsDataProvider(this);
            Images = new ImagesDataProvider(this);
            Palettes = new PalettesDataProvider(this);
            Datas = new Data2DataProvider(this);

            Initialize();
        }

        #endregion Public Constructors

        #region Public Properties

        public ActionSetsDataProvider ActionSets { get; }
        public AssetsDataProvider Assets { get; }
        public DataFormatMan FormatMan { get; } = new DataFormatMan();
        public ImagesDataProvider Images { get; }
        public MapsDataProvider Maps { get; }
        public PalettesDataProvider Palettes { get; }
        public SoundsDataProvider Sounds { get; }
        public SpriteSetsDataProvider SpriteSets { get; }
        public TileSetsDataProvider TileSets { get; }
        public Data2DataProvider Datas { get; }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

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
            FormatMan.RegisterFormat("BINARY", new BinaryFormat());
            FormatMan.RegisterFormat("PCM_SOUND", new PCMSoundFormat());
        }

        public void Save()
        {
            Datas.Save();

            Assets.Save();

            UnitOfWork.Save();
        }




        #endregion Private Methods
    }
}