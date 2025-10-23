using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Animations;

namespace OpenBreed.Animation.Interface.Data
{
    public interface IEditableClipDataLoader<TObject> : IDataLoader<IEditableClip<TObject>>
    {
        #region Public Methods

        IEditableClip<TObject> Load(IDbAnimation dbAnimation);

        #endregion Public Methods
    }
}