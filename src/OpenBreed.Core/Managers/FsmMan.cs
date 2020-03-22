using OpenBreed.Core.Collections;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Managers
{
    public class FsmMan
    {
        //private IdMap<StateMachine> 

        public ICore Core { get; }

        public FsmMan(ICore core)
        {
            Core = core;
        }

        //public 
    }
}
