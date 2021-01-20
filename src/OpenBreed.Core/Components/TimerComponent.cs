using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Components
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
        public bool Enabled { get; set; }
        public double Interval { get; set; }

        #endregion Public Properties
    }

    public class TimerComponent : IEntityComponent
    {
        #region Public Constructors

        public TimerComponent()
        {
            Items = new List<TimerData>();
        }

        //public TimerComponent(TimerComponentBuilder builder)
        //{
        //    Items = builder.Items.ToList();
        //}

        #endregion Public Constructors

        #region Public Properties

        public List<TimerData> Items { get; }

        #endregion Public Properties
    }

    public sealed class TimerComponentFactory : ComponentFactoryBase<ITimerComponentTemplate>
    {
        public TimerComponentFactory(ICore core) : base(core)
        {

        }

        protected override IEntityComponent Create(ITimerComponentTemplate template)
        {
            return new TimerComponent();
        }
    }
}