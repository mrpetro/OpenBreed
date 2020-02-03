﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Commands
{
    public struct StopAnimCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "STOP_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public StopAnimCommand(int entityId, string id)
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