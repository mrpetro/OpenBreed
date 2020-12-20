using System;

namespace OpenBreed.Common
{
    public interface IVariableMan
    {
        #region Public Methods

        void RegisterVariable(Type type, object value, string name);

        void UnregisterVariable(string name);

        string ExpandVariables(string query);

        #endregion Public Methods
    }
}