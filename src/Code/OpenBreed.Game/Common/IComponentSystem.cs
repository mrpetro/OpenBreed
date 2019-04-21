using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Game.Entities.Components;

namespace OpenBreed.Game.Common
{
    public interface IComponentSystem
    {
        void AddComponent(IEntityComponent component);
    }
}
