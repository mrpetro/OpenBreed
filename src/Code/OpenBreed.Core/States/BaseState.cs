using System;
using System.Drawing;
using OpenTK;

namespace OpenBreed.Core.States
{
    public abstract class BaseState : IState
    {
        #region Public Properties

        public StateMan StateMan { get; private set; }
        public abstract string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual void OnResize(Rectangle clientRectangle)
        {
        }

        public virtual void OnRenderFrame(FrameEventArgs e)
        {
        }

        public virtual string Update(float dt)
        {
            return null;
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

        public void EnterState()
        {
            Console.WriteLine($"Entering state '{Id}'");
            OnEnter();
        }

        public void LeaveState()
        {
            Console.WriteLine($"Leaving state '{Id}'");
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