using OpenBreed.Common;
using OpenBreed.Wecs.Components;
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

    public sealed class FsmComponentFactory : ComponentFactoryBase<IFsmComponentTemplate>
    {
        #region Private Fields

        private readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Internal Constructors

        internal FsmComponentFactory(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IFsmComponentTemplate template)
        {
            var builder = managerCollection.GetManager<FsmComponentBuilder>();

            foreach (var state in template.States)
                builder.AddState(state.FsmName, state.StateName);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class FsmComponentBuilder
    {
        #region Internal Fields

        internal readonly List<MachineState> States = new List<MachineState>();

        #endregion Internal Fields

        #region Private Fields

        private readonly IFsmMan fsmMan;

        #endregion Private Fields

        #region Internal Constructors

        internal FsmComponentBuilder(IFsmMan fsmMan)
        {
            this.fsmMan = fsmMan;
        }

        #endregion Internal Constructors

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