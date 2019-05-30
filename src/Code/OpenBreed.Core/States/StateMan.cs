using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Core.States
{
    public class StateMan
    {
        #region Private Fields

        private BaseState nextState = null;
        private BaseState activeState = null;
        private Dictionary<string, BaseState> states = new Dictionary<string, BaseState>();

        #endregion Private Fields

        #region Public Constructors

        public StateMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void SetNextState(string stateId)
        {
            if (!states.TryGetValue(stateId, out nextState))
                throw new InvalidOperationException($"Unknown state id '{stateId}'");
        }

        public void ChangeState()
        {
            if (activeState != null)
                activeState.LeaveState();

            activeState = nextState;
            activeState.EnterState();
        }

        public void Update(float dt)
        {
            activeState.Update(dt);

            if (activeState != nextState)
                ChangeState();
        }

        public void ProcessInputs(FrameEventArgs e)
        {
            activeState.ProcessInputs(e);
        }

        public void RegisterState(BaseState state)
        {
            states.Add(state.Id, state);
            state.OnRegister(this);
        }

        public void OnResize(Rectangle clientRectangle)
        {
            activeState.OnResize(clientRectangle);
        }


        #endregion Public Methods
    }
}