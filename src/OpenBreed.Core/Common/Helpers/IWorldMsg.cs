using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Helpers
{
    public interface IWorldMsg : IMsg
    {
        int WorldId { get; }
    }
}
