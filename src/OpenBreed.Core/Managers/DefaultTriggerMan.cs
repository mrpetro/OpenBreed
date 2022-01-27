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

        #endregion Public Properties
    }
}