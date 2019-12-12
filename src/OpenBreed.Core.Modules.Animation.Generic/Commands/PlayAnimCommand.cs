﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Commands
{
    public struct PlayAnimCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "PLAY_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PlayAnimCommand(int entityId, string id)
        {
            EntityId = entityId;
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public string Id { get; }

        #endregion Public Properties
    }
}