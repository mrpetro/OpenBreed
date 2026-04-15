using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Animations;

namespace OpenBreed.Animation.Abstractions.Data
{
    public interface IReadOnlyClipDataLoader<TObject> : IDataLoader<IReadOnlyClip<TObject>>
    {
        #region Public Methods

        IReadOnlyClip<TObject> Load(IDbAnimation dbAnimation, bool reload = false);

        #endregion Public Methods
    }
}