using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Interface
{
    public delegate void ActionBinderCallback<TAction>(TAction action) where TAction : Enum;

    public interface IActionTriggerBinder
    {
        void Subscribe<TTrigger>(TTrigger trigger, ActionBinderCallback<Enum> triggerCallback)
            where TTrigger : Enum;

        void Unsubscribe<TTrigger>(TTrigger trigger, ActionBinderCallback<Enum> triggerCallback)
            where TTrigger : Enum;

    }
}
