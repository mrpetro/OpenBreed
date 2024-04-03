using System;

namespace OpenBreed.Editor.VM
{
    public interface IControlFactory
    {
        #region Public Methods

        public object Create(Type entryType);

        bool SupportsWpf(Type entryType);

        #endregion Public Methods
    }
}