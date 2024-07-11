using OpenBreed.Model.Sprites;
using System.IO;

namespace OpenBreed.Reader.Sprites
{
    public interface ISpriteSetReader
    {
        #region Public Methods

        SpriteSetModel Read(Stream stream);

        #endregion Public Methods
    }
}