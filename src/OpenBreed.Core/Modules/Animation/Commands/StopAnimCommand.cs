﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Commands
{
    public struct StopAnimCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "STOP_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public StopAnimCommand(int entityId, string id, int animatorId)
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