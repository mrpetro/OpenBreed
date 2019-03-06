using OpenBreed.Common.Data2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class Data2DataProvider
    {
        private Dictionary<string, object> _models = new Dictionary<string, object>();

        #region Public Constructors

        public Data2DataProvider(DataProvider provider)
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

                var entry = Provider.UnitOfWork.GetRepository<IDataEntry>().GetById(item.Key);
                if (entry == null)
                    throw new Exception($"Data error: {item.Key}");

                if (entry.AssetRef == null)
                    throw new InvalidOperationException("Missing Asset reference");

                var asset = Provider.Assets.GetAsset(entry.AssetRef);
                Provider.FormatMan.Save(asset, item.Value, entry.Format);
            }
        }

        public object GetData(string id)
        {
            object data;

            if (_models.TryGetValue(id, out data))
                return data;

            var entry = Provider.UnitOfWork.GetRepository<IDataEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Data error: " + id);

            if (entry.AssetRef == null)
                return null;

            var asset = Provider.Assets.GetAsset(entry.AssetRef);
            data = Provider.FormatMan.Load(asset, entry.Format);

            _models.Add(id, data);

            return data;
        }

    }
}
