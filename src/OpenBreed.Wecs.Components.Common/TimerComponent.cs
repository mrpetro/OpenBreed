﻿using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public interface ITimerComponentTemplate : IComponentTemplate
    {
    }

    public class TimerData
    {
        #region Public Constructors

        public TimerData(string name, int timerId, double interval)
        {
            Name = name;
            TimerId = timerId;
            Interval = interval;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }
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

        #endregion Public Constructors

        #region Public Properties

        public List<TimerData> Items { get; }

        #endregion Public Properties
    }

    public sealed class TimerComponentFactory : ComponentFactoryBase<ITimerComponentTemplate>
    {
        #region Internal Constructors

        public TimerComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ITimerComponentTemplate template)
        {
            return new TimerComponent();
        }

        #endregion Protected Methods
    }
}