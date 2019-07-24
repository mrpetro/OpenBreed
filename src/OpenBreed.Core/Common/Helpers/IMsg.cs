using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Helpers
{
    public interface IEntityMsg : IMsg
    {
        IEntity Entity { get; }
    }
}
