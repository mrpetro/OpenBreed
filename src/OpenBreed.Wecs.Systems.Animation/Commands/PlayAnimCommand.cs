﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Animation.Commands
{
    public struct PlayAnimCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "PLAY_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PlayAnimCommand(int entityId, string id, int animatorId)
        {
            EntityId = entityId;
            AnimatorId = animatorId;
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int AnimatorId { get; }
        public string Id { get; }

        #endregion Public Properties
    }
}