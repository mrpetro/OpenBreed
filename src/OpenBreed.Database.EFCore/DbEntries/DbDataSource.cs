using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using System;

namespace OpenBreed.Database.EFCore.DbEntries
{
    public enum DataSourceTypeEnum
    {
        DbFileDataSource = 0,
        DbEpfArchiveFileDataSource = 1,
    }

    public class DataSourceType
    {
        public DataSourceTypeEnum Id { get; set; }
        public string Name { get; set; }
    }

    public class DbDataSource : DbEntry
    {
        public DbDataSource()
        { }

        protected DbDataSource(DbDataSource other)
            : base(other)
        {
        }

        public DataSourceTypeEnum TypeId { get; set; }
        public DataSourceType Type { get; set; }

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }
    }

    public class DbEpfArchiveFileDataSource : DbDataSource, IDbEpfArchiveDataSource
    {
        public DbEpfArchiveFileDataSource()
        {
            TypeId = DataSourceTypeEnum.DbEpfArchiveFileDataSource;
        }

        protected DbEpfArchiveFileDataSource(DbEpfArchiveFileDataSource other)
            : base(other)
        {
            TypeId = DataSourceTypeEnum.DbEpfArchiveFileDataSource;
            ArchivePath = other.ArchivePath;
            EntryName = other.EntryName;
        }

        public string ArchivePath { get; set; }
        public string EntryName { get; set; }

        public override IDbEntry Copy()
        {
            return new DbEpfArchiveFileDataSource(this);
        }
    }

    public class DbFileDataSource : DbDataSource, IDbFileDataSource
    {
        public DbFileDataSource()
        {
            TypeId = DataSourceTypeEnum.DbFileDataSource;
        }

        protected DbFileDataSource(DbFileDataSource other)
            : base(other)
        {
            TypeId = DataSourceTypeEnum.DbFileDataSource;
            FilePath = other.FilePath;
        }

        public string FilePath { get; set; }

        public override IDbEntry Copy()
        {
            return new DbFileDataSource(this);
        }
    }
}