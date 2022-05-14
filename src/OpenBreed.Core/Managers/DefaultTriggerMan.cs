using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    internal class DefaultTriggerMan : ITriggerMan
    {
        #region Public Constructors

        public DefaultTriggerMan(IEventsMan eventsMan)
        {
            EventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEventsMan EventsMan { get; }

        private Dictionary<Type, List<(Delegate ConditionFunction, Action Action, bool OneTime)>> logics = new Dictionary<Type, List<(Delegate ConditionFunction, Action Action, bool OneTime)>>();

        public void CreateTrigger<TEventArgs>(Func<TEventArgs, bool> conditionFunction, Action action, bool singleTime) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);
            if(!logics.TryGetValue(eventType, out List<(Delegate ConditionFunction, Action Action, bool OneTime)> list))
            {
                list = new List<(Delegate ConditionFunction, Action Action, bool OneTime)>();
                logics.Add(eventType, list);
                EventsMan.Subscribe<TEventArgs>(ConditionalAction);
            }

            list.Add((conditionFunction, action, singleTime));
        }

        void ConditionalAction<TEventArgs>(object sender, TEventArgs args) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);

            if (!logics.TryGetValue(eventType, out List<(Delegate ConditionFunction, Action Action, bool OneTime)> list))
                return;

            var toRemove = new List<(Delegate ConditionFunction, Action Action, bool OneTime)>();

            foreach (var item in list)
            {
                if (ConditionalAction<TEventArgs>(sender, args, (Func<TEventArgs, bool>)item.ConditionFunction, item.Action, item.OneTime))
                    toRemove.Add(item);
            }

            toRemove.ForEach(item => list.Remove(item));
        }

        bool ConditionalAction<TEventArgs>(object sender, TEventArgs args, Func<TEventArgs, bool> conditionFunction, Action action, bool singleTime)
        {
            if (!conditionFunction.Invoke(args))
                return false;

            action.Invoke();

            return singleTime;
        }

        public ITriggerBuilder NewTrigger() => new TriggerBuilder(this);

        #endregion Public Properties
    }

    internal class TriggerBuilder : ITriggerBuilder
    {
        public TriggerBuilder(ITriggerMan triggerMan)
        {
            TriggerMan = triggerMan;
        }

        public ITriggerMan TriggerMan { get; }
    }
}