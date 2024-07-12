using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenBreed.Input.Generic
{
    public class DefaultActionTriggerBinder : IActionTriggerBinder
    {
        #region Private Fields

        private readonly IInputsMan inputsMan;

        private readonly Dictionary<Keys, HashSet<Enum>> mappings = new Dictionary<Keys, HashSet<Enum>>();
        private readonly Dictionary<Enum, HashSet<Delegate>> subscriptions = new Dictionary<Enum, HashSet<Delegate>>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultActionTriggerBinder(IInputsMan inputsMan)
        {
            this.inputsMan = inputsMan;

            inputsMan.KeyboardStateChanged += InputsMan_KeyboardStateChanged;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Bind<TTrigger>(TTrigger trigger, Keys key)
            where TTrigger : Enum
        {
            if (!mappings.TryGetValue(key, out HashSet<Enum> triggers))
            {
                triggers = new HashSet<Enum>();
                mappings.Add(key, triggers);
            }

            triggers.Add(trigger);
        }

        public void Subscribe<TTrigger>(TTrigger trigger, ActionBinderCallback<Enum> triggerCallback) where TTrigger : Enum
        {
            if (!subscriptions.TryGetValue(trigger, out HashSet<Delegate> delegates))
            {
                delegates = new HashSet<Delegate>();
                subscriptions.Add(trigger, delegates);
            }

            delegates.Add(triggerCallback);
        }

        public void Unsubscribe<TTrigger>(TTrigger trigger, ActionBinderCallback<Enum> triggerCallback) where TTrigger : Enum
        {
            if (!subscriptions.TryGetValue(trigger, out HashSet<Delegate> delegates))
                return;

            delegates.Remove(triggerCallback);

            if (!delegates.Any())
                subscriptions.Remove(trigger);
        }

        #endregion Public Methods

        #region Private Methods

        private void InputsMan_KeyboardStateChanged(object sender, KeyboardStateEventArgs e)
        {
            foreach (var mapping in mappings)
            {
                if (!inputsMan.IsPressed((int)mapping.Key))
                    continue;

                foreach (var triggers in mapping.Value)
                    Raise(triggers);
            }
        }

        private void Raise(Enum trigger)
        {
            if (!subscriptions.TryGetValue(trigger, out HashSet<Delegate> delegates))
                return;

            foreach (var deleg in delegates)
            {
                ((ActionBinderCallback<Enum>)deleg).Invoke(trigger);
            }    
        }

        #endregion Private Methods
    }
}