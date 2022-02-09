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