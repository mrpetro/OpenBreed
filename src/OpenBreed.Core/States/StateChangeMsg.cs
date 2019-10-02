﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.States
{
    public class StateChangeMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "STATE_CHANGE";

        #endregion Public Fields

        #region Public Constructors

        public StateChangeMsg(IEntity entity, string fsmName, string stateId)
        {
            Entity = entity;
            FsmName = fsmName;
            StateId = stateId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public string FsmName { get; }
        public string StateId { get; }

        #endregion Public Properties
    }
}