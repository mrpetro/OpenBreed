using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Palette interface
    /// </summary>
    public interface IPalette
    {
        #region Public Properties

        /// <summary>
        /// ID of this palette
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Provides direct access to color floats array which looks as follows
        /// [C1.R, C1.G, C1.B, C1.A, C2.R, C2.G, C2.B, C2.A, ...]
        /// </summary>
        float[] DirectData { get; }

        /// <summary>
        /// Get length of this palette
        /// </summary>
        int Length { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets color by given index
        /// </summary>
        /// <param name="index">Index of palette color to get</param>
        /// <returns>Color</returns>
        Color4 GetColor(uint index);

        /// <summary>
        /// Sets color by given index
        /// </summary>
        /// <param name="index">>Index of palette color to set</param>
        /// <param name="color">Color</param>
        void SetColor(uint index, Color4 color);

        #endregion Public Methods
    }
}