using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    internal class DefaultTriggerMan : ITriggerMan
    {
        #region Private Fields

        private Dictionary<Type, List<(Delegate ConditionFunction, Delegate Action, bool OneTime)>> logics = new Dictionary<Type, List<(Delegate ConditionFunction, Delegate Action, bool OneTime)>>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultTriggerMan(IEventsMan eventsMan)
        {
            EventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEventsMan EventsMan { get; }

        #endregion Public Properties

        #region Public Methods

        public void CreateTrigger<TEventArgs>(Func<TEventArgs, bool> conditionFunction, Action<TEventArgs> action, bool singleTime) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);
            if (!logics.TryGetValue(eventType, out List<(Delegate ConditionFunction, Delegate Action, bool OneTime)> list))
            {
                list = new List<(Delegate ConditionFunction, Delegate Action, bool OneTime)>();
                logics.Add(eventType, list);
                EventsMan.Subscribe<TEventArgs>(ConditionalAction);
            }

            list.Add((conditionFunction, action, singleTime));
        }

        public ITriggerBuilder NewTrigger() => new TriggerBuilder(this);

        #endregion Public Methods

        #region Private Methods

        private void ConditionalAction<TEventArgs>(TEventArgs args) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);

            if (!logics.TryGetValue(eventType, out List<(Delegate ConditionFunction, Delegate Action, bool OneTime)> list))
                return;

            var toRemove = new List<(Delegate ConditionFunction, Delegate Action, bool OneTime)>();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];

                if (ConditionalAction<TEventArgs>(args, (Func<TEventArgs, bool>)item.ConditionFunction, (Action<TEventArgs>)item.Action, item.OneTime))
                    toRemove.Add(item);
            }

            toRemove.ForEach(item => list.Remove(item));
        }

        private bool ConditionalAction<TEventArgs>(TEventArgs args, Func<TEventArgs, bool> conditionFunction, Action<TEventArgs> action, bool singleTime)
        {
            if (!conditionFunction.Invoke(args))
                return false;

            action.Invoke(args);

            return singleTime;
        }

        #endregion Private Methods
    }

    internal class TriggerBuilder : ITriggerBuilder
    {
        #region Public Constructors

        public TriggerBuilder(ITriggerMan triggerMan)
        {
            TriggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITriggerMan TriggerMan { get; }

        #endregion Public Properties
    }
}