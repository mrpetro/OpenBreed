using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Builders
{
    public class TimerComponentBuilder : BaseComponentBuilder<TimerComponentBuilder, TimerComponent>
    {
        #region Private Fields

        internal List<TimerData> Items { get; }

        #endregion Private Fields

        #region Protected Constructors

        protected TimerComponentBuilder(ICore core) : base(core)
        {
            Items = new List<TimerData>();
        }

        #endregion Protected Constructors

        #region Internal Properties

        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new TimerComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new TimerComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (Items.Count != index - 1)
                throw new InvalidOperationException("Incorrect usage");

            throw new NotImplementedException();

            //var toAdd = ToStringArray(value);

            //if(toAdd.Count != 2)
            //    throw new InvalidOperationException("Incorrect usage");

            //var fsm = Core.StateMachines.GetByName(toAdd[0]);

            //if(fsm == null)
            //    throw new InvalidOperationException($"FSM '{toAdd[0]}' does not exist.");

            //var stateId = fsm.GetStateIdByName(toAdd[1]);

            //Items.Add(new TimerData() { FsmId = fsm.Id, StateId = stateId });
        }

        #endregion Public Methods
    }
}