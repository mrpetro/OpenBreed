using OpenBreed.Core.Common.Helpers;
using OpenTK;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Events
{
    public class ControlFireChangedEvent : IEvent
    {
        #region Public Fields

        public const string TYPE = "CONTROL_FIRE_CHANGED";

        #endregion Public Fields

        #region Public Constructors

        public ControlFireChangedEvent(bool fire)
        {
            Fire = fire;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Fire { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}