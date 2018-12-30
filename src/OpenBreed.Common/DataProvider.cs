using OpenBreed.Common.Formats;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
using OpenBreed.Common.Sources;
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
        public DataSourceProvider AssetsProvider { get; }

        #region Private Fields

        private IUnitOfWork _unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public DataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            AssetsProvider = new DataSourceProvider(_unitOfWork);

            Initialize();
        }

        #endregion Public Constructors

        #region Public Properties

        public DataFormatMan FormatMan { get; } = new DataFormatMan();

        #endregion Public Properties

        #region Public Methods

        public Image GetImage(string name)
        {
            var imageEntity = _unitOfWork.GetRepository<IImageEntity>().GetByName(name);
            if (imageEntity == null)
                throw new Exception("Image error: " + name);

            var asset = AssetsProvider.GetAsset(imageEntity.SourceRef);

            return FormatMan.Load(asset, imageEntity.Format) as Image;
        }

        public PaletteModel GetPalette(string name)
        {
            var paletteEntity = _unitOfWork.GetRepository<IPaletteEntity>().GetByName(name);
            if (paletteEntity == null)
                throw new Exception("Palette error: " + name);

            var asset = AssetsProvider.GetAsset(paletteEntity.SourceRef);

            return FormatMan.Load(asset, paletteEntity.Format) as PaletteModel;
        }

        public IPropSetEntity GetPropSet(string name)
        {
            var propSetEntity = _unitOfWork.GetRepository<IPropSetEntity>().GetByName(name);
            if (propSetEntity == null)
                throw new Exception("PropSet error: " + name);

            return propSetEntity;
        }

        public SourceBase GetSource(string name)
        {
            return null;
        }

        public SpriteSetModel GetSpriteSet(string name)
        {
            var spriteSetEntity = _unitOfWork.GetRepository<ISpriteSetEntity>().GetByName(name);
            if (spriteSetEntity == null)
                throw new Exception("SpriteSet error: " + name);

            var asset = AssetsProvider.GetAsset(spriteSetEntity.SourceRef);

            return FormatMan.Load(asset, spriteSetEntity.Format) as SpriteSetModel;
        }

        public TileSetModel GetTileSet(string name)
        {
            var tileSetEntity = _unitOfWork.GetRepository<ITileSetEntity>().GetByName(name);
            if (tileSetEntity == null)
                throw new Exception("TileSet error: " + name);

            var asset = AssetsProvider.GetAsset(tileSetEntity.SourceRef);

            return FormatMan.Load(asset, tileSetEntity.Format) as TileSetModel;
        }

        public LevelModel GetLevel(string name)
        {
            var levelEntity = _unitOfWork.GetRepository<ILevelEntity>().GetByName(name);
            if (levelEntity == null)
                throw new Exception("Level error: " + name);

            var asset = AssetsProvider.GetAsset(levelEntity.SourceRef);

            var level = new LevelModel();
            level.Map = FormatMan.Load(asset, levelEntity.Format) as MapModel;


            if (levelEntity.TileSetRef != null)
                level.TileSets.Add(GetTileSet(levelEntity.TileSetRef));

            if (levelEntity.PropertySetRef != null)
                level.PropSet = GetPropSet(levelEntity.PropertySetRef);

            foreach (var spriteSetRef in levelEntity.SpriteSetRefs)
                level.SpriteSets.Add(GetSpriteSet(spriteSetRef));

            if (levelEntity.PaletteRefs.Any())
            {
                foreach (var paletteRef in levelEntity.PaletteRefs)
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
        }

        #endregion Private Methods
    }
}
