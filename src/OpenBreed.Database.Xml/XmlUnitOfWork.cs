using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Database.Xml
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        #region Private Fields

        private readonly IDatabase database;

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        #endregion Private Fields

        #region Internal Constructors

        public XmlUnitOfWork(IDatabase database)
        {
            this.database = database;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name
        { get { return database.Name; } }

        public IEnumerable<IRepository> Repositories
        { get { return _repositories.Values; } }

        #endregion Public Properties

        #region Public Methods

        public IRepository<T> GetRepository<T>() where T : IDbEntry
        {
            return (IRepository<T>)GetRepository(typeof(T));
        }

        public IRepository GetRepository(string name)
        {
            return _repositories.Values.FirstOrDefault(item => item.Name == name);
        }

        public IRepository GetRepository(Type type)
        {
            IRepository foundRepo;

            if (!_repositories.TryGetValue(type, out foundRepo))
                throw new Exception($"Repository of type {type} not found.");

            return foundRepo;
        }

        public void RegisterRepository<T>(IRepository<T> repository) where T : IDbEntry
        {
            _repositories.Add(typeof(T), repository);
        }

        public void Save()
        {
            database.Save();
        }

        #endregion Public Methods
    }
}