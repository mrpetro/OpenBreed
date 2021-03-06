﻿using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Core.Commands
{
    public class TimerStartCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "TIMER_START";

        #endregion Public Fields

        #region Public Constructors

        public TimerStartCommand(int entityId, int timerId, double interval)
        {
            EntityId = entityId;
            TimerId = timerId;
            Interval = interval;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int TimerId { get; }
        public double Interval { get; }

        #endregion Public Properties
    }
}