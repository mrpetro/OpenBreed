﻿using OpenBreed.Core.Common.Helpers;
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

        public WalkingControlMsg(int entityId, Vector2 direction)
        {
            EntityId = entityId;
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}