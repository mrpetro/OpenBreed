using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Entities
{
    public interface ISystemEvent
    {
        string Type { get; }
        object Data { get; }
    }
}
