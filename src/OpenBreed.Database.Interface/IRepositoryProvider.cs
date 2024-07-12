using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Interface
{
    public interface IReadonlyRepositoryProvider
    {
        #region Public Properties

        IEnumerable<IReadonlyRepository> Repositories { get; }

        #endregion Public Properties

        #region Public Methods

        IReadonlyRepository<TEntry> GetRepository<TEntry>() where TEntry : IDbEntry;

        IReadonlyRepository GetRepository(string name);

        IReadonlyRepository GetRepository(Type type);

        #endregion Public Methods
    }

    public interface IRepositoryProvider
    {
        #region Public Properties

        IEnumerable<IRepository> Repositories { get; }

        #endregion Public Properties

        #region Public Methods

        IRepository<TEntry> GetRepository<TEntry>() where TEntry : IDbEntry;

        IRepository GetRepository(string name);

        IRepository GetRepository(Type type);

        #endregion Public Methods
    }
}