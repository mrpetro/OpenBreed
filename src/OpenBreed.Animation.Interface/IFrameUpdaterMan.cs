
using System;

namespace OpenBreed.Animation.Interface
{
    public interface IFrameUpdaterMan
    {
        #region Public Methods

        int Register<TValue>(string name, FrameUpdater<TValue> frameUpdater);

        FrameUpdater<TValue> GetById<TValue>(int id);

        FrameUpdater<TValue> GetByName<TValue>(string name);

        #endregion Public Methods
    }
}