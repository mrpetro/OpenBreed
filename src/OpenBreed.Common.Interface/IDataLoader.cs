using System;

namespace OpenBreed.Common.Interface
{
    public interface IDataLoader
    {
    }

    public interface IDataLoader<TInterface> : IDataLoader
    {
        #region Public Methods

        TInterface Load(string entryId);

        #endregion Public Methods
    }
}