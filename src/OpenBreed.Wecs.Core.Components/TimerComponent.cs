using System.Collections.Generic;

namespace OpenBreed.Wecs.Core.Components
{
    public interface ITimerComponentTemplate : IComponentTemplate
    {
    }

    public class TimerData
    {
        #region Public Constructors

        public TimerData(int timerId, double interval)
        {
            TimerId = timerId;
            Interval = interval;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TimerId { get; }
        public double Interval { get; set; }

        #endregion Public Properties
    }

    [ComponentName("Timer")]
    public class TimerComponent : IEntityComponent
    {
        #region Public Constructors

        public TimerComponent()
        {
            Items = new List<TimerData>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<TimerData> Items { get; }

        #endregion Public Properties
    }
}