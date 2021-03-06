﻿using OpenBreed.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Control.Commands
{
    public class FollowerSetTargetCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "FOLLOWER_SET_TARGET";

        #endregion Public Fields

        #region Public Constructors

        public FollowerSetTargetCommand(int followerEntityId, int targetEntityId)
        {
            EntityId = followerEntityId;
            TargetEntityId = targetEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TargetEntityId { get; }

        public int EntityId { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
