using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Fsm
{
    public interface IMachineStateTemplate
    {
        #region Public Properties

        string FsmName { get; set; }
        string StateName { get; set; }

        #endregion Public Properties
    }

    public interface IFsmStateTemplate
    {
        #region Public Properties

        string Name { get; set; }
        string Value { get; set; }

        #endregion Public Properties
    }

    public interface IFsmComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        IEnumerable<IMachineStateTemplate> States { get; }
        IEnumerable<IFsmStateTemplate> ExStates { get; }


        #endregion Public Properties
    }

    public class FsmComponent : IEntityComponent
    {
        #region Private Fields

        private readonly Dictionary<Type, Enum> exStates = new Dictionary<Type, Enum>();

        #endregion Private Fields

        #region Public Constructors

        public FsmComponent(FsmComponentBuilder builder)
        {
            States = builder.States.ToList();
            exStates = builder.ExStates;
        }

        #endregion Public Constructors

        //public FsmComponent(FsmComponentBuilder builder)
        //{
        //    States = builder.States.ToList();
        //}

        #region Public Properties

        public List<MachineState> States { get; }

        #endregion Public Properties

        #region Public Methods

        public TState Get<TState>() where TState : Enum
        {
            return (TState)exStates[typeof(TState)];
        }

        public void Set<TState>(TState value) where TState : Enum
        {
            exStates[typeof(TState)] = value;
        }

        #endregion Public Methods
    }

    public class FsmComponentBuilder
    {
        #region Internal Fields

        internal readonly List<MachineState> States = new List<MachineState>();
        internal readonly new Dictionary<Type, Enum> ExStates = new Dictionary<Type, Enum>();

        #endregion Internal Fields

        #region Private Fields

        private readonly IFsmMan fsmMan;

        #endregion Private Fields

        #region Public Constructors

        public FsmComponentBuilder(IFsmMan fsmMan)
        {
            this.fsmMan = fsmMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public FsmComponent Build()
        {
            return new FsmComponent(this);
        }

        public void AddState(string fsmName, string stateName)
        {
            var fsm = fsmMan.GetByName(fsmName);

            if (fsm == null)
                throw new InvalidOperationException($"FSM '{fsmName}' does not exist.");

            var stateId = fsm.GetStateIdByName(stateName);

            States.Add(new MachineState(fsm.Id, stateId));
        }

        internal void SetState(Enum value)
        {
            ExStates[value.GetType()] = value;
        }

        #endregion Public Methods
    }
}