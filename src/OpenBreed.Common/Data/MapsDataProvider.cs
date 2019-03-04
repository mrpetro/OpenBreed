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
        private Dictionary<string, MapModel> _models = new Dictionary<string, MapModel>();


        #region Public Constructors

        public MapsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        internal void Save()
        {
            foreach (var item in _models)
            {
                var entryId = item.Key;
                var data = item.Value;

                var entry = Provider.UnitOfWork.GetRepository<IMapEntry>().GetById(item.Key);
                if (entry == null)
                    throw new Exception($"Map error: {item.Key}");

                if (entry.AssetRef == null)
                    throw new InvalidOperationException("Missing Asset reference");

                var asset = Provider.Assets.GetAsset(entry.AssetRef);
                Provider.FormatMan.Save(asset, item.Value, entry.Format);
            }
        }

        public MapModel GetMap(string id)
        {
            MapModel map;

            if (_models.TryGetValue(id, out map))
                return map;

            var entry = Provider.UnitOfWork.GetRepository<IMapEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Map error: " + id);

            if (entry.AssetRef == null)
                return null;

            var asset = Provider.Assets.GetAsset(entry.AssetRef);
            map = Provider.FormatMan.Load(asset, entry.Format) as MapModel;
            map.Tag = id;

            _models.Add(id, map);

            return map;
        }

    }
}
