using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Interface
{
    public interface IRepositoryProvider
    {
        #region Public Properties

        IEnumerable<IRepository> Repositories { get; }

        #endregion Public Properties

        #region Public Methods

        IRepository<T> GetRepository<T>() where T : IEntry;

        IRepository GetRepository(string name);

        IRepository GetRepository(Type type);

        #endregion Public Methods
    }
}