using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    public class PositionChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public PositionChangedEventArgs(Vector2 position)
        {
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Position { get; }

        #endregion Public Properties
    }
}
