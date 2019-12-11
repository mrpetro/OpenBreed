using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public interface IWorldCommand : ICommand
    {
        int WorldId { get; }
    }
}
