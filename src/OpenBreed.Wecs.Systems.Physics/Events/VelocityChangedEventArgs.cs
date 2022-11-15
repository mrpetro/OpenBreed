using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    public class VelocityChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public VelocityChangedEventArgs(Vector2 value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Value { get; }

        #endregion Public Properties
    }
}