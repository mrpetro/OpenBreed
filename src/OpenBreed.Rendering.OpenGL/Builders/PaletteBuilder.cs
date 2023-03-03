using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class PaletteBuilder : IPaletteBuilder
    {
        #region Private Fields

        private readonly List<Color4> colors = new List<Color4>();
        private readonly PaletteMan paletteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal PaletteBuilder(PaletteMan paletteMan)
        {
            this.paletteMan = paletteMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal string Name { get; private set; }

        internal float[] DirectData => colors.SelectMany(item => new float[] { item.R, item.G, item.B, item.A }).ToArray();

        #endregion Internal Properties

        #region Public Methods

        public IPalette Build()
        {
            return new Palette(this);
        }

        public IPaletteBuilder SetColor(int index, Color4 color)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException($"Trying to set color with negative index ({index}).");
            }

            if (index >= colors.Count)
            {
                throw new IndexOutOfRangeException($"Trying to set color with index ({index}) bigger than palette size ({colors.Count}).");
            }

            colors[index] = color;

            return this;
        }

        public IPaletteBuilder SetColors(Color4[] colors, int startIndex = 0, int length = 0)
        {
            ArgumentNullException.ThrowIfNull(colors);

            if (length == 0)
            {
                if(this.colors.Count > colors.Length)
                    length = colors.Length - startIndex;
                else
                    length = colors.Length;
            }

            if (startIndex < 0)
            {
                throw new IndexOutOfRangeException($"Trying to use negative starting index ({startIndex}) for setting multiple colors.");
            }

            if (startIndex >= this.colors.Count)
            {
                throw new IndexOutOfRangeException($"Trying to use starting index ({startIndex}) bigger than palette size ({this.colors.Count}).");
            }

            if (length > colors.Length)
            {
                throw new IndexOutOfRangeException($"Length exceeds input colors array size ({length}).");
            }

            if (startIndex + length > this.colors.Count)
            {
                throw new IndexOutOfRangeException($"Stating index and length exceeds palette length ({length}).");
            }

            for (int i = 0; i < length; i++)
            {
                this.colors[startIndex + i] = colors[i];
            }

            return this;
        }

        public IPaletteBuilder SetLength(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException($"Trying to set less than zero palette length ({length}).");
            }

            if (colors.Count > length)
            {
                colors.RemoveRange(0, colors.Count - length);
            }
            else if (colors.Count < length)
            {
                colors.AddRange(Enumerable.Range(0, length - colors.Count).Select(item => new Color4()));
            }

            return this;
        }

        public IPaletteBuilder SetName(string name)
        {
            if (paletteMan.Contains(name))
                throw new InvalidOperationException($"Palette with name '{name}' already exists.");

            Name = name;
            return this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(Palette palette)
        {
            return paletteMan.Register(Name, palette);
        }

        #endregion Internal Methods
    }
}