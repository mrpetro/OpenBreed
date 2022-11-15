using OpenBreed.Database.EFCore;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class AssetsRepository : IRepository<IDbAsset>
    {
        private OpenBreedDbContext dataEx;

        public AssetsRepository(OpenBreedDbContext dataEx, Tables.XmlDbAssetTableDef tableDef)
        {
            this.dataEx = dataEx;

            if(!dataEx.Assets.Any())
                MigrateTable(tableDef);
        }


        private void MigrateTable(Tables.XmlDbAssetTableDef tableDef)
        {
            foreach (var item in tableDef.Items)
                Add(item);
 
            dataEx.SaveChanges();
        }

        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(IDbAsset);
            }
        }

        public IEnumerable<IDbEntry> Entries => dataEx.DataSources;

        public string Name => "Assets";

        public void Add(IDbAsset entry)
        {
            dataEx.Add(new DbAsset()
            {
                Id = entry.Id,
                Description = entry.Description,
                DataSourceRef = entry.DataSourceRef,
                FormatType = entry.FormatType,
                AssetParameters = DbAsset.FromParameters(entry.Parameters)
            });
        }

        public IDbEntry Find(string name)
        {
            return GetById(name);
        }

        public IDbAsset GetById(string id)
        {
            var found = dataEx.Assets.Find(id);

            if (found is null)
                return null;

            return found;
        }


        public IDbAsset GetNextTo(IDbAsset entry)
        {
            bool found = false;

            foreach (var item in dataEx.Assets)
            {
                if (found)
                    return item;

                found = (item.Id == entry.Id);
            }

            return null;
        }

        public IDbAsset GetPreviousTo(IDbAsset entry)
        {
            DbAsset previous = null;

            foreach (var item in dataEx.Assets)
            {
                if (item.Id == entry.Id)
                    break;

                previous = item;
            }

            if (previous is null)
                return null;

            return previous;
        }

        public IDbEntry New(string newId, Type entryType = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(IDbAsset entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IDbAsset entry)
        {
            throw new NotImplementedException();
        }
    }
}
