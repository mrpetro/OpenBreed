using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Animations;

namespace OpenBreed.Animation.Interface.Data
{
    public interface IAnimationClipDataLoader<TObject> : IDataLoader<IClip<TObject>>
    {
        #region Public Methods

        IClip<TObject> Load(IDbAnimation dbAnimation, params object[] args);

        #endregion Public Methods
    }
}