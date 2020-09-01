using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using OpenBreed.Common.Model.Palettes;

namespace OpenBreed.Common.Builders.Palettes
{
    public class PaletteBuilder
    {
        internal string Name;
        internal Color[] Colors;

        public static PaletteBuilder NewPaletteModel()
        {
            return new PaletteBuilder();
        }

        private static Color[] DefaultPalette()
        {
            Color[] colors = new Color[256];

            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                colors[colorIndex] = Color.FromArgb(colorIndex, colorIndex, colorIndex);

            return colors;
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

        public PaletteBuilder SetColors(Color[] colors)
        {
            Colors = colors;

            return this;
        }

        public PaletteBuilder SetColor(int colorIndex, Color color)
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
    }
}
