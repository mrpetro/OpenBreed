
namespace OpenBreed.Animation.Interface
{
    public interface IAnimationMan
    {
        #region Public Methods

        IAnimation Create(string name, float length);

        IAnimation GetById(int id);

        IAnimation GetByName(string name);

        void UnloadAll();

        #endregion Public Methods
    }
}