using OpenBreed.Core.Modules.Animation.Helpers;

namespace OpenBreed.Core.Managers
{
    public interface IAnimMan
    {
        #region Public Methods

        IAnimation Create(string name, float length);

        IAnimation GetById(int id);

        IAnimation GetByName(string name);

        void UnloadAll();

        #endregion Public Methods
    }
}