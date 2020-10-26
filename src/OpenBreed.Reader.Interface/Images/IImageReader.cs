using System.Drawing;
using System.IO;

namespace OpenBreed.Reader.Images
{
    public interface IImageReader
    {
        #region Public Methods

        Image Read(Stream stream);

        #endregion Public Methods
    }
}