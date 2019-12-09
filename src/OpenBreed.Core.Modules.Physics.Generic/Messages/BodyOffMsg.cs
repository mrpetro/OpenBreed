﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Messages
{
    public struct BodyOffMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "BODY_OFF";

        #endregion Public Fields

        #region Public Constructors

        public BodyOffMsg(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
