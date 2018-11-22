using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Sprites.Builders
{
    public class SpriteBuilder
    {
        internal int Index;
        internal int Width;
        internal int Height;
        internal byte[] Data = null;

        public SpriteBuilder SetIndex(int index)
        {
            Index = index;
            return this;
        }

        public SpriteBuilder SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            return this;
        }

        public SpriteBuilder SetData(byte[] data)
        {
            Data = data;
            return this;
        }

        public static SpriteBuilder NewSprite()
        {
            return new SpriteBuilder();
        }

        public SpriteModel Build()
        {
            return new SpriteModel(this);
        }
    }
}
