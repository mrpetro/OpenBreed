
using System;

namespace OpenBreed.Animation.Interface
{
    public interface IFrameUpdaterMan<TObject>
    {
        #region Public Methods

        int Register<TValue>(string name, FrameUpdater<TObject, TValue> frameUpdater, FrameLoader<TValue> frameLoader = null);

        FrameUpdater<TObject, TValue> GetById<TValue>(int id);

        FrameUpdater<TObject, TValue> GetByName<TValue>(string name);
        FrameLoader<TValue> GetLoaderByName<TValue>(string name);

        #endregion Public Methods
    }
}