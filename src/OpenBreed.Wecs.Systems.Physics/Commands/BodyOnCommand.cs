﻿using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Commands
{
    public struct BodyOnCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "BODY_ON";

        #endregion Public Fields

        #region Public Constructors

        public BodyOnCommand(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
