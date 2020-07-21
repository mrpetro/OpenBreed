using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Components
{
    public interface IControlComponent : IEntityComponent
    {
        string Type { get; }
    }
}
