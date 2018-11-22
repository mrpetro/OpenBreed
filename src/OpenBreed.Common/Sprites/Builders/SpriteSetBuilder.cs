using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Sprites.Builders
{
    public class SpriteSetBuilder
    {
        internal List<SpriteModel> Sprites;

        public SpriteSetBuilder()
        {
            Sprites = new List<SpriteModel>();
        }

        public SpriteSetBuilder AddSprite(SpriteModel sprite)
        {
            Sprites.Add(sprite);
            return this;
        }

        public static SpriteSetBuilder NewSpriteSet()
        {
            return new SpriteSetBuilder();
        }

        public SpriteSetModel Build()
        {
            return new SpriteSetModel(this);
        }
    }
}
