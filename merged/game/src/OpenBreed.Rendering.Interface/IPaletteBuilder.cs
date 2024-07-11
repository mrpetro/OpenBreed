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
        /// Sets color with specific palette index
        /// </summary>
        /// <param name="index">Index of palette color to set</param>
        /// <param name="color">Color to set</param>
        /// <returns>This builder instance</returns>
        IPaletteBuilder SetColor(int index, Color4 color);

        /// <summary>
        /// Sets multiple palette colors
        /// </summary>
        /// <param name="colors">Colors to set</param>
        /// <param name="startIndex">Optional starting index of color to modify</param>
        /// <param name="startIndex">Optional length of colors array to modify</param>
        /// <returns>This builder instance</returns>
        IPaletteBuilder SetColors(Color4[] colors, int startIndex = 0, int length = 0);

        /// <summary>
        /// Build palette
        /// </summary>
        /// <returns>Palette object</returns>
        IPalette Build();

        #endregion Public Methods
    }
}