﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct SpriteOnMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "SPRITE_ON";

        #endregion Public Fields

        #region Public Constructors

        public SpriteOnMsg(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}