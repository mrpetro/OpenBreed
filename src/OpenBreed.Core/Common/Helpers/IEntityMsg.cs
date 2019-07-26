using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Helpers
{
    public interface IMsg
    {
        string Type { get; }
        object Data { get; }
    }
}
