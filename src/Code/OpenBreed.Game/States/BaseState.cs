using System;
using OpenTK;

namespace OpenBreed.Game.States
{
    public abstract class BaseState
    {


        #region Public Properties

        public StateMan StateMan { get; private set; }
        public abstract string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual void OnRenderFrame(FrameEventArgs e)
        {
        }

        public virtual void OnUpdate(FrameEventArgs e)
        {
        }

        public virtual void ProcessInputs(FrameEventArgs e)
        {
        }

        #endregion Public Methods

        #region Internal Methods

        internal virtual void EnterState()
        {
            Console.Write($"Entering state '{Name}'");
        }

        internal virtual void LeaveState()
        {
            Console.Write($"Leaving state '{Name}'");
        }

        #endregion Internal Methods

        #region Protected Methods

        protected void ChangeState(string stateId)
        {
            StateMan.ChangeState(stateId);
        }

        internal void OnRegister(StateMan stateMan)
        {
            StateMan = stateMan;
        }

        #endregion Protected Methods
    }
}