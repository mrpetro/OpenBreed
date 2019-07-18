using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Events
{
    public class ControlDirectionChangedEvent : ISystemEvent
    {
        #region Public Fields

        public const string TYPE = "CONTROL_DIRECTION_CHANGED";

        #endregion Public Fields

        #region Public Constructors

        public ControlDirectionChangedEvent(Vector2 direction)
        {
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; }
        public string Type { get { return TYPE; } }
        public object Data { get { return Direction; } }

        #endregion Public Properties
    }
}