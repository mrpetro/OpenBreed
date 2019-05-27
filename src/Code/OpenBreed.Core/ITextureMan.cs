using System.Drawing;

namespace OpenBreed.Core
{
    public interface ITextureMan
    {
        #region Public Methods

        ITexture GetByName(string name);

        ITexture GetById(int id);

        ITexture Load(Bitmap bitmap, string name);

        ITexture Load(string filePath, string name = null);

        void UnloadAll();

        #endregion Public Methods
    }
}