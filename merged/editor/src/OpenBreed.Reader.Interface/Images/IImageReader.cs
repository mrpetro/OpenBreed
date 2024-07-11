using OpenBreed.Common.Interface.Drawing;
using System.IO;

namespace OpenBreed.Reader.Images
{
    public interface IImageReader
    {
        #region Public Methods

        IImage Read(Stream stream);

        #endregion Public Methods
    }
}