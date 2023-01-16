using System;

namespace OpenBreed.Input.Interface
{
    public interface IBindingsProvider
    {
        #region Public Methods

        int GetMappedKey<TKey>(TKey key) where TKey : Enum;

        #endregion Public Methods
    }
}