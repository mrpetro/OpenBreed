using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Linq;

namespace OpenBreed.Database.Xml.Repositories
{
    public abstract class XmlRepositoryBase<T> : XmlReadonlyRepositoryBase<T>, IRepository<T> where T : class, IDbEntry
    {
        #region Public Methods

        public abstract void Add(T newEntry);

        public IDbEntry New(string newId, Type entryType = null)
        {
            if (Find(newId) != null)
                throw new Exception($"Entry with Id '{newId}' already exist.");

            if (entryType == null)
                entryType = EntryTypes.FirstOrDefault();

            var newEntry = Create(entryType);

            newEntry.Id = newId;
            Add(newEntry);
            return newEntry;
        }

        public void Remove(T entry)
        {
            throw new NotImplementedException();
        }

        public void Update(T entry)
        {
            var index = GetIndexOf(entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            ReplaceEntryWithIndex(index, entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void ReplaceEntryWithIndex(int index, T newEntry);

        protected T Create(Type type)
        {
            return Activator.CreateInstance(type) as T;
        }

        #endregion Protected Methods
    }
}