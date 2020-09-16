using OpenBreed.Model.Palettes;
using System.IO;

namespace OpenBreed.Reader.Palettes
{
    public interface IPaletteReader
    {
        #region Public Methods

        PaletteModel Read(Stream stream);

        #endregion Public Methods
    }
}