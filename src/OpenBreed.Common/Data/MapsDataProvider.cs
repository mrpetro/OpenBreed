using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class MapsDataProvider
    {
        #region Public Constructors

        public MapsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        public MapModel GetMap(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IMapEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Level error: " + id);

            if (entry.AssetRef == null)
                return null;

            var asset = Provider.Assets.GetAsset(entry.AssetRef);

            var map = Provider.FormatMan.Load(asset, entry.Format) as MapModel;

            if (entry.TileSetRef != null)
                map.TileSets.Add(Provider.TileSets.GetTileSet(entry.TileSetRef));

            if (entry.ActionSetRef != null)
                map.ActionSet = Provider.ActionSets.GetActionSet(entry.ActionSetRef);

            foreach (var spriteSetRef in entry.SpriteSetRefs)
                map.SpriteSets.Add(Provider.SpriteSets.GetSpriteSet(spriteSetRef));
        
            if (entry.PaletteRefs.Any())
            {
                foreach (var paletteRef in entry.PaletteRefs)
                    map.Palettes.Add(Provider.Palettes.GetPalette(paletteRef));
            }
            else
            {
                foreach (var palette in map.Properties.Palettes)
                    map.Palettes.Add(palette);
            }

            map.Tag = id;
            return map;
        }

    }
}
