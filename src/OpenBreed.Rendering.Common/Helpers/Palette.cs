using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Common.Builders;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Common.Helpers
{
    internal class Palette : IPalette
    {
        #region Public Constructors

        public Palette(PaletteBuilder builder)
        {
            Id = builder.Register(this);
            DirectData = builder.DirectData;
        }

        #endregion Public Constructors

        #region Public Properties

        public float[] DirectData { get; }

        /// <summary>
        /// Id of this picture
        /// </summary>
        public int Id { get; }

        public int Length => DirectData.Length / 4;

        #endregion Public Properties

        #region Public Methods

        public Color4<Rgba> GetColor(uint index)
        {
            index = index * 4;
            return new Color4<Rgba>(
                DirectData[index],
                DirectData[++index],
                DirectData[++index],
                DirectData[++index]);
        }

        public void SetColor(uint index, Color4<Rgba> color)
        {
            index = index * 4;
            DirectData[index] = color.X;
            DirectData[++index] = color.Y;
            DirectData[++index] = color.Z;
            DirectData[++index] = color.W;
        }

        #endregion Public Methods
    }
}