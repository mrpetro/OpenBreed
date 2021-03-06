﻿using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct SpriteOnCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "SPRITE_ON";

        #endregion Public Fields

        #region Public Constructors

        public SpriteOnCommand(int entityId)
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