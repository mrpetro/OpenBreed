using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Common.Components
{
    public interface ITransformComponent : IEntityComponent
    {
        Matrix4 Matrix { get; }
    }
}
