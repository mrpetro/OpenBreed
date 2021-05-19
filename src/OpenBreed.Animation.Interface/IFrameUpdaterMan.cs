
using System;

namespace OpenBreed.Animation.Interface
{
    public interface IFrameUpdaterMan
    {
        #region Public Methods

        int Register(Delegate frameUpdater);

        Delegate GetById(int id);

        Delegate GetByName(string name);

        #endregion Public Methods
    }
}