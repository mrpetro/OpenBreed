using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class ActorSystem : IEventSystem<DestroyedEvent>
    {
        public void Update(DestroyedEvent e)
        {
            //throw new NotImplementedException();
        }
    }
}
