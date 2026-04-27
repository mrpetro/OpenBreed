using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class ActorSystem : IEventSystem<DestroyedEvent>
    {
        public void OnEvent(DestroyedEvent e, IWorld world)
        {
            //throw new NotImplementedException();
        }
    }
}
