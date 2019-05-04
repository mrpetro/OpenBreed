using OpenBreed.Game.Physics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Physics.Components
{
    public interface IDynamicBody : IPhysicsComponent
    {
        /// <summary>
        /// DEBUG only
        /// </summary>
        bool Collides { get; set; }
        List<Tuple<int, int>> Boxes { get; set; }
    }
}
