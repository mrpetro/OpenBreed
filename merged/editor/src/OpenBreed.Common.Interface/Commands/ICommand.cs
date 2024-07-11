using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Interface.Commands
{
    public interface ICommand
    {
        void Execute();
        void UnExecute();
    }
}
