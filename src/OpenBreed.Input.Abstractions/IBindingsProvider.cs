using System;

namespace OpenBreed.Input.Abstractions
{
    public interface IBindingsProvider
    {
        #region Public Methods

        int GetMappedKey<TKey>(TKey key) where TKey : Enum;

        #endregion Public Methods
    }
}