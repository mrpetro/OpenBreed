using OpenBreed.Core.Builders;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Components
{
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
        public bool Enabled { get; internal set; }
        public double Interval { get; internal set; }

        #endregion Public Properties
    }

    public class TimerComponent : IEntityComponent
    {
        #region Public Constructors

        public TimerComponent(TimerComponentBuilder builder)
        {
            Items = builder.Items.ToList();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<TimerData> Items { get; }

        #endregion Public Properties
    }
}