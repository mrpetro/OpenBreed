﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public class FollowedAddFollowerCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "FOLLOWED_ADD_FOLLOWER";

        #endregion Public Fields

        #region Public Constructors

        public FollowedAddFollowerCommand(int entityId, int followerEntityId)
        {
            EntityId = entityId;
            FollowerEntityId = followerEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int FollowerEntityId { get; }

        public int EntityId { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
