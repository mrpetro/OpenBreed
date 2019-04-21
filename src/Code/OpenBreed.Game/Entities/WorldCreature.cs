using OpenBreed.Game.Entities.Builders;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public class WorldCreature : WorldEntity
    {
        Vector2 position;

        public WorldCreature(WorldCreatureBuilder builder) : base(builder)
        {
            position = builder.position;
        }
    }
}
