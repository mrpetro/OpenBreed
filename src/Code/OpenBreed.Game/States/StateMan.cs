using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Game.States
{
    public class StateMan
    {
        #region Private Fields

        private BaseState activeGameState = null;
        private Dictionary<string, BaseState> states = new Dictionary<string, BaseState>();

        #endregion Private Fields

        #region Public Constructors

        public StateMan(Program program)
        {
            Program = program;
        }

        #endregion Public Constructors

        #region Public Properties

        public Program Program { get; }

        #endregion Public Properties

        #region Public Methods

        public void ChangeState(string stateId)
        {
            BaseState state;
            if (!states.TryGetValue(stateId, out state))
                throw new InvalidOperationException($"Unknown state id '{stateId}'");

            if (activeGameState != null)
                activeGameState.LeaveState();

            activeGameState = state;
            activeGameState.EnterState();
        }

        public void OnLoad()
        {
            activeGameState.OnLoad();
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            activeGameState.OnRenderFrame(e);
        }

        public void OnUpdate(FrameEventArgs e)
        {
            activeGameState.OnUpdate(e);
        }

        public void ProcessInputs(FrameEventArgs e)
        {
            activeGameState.ProcessInputs(e);
        }

        public void RegisterState(BaseState state)
        {
            states.Add(state.Name, state);
            state.OnRegister(this);
        }

        internal void OnResize(Rectangle clientRectangle)
        {
            activeGameState.OnResize(clientRectangle);
        }


        #endregion Public Methods
    }
}