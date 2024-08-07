﻿using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    public class ItemPickupSystem : EventSystem<ActorCollisionEvent, ItemPickupSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public ItemPickupSystem(IEventsMan eventsMan, IEntityMan entityMan)
            : base(eventsMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(object sender, ActorCollisionEvent e)
        {
            var actorEntity = entityMan.GetById(e.EntityId);
            var otherEntity = entityMan.GetById(e.OtherEntityId);
        }

        #endregion Public Methods
    }
}