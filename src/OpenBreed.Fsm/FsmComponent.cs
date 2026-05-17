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

    public interface IFsmComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        IEnumerable<IMachineStateTemplate> States { get; }

        #endregion Public Properties
    }

    public class FsmComponent : IEntityComponent
    {
        #region Public Constructors

        public FsmComponent(FsmComponentBuilder builder)
        {
            States = builder.States.ToList();
        }

        #endregion Public Constructors

        //public FsmComponent(FsmComponentBuilder builder)
        //{
        //    States = builder.States.ToList();
        //}

        #region Public Properties

        public List<MachineState> States { get; }

        #endregion Public Properties
    }

    public class FsmComponentBuilder
    {
        #region Internal Fields

        internal readonly List<MachineState> States = new List<MachineState>();

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

        #endregion Public Methods
    }
}