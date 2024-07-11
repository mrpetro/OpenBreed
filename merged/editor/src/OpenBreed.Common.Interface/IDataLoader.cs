using System;

namespace OpenBreed.Common.Interface
{
    public interface IDataLoader
    {
        #region Public Methods

        object LoadObject(string entryId);

        #endregion Public Methods
    }

    public interface IDataLoader<TInterface> : IDataLoader
    {
        #region Public Methods

        TInterface Load(string entryId, params object[] args);

        #endregion Public Methods
    }
}