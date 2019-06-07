using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Movement.Components
{
    public interface IMovementComponent : IEntityComponent
    {
        void Update(float dt);
    }
}
