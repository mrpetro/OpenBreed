
namespace OpenBreed.Animation.Interface
{
    public interface IClipMan
    {
        #region Public Methods

        IClip CreateClip(string name, float length);

        IClip GetById(int id);

        IClip GetByName(string name);

        void UnloadAll();

        #endregion Public Methods
    }
}