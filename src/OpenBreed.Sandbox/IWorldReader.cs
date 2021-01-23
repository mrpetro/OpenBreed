using OpenBreed.Core;
using OpenBreed.Ecsw;
using OpenBreed.Ecsw.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox
{
    public interface IWorldReader
    {
        World GetWorld();
    }
}
