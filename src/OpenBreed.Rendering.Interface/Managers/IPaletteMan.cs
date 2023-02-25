using OpenBreed.Model.Palettes;
using OpenTK;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Palette manager interface
    /// </summary>
    public interface IPaletteMan
    {
        #region Public Methods

        /// <summary>
        /// Get palette by it's ID
        /// </summary>
        /// <param name="paletteId">Id of palette to get</param>
        /// <returns>Palette object</returns>
        IPalette GetById(int paletteId);

        /// <summary>
        /// Get palette by it's name
        /// </summary>
        /// <param name="paletteName">Name of palette to get</param>
        /// <returns>Palette object</returns>
        IPalette GetByName(string paletteName);

        /// <summary>
        /// Get palette name based on it's ID
        /// </summary>
        /// <param name="paletteId">ID of palette</param>
        /// <returns>Palette name</returns>
        string GetName(int paletteId);

        /// <summary>
        /// Checks if palette given name already exists
        /// </summary>
        /// <param name="paletteName">Name of palette to check</param>
        /// <returns>True if exits, false otherwise</returns>
        bool Contains(string paletteName);

        /// <summary>
        /// Creates new palette
        /// </summary>
        /// <returns>Palette builder</returns>
        IPaletteBuilder CreatePalette();

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}