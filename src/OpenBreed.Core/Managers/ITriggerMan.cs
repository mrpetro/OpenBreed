using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Managers
{
    public interface ITriggerMan
    {
        IEventsMan EventsMan { get; }

        void CreateTrigger<TEventArgs>(Func<TEventArgs, bool> conditionFunction, Action action, bool singleTime) where TEventArgs : EventArgs;
        
        ITriggerBuilder NewTrigger();
    }
}
