using OpenBreed.Common.Sprites.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Sprites
{
    public class SpriteSetModel
    {
        public List<SpriteModel> Sprites { get; set; }

        public SpriteSetModel(SpriteSetBuilder builder)
        {
            Sprites = builder.Sprites;
        }
    }
}
