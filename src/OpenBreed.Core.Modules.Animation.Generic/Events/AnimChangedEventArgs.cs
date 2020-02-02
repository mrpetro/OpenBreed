using System;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Event arguments that are passed with ANIMATION_CHANGED event
    /// </summary>
    public class AnimChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public AnimChangedEventArgs(object frame)
        {
            Frame = frame;
        }

        #endregion Public Constructors

        #region Public Properties

        public object Frame { get; }

        #endregion Public Properties
    }
}