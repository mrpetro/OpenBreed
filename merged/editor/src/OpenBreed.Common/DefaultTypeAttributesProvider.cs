using OpenBreed.Common.Interface;
using System;

namespace OpenBreed.Common
{
    /// <summary>  
    /// Default implementation for ITypeAttributesProvider interface  
    /// </summary>
    public class DefaultTypeAttributesProvider : ITypeAttributesProvider
    {
        #region Public Methods

        public object[] GetAttributes(Type type)
        {
            return type.GetCustomAttributes(inherit: true);
        }

        #endregion Public Methods
    }
}