﻿using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Modules.Physics.Messages;

namespace OpenBreed.Game.Components.States
{
    public class OpenedState : IState
    {
        #region Private Fields

        private readonly int leftTileId;
        private readonly int rightTileId;
        private IEntity[] doorParts;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState(string id, int leftTileId, int rightTileId)
        {
            Id = id;
            this.leftTileId = leftTileId;
            this.rightTileId = rightTileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostMsg(new SpriteOffMsg(Entity));

            foreach (var part in doorParts)
                Entity.PostMsg(new BodyOffMsg(part));

            Entity.PostMsg(new TileSetMsg(doorParts[0], leftTileId));
            Entity.PostMsg(new TileSetMsg(doorParts[1], rightTileId));
            Entity.PostMsg(new TextSetMsg(Entity, "Door - Opened"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            doorParts = Entity.World.Systems.OfType<GroupSystem>().First().GetGroup(Entity).ToArray();
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Close":
                    return "Closing";
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

    }
}