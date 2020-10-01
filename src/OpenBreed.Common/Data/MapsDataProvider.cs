using OpenBreed.Model.Maps;
using OpenBreed.Database.Interface.Items.Maps;
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
                throw new Exception("Map error: " + id);

            if (entry.DataRef == null)
                return null;

            var map = Provider.GetData<MapModel>(entry.DataRef);

            if (entry.TileSetRef != null)
                map.TileSet = Provider.TileSets.GetTileSet(entry.TileSetRef);

            map.Palettes.Clear();
            foreach (var paletteRef in entry.PaletteRefs)
                map.Palettes.Add(Provider.Palettes.GetPalette(paletteRef));

            if (entry.ActionSetRef != null)
                map.ActionSet = Provider.ActionSets.GetActionSet(entry.ActionSetRef);

            return map;
        }

    }
}
