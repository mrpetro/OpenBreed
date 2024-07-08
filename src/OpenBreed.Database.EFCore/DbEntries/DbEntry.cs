using OpenBreed.Database.Interface.Items;

namespace OpenBreed.Database.EFCore.DbEntries
{
    public abstract class DbEntry : IDbEntry
    {
        protected DbEntry()
        { }

        protected DbEntry(DbEntry other)
        {
            Id = other.Id;
            Description = other.Description;
        }

        #region Public Properties

        public string Description { get; set; }

        public string Id { get; set; }

        public abstract IDbEntry Copy();

        public bool Equals(IDbEntry other)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"DbTableItem '{Id}'";
        }

        #endregion Public Methods
    }
}