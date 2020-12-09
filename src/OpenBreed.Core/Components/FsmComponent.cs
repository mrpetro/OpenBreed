using OpenBreed.Core.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Components
{
    public interface IMachineStateTemplate
    {
        string FsmName { get; set; }
        string StateName { get; set; }
    }

    public interface IFsmComponentTemplate : IComponentTemplate
    {
        IEnumerable<IMachineStateTemplate> States { get; }
    }

    public class MachineState
    {
        #region Public Fields

        public int FsmId;
        public int StateId;

        #endregion Public Fields
    }

    public class FsmComponent : IEntityComponent
    {
        #region Public Constructors

        public FsmComponent(FsmComponentBuilderEx builder)
        {
            States = builder.States.ToList();
        }

        //public FsmComponent(FsmComponentBuilder builder)
        //{
        //    States = builder.States.ToList();
        //}

        #endregion Public Constructors

        #region Public Properties

        public List<MachineState> States { get; }

        #endregion Public Properties
    }


    public sealed class FsmComponentFactory : ComponentFactoryBase<IFsmComponentTemplate>
    {
        #region Public Constructors

        public FsmComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IFsmComponentTemplate template)
        {
            var builder = FsmComponentBuilderEx.New(core);

            foreach (var state in template.States)
                builder.AddState(state.FsmName, state.StateName);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class FsmComponentBuilderEx
    {
        #region Internal Fields

        internal readonly List<MachineState> States = new List<MachineState>();

        #endregion Internal Fields

        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private FsmComponentBuilderEx(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Public Methods

        public static FsmComponentBuilderEx New(ICore core)
        {
            return new FsmComponentBuilderEx(core);
        }

        public FsmComponent Build()
        {
            return new FsmComponent(this);
        }

        public void AddState(string fsmName, string stateName)
        {
            var fsm = core.StateMachines.GetByName(fsmName);

            if (fsm == null)
                throw new InvalidOperationException($"FSM '{fsmName}' does not exist.");

            var stateId = fsm.GetStateIdByName(stateName);

            States.Add(new MachineState() { FsmId = fsm.Id, StateId = stateId });
        }

        #endregion Public Methods
    }
}