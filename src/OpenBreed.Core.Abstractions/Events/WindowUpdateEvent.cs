using System;

namespace OpenBreed.Core.Abstractions.Events
{
    public class WindowUpdateEvent : EventArgs
    {
        #region Public Constructors

        public WindowUpdateEvent(IUpdateContext context, float dt)
        {
            Context = context;
            Dt = dt;
        }

        #endregion Public Constructors

        #region Public Properties

        public IUpdateContext Context { get; }

        public float Dt { get; }

        #endregion Public Properties
    }
}