using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.OpenGL.Helpers
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

        public Color4 GetColor(uint index)
        {
            index = index * 4;
            return new Color4(
                DirectData[index],
                DirectData[++index],
                DirectData[++index],
                DirectData[++index]);
        }

        public void SetColor(uint index, Color4 color)
        {
            index = index * 4;
            DirectData[index] = color.R;
            DirectData[++index] = color.G;
            DirectData[++index] = color.B;
            DirectData[++index] = color.A;
        }

        #endregion Public Methods
    }
}