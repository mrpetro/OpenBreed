using System;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Event args for event that occurs when animation frame is changed
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