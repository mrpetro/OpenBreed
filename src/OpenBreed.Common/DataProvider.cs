using OpenBreed.Common.Formats;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Actions;
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

            var image = FormatMan.Load(asset, entry.Format) as Image;
            image.Tag = name;
            return image;
        }

        public PaletteModel GetPalette(string id)
        {
            var entry = _unitOfWork.GetRepository<IPaletteEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var palette = FormatMan.Load(asset, entry.Format) as PaletteModel;
            palette.Tag = id;
            return palette;
        }

        public SoundModel GetSound(string id)
        {
            var entry = _unitOfWork.GetRepository<ISoundEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Sound error: " + id);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var sound = FormatMan.Load(asset, entry.Format) as SoundModel;
            sound.Tag = id;
            return sound;
        }

        public IActionSetEntry GetActionSet(string id)
        {
            var entry = _unitOfWork.GetRepository<IActionSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("PropSet error: " + id);

            return entry;
        }

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = _unitOfWork.GetRepository<ISpriteSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("SpriteSet error: " + id);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var spriteSet = FormatMan.Load(asset, entry.Format) as SpriteSetModel;
            spriteSet.Tag = id;
            return spriteSet;
        }

        public TileSetModel GetTileSet(string id)
        {
            var entry = _unitOfWork.GetRepository<ITileSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("TileSet error: " + id);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var tileSet = FormatMan.Load(asset, entry.Format) as TileSetModel;
            tileSet.Tag = id;
            return tileSet;
        }

        public MapModel GetMap(string id)
        {
            var entry = _unitOfWork.GetRepository<IMapEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Level error: " + id);

            var asset = AssetsProvider.GetAsset(entry.AssetRef);

            var map = FormatMan.Load(asset, entry.Format) as MapModel;

            if (entry.TileSetRef != null)
                map.TileSets.Add(GetTileSet(entry.TileSetRef));

            if (entry.ActionSetRef != null)
                map.ActionSet = GetActionSet(entry.ActionSetRef);

            foreach (var spriteSetRef in entry.SpriteSetRefs)
                map.SpriteSets.Add(GetSpriteSet(spriteSetRef));

            if (entry.PaletteRefs.Any())
            {
                foreach (var paletteRef in entry.PaletteRefs)
                    map.Palettes.Add(GetPalette(paletteRef));
            }
            else
            {
                foreach (var palette in map.Properties.Palettes)
                    map.Palettes.Add(palette);
            }

            map.Tag = id;
            return map;
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
