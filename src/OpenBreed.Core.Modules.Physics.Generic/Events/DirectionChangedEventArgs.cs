using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Events
{
    public class DirectionChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public DirectionChangedEventArgs(Vector2 direction)
        {
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}
