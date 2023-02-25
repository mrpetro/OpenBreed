using OpenBreed.Database.Interface.Items.Palettes;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for palette builder
    /// </summary>
    public interface IPaletteBuilder
    {
        #region Public Methods

        /// <summary>
        /// Sets created palette name
        /// </summary>
        /// <param name="name">Proposed palette name</param>
        /// <returns>This builder instance</returns>
        IPaletteBuilder SetName(string name);

        /// <summary>
        /// Sets palette length
        /// </summary>
        /// <param name="length">Length of palette</param>
        /// <returns>This builder instance</returns>
        IPaletteBuilder SetLength(int length);

        /// <summary>
        /// Sets color for specific palette index
        /// </summary>
        /// <param name="index">Index of palette color to set</param>
        /// <param name="color">Color to set</param>
        /// <returns>This builder instance</returns>
        IPaletteBuilder SetColor(int index, Color4 color);

        /// <summary>
        /// Build palette
        /// </summary>
        /// <returns>Palette object</returns>
        IPalette Build();

        #endregion Public Methods
    }
}