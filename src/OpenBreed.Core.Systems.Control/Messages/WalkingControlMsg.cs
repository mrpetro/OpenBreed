using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Messages
{
    public struct WalkingControlMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "WALKING_CONTROL";

        #endregion Public Fields

        #region Public Constructors

        public WalkingControlMsg(IEntity entity, Vector2 direction)
        {
            Entity = entity;
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}