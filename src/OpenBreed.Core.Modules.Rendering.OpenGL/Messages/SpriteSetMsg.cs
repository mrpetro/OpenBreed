﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct SpriteSetMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "SPRITE_SET";

        #endregion Public Fields

        #region Public Constructors

        public SpriteSetMsg(IEntity entity, int imageId)
        {
            Entity = entity;
            ImageId = imageId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public int ImageId { get; }

        #endregion Public Properties
    }
}