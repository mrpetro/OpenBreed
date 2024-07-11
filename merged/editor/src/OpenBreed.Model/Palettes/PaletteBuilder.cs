using OpenBreed.Common.Interface.Drawing;
using System;

namespace OpenBreed.Model.Palettes
{
    public class PaletteBuilder
    {
        #region Internal Fields

        internal string Name;
        internal MyColor[] Colors;

        #endregion Internal Fields

        #region Private Constructors

        private PaletteBuilder()
        {
            Colors = new MyColor[256];
        }

        #endregion Private Constructors

        #region Public Methods

        public static PaletteBuilder NewPaletteModel()
        {
            return new PaletteBuilder();
        }

        public PaletteBuilder SetName(string name)
        {
            Name = name;

            return this;
        }

        public PaletteBuilder CreateColors()
        {
            Colors = DefaultPalette();

            return this;
        }

        public PaletteBuilder SetColors(MyColor[] colors)
        {
            Colors = colors;

            return this;
        }

        public PaletteBuilder SetColor(int colorIndex, MyColor color)
        {
            if (Colors == null)
                throw new Exception("Colors not created first!");

            Colors[colorIndex] = color;

            return this;
        }

        public PaletteModel Build()
        {
            return new PaletteModel(this);
        }

        #endregion Public Methods

        #region Private Methods

        private static MyColor[] DefaultPalette()
        {
            var colors = new MyColor[256];

            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                colors[colorIndex] = MyColor.FromArgb((byte)colorIndex, (byte)colorIndex, (byte)colorIndex);

            return colors;
        }

        #endregion Private Methods
    }
}