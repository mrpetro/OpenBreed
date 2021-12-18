
namespace OpenBreed.Animation.Interface
{
    public interface IClipMan<TObject>
    {
        #region Public Methods

        IClip<TObject> CreateClip(string name, float length);

        IClip<TObject> GetById(int id);

        IClip<TObject> GetByName(string name);

        void UnloadAll();

        #endregion Public Methods
    }
}