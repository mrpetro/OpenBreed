using OpenBreed.Common.Formats;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class DataProvider
    {
        public AssetsDataProvider AssetsProvider { get; }

        #region Private Fields

        private IUnitOfWork _unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public DataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            AssetsProvider = new AssetsDataProvider(_unitOfWork);

            Initialize();
        }

        #endregion Public Constructors

        #region Public Properties

        public DataFormatMan FormatMan { get; } = new DataFormatMan();

        #endregion Public Properties

        #region Public Methods

        public Image GetImage(string name)
        {
            var entry = _unitOfWork.GetRepository<IImageEntry>().GetById(name);
            if (entry == null)
                throw new Exception("Image error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            return FormatMan.Load(asset, entry.Format) as Image;
        }

        public PaletteModel GetPalette(string name)
        {
            var entry = _unitOfWork.GetRepository<IPaletteEntry>().GetById(name);
            if (entry == null)
                throw new Exception("Palette error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            return FormatMan.Load(asset, entry.Format) as PaletteModel;
        }

        public SoundModel GetSound(string name)
        {
            var entry = _unitOfWork.GetRepository<ISoundEntry>().GetById(name);
            if (entry == null)
                throw new Exception("Sound error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            return FormatMan.Load(asset, entry.Format) as SoundModel;
        }

        public IPropSetEntry GetPropSet(string name)
        {
            var entry = _unitOfWork.GetRepository<IPropSetEntry>().GetById(name);
            if (entry == null)
                throw new Exception("PropSet error: " + name);

            return entry;
        }

        public AssetBase GetSource(string name)
        {
            return null;
        }

        public SpriteSetModel GetSpriteSet(string name)
        {
            var entry = _unitOfWork.GetRepository<ISpriteSetEntry>().GetById(name);
            if (entry == null)
                throw new Exception("SpriteSet error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            return FormatMan.Load(asset, entry.Format) as SpriteSetModel;
        }

        public TileSetModel GetTileSet(string name)
        {
            var entry = _unitOfWork.GetRepository<ITileSetEntry>().GetById(name);
            if (entry == null)
                throw new Exception("TileSet error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            return FormatMan.Load(asset, entry.Format) as TileSetModel;
        }

        public LevelModel GetLevel(string name)
        {
            var entry = _unitOfWork.GetRepository<IMapEntry>().GetById(name);
            if (entry == null)
                throw new Exception("Level error: " + name);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var level = new LevelModel();
            level.Map = FormatMan.Load(asset, entry.Format) as MapModel;


            if (entry.TileSetRef != null)
                level.TileSets.Add(GetTileSet(entry.TileSetRef));

            if (entry.PropertySetRef != null)
                level.PropSet = GetPropSet(entry.PropertySetRef);

            foreach (var spriteSetRef in entry.SpriteSetRefs)
                level.SpriteSets.Add(GetSpriteSet(spriteSetRef));

            if (entry.PaletteRefs.Any())
            {
                foreach (var paletteRef in entry.PaletteRefs)
                    level.Palettes.Add(GetPalette(paletteRef));
            }
            else
            {
                foreach (var palette in level.Map.Properties.Palettes)
                    level.Palettes.Add(palette);
            }

            return level;
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
            FormatMan.RegisterFormat("PALETTE", new PaletteFormat());
            FormatMan.RegisterFormat("PCM_SOUND", new PCMSoundFormat());
        }

        #endregion Private Methods
    }
}
