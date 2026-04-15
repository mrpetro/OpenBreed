using System;

namespace OpenBreed.Input.Abstractions
{
    public interface IActionCodeProvider
    {
        #region Public Methods

        int GetCode<TAction>(TAction action) where TAction : Enum;

        bool TryGetCode(string actionTypeName, string actionName, out int actionCode);

        #endregion Public Methods
    }
}