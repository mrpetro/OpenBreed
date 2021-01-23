using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IControlComponent : IEntityComponent
    {
        string Type { get; }
    }
}
