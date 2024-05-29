using OpenBreed.Database.EFCore;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class DataSourcesRepository : IRepository<IDbDataSource>
    {
        private OpenBreedDbContext dataEx;

        public DataSourcesRepository(OpenBreedDbContext dataEx, Tables.XmlDbDataSourceTableDef xmlDbDataSourceTableDef)
        {
            this.dataEx = dataEx;

            if(!dataEx.DataSources.Any())
                MigrateTable(xmlDbDataSourceTableDef);
        }


        private void MigrateTable(Tables.XmlDbDataSourceTableDef tableDef)
        {
            foreach (var item in tableDef.Items)
                Add(item);
 
            dataEx.SaveChanges();
        }

        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(IDbFileDataSource);
                yield return typeof(IDbEpfArchiveDataSource);
            }
        }

        public IEnumerable<IDbEntry> Entries => dataEx.DataSources;

        public string Name => "Data sources";

        public void Add(IDbDataSource entry)
        {
            if (entry is IDbFileDataSource fileDataSource)
            {
                dataEx.Add(new DbFileDataSource()
                {
                    Id = fileDataSource.Id,
                    Description = fileDataSource.Description,
                    FilePath = fileDataSource.FilePath
                });
            }
            else if (entry is IDbEpfArchiveDataSource epfArchiveDataSource)
            {
                dataEx.Add(new DbEpfArchiveFileDataSource()
                {
                    Id = epfArchiveDataSource.Id,
                    Description = epfArchiveDataSource.Description,
                    ArchivePath = epfArchiveDataSource.ArchivePath,
                    EntryName = epfArchiveDataSource.EntryName
                });
            }
        }

        public IDbEntry Find(string name)
        {
            return GetById(name);
        }

        public IDbDataSource GetById(string id)
        {
            var found = dataEx.DataSources.Find(id);

            if (found is null)
                return null;

            switch (found.TypeId)
            {
                case DataSourceTypeEnum.DbFileDataSource:
                    return dataEx.FileDataSources.Find(id);
                case DataSourceTypeEnum.DbEpfArchiveFileDataSource:
                    return dataEx.EpfArchiveFileDataSources.Find(id);
                default:
                    throw new NotImplementedException($"Unknown id = {id}");
            }
        }

        private IDbDataSource GetByIdAndType(string id, DataSourceTypeEnum type)
        {
            switch (type)
            {
                case DataSourceTypeEnum.DbFileDataSource:
                    return dataEx.FileDataSources.Find(id);
                case DataSourceTypeEnum.DbEpfArchiveFileDataSource:
                    return dataEx.EpfArchiveFileDataSources.Find(id);
                default:
                    throw new NotImplementedException($"Unknown id = {id}");
            }
        }

        public IDbDataSource GetNextTo(IDbDataSource entry)
        {
            bool found = false;

            foreach (var item in dataEx.DataSources)
            {
                if (found)
                    return GetByIdAndType(item.Id, item.TypeId);

                found = (item.Id == entry.Id);
            }

            return null;
        }

        public IDbDataSource GetPreviousTo(IDbDataSource entry)
        {
            DbDataSource previous = null;

            foreach (var item in dataEx.DataSources)
            {
                if (item.Id == entry.Id)
                    break;

                previous = item;
            }

            if (previous is null)
                return null;

            return GetByIdAndType(previous.Id, previous.TypeId);
        }

        public IDbEntry New(string newId, Type entryType = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(IDbDataSource entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IDbDataSource entry)
        {
            throw new NotImplementedException();
        }

        bool IRepository<IDbDataSource>.Remove(IDbDataSource entry)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IDbEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
