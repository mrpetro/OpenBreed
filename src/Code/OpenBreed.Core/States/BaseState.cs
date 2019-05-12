using System;
using System.Drawing;
using OpenTK;

namespace OpenBreed.Core.States
{
    public abstract class BaseState
    {
        #region Public Properties

        public StateMan StateMan { get; private set; }
        public abstract string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual void OnResize(Rectangle clientRectangle)
        {
        }

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

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnLeave()
        {

        }

        internal void EnterState()
        {
            Console.Write($"Entering state '{Name}'");
            OnEnter();
        }

        internal void LeaveState()
        {
            Console.Write($"Leaving state '{Name}'");
            OnLeave();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected void ChangeState(string stateId)
        {
            StateMan.SetNextState(stateId);
        }

        internal void OnRegister(StateMan stateMan)
        {
            StateMan = stateMan;
        }

        #endregion Protected Methods
    }
}