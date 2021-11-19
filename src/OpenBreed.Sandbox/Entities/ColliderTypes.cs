﻿using OpenBreed.Core.Managers;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class ColliderTypes
    {
        #region Public Properties

        public static int ActorBody { get; private set; }
        public static int ActorTrigger { get; private set; }
        public static int DoorOpenTrigger { get; private set; }
        public static int ItemPickupTrigger { get; private set; }
        public static int FullObstacle { get; private set; }
        public static int ActorOnlyObstacle { get; private set; }
        public static int WorldExitTrigger { get; private set; }
        public static int TeleportEntryTrigger { get; private set; }
        public static int Projectile { get; private set; }
        public static int Pickable { get; private set; }

        //public static int Solid { get; private set; }
        //public static int Sensor { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void Initialize(ICollisionMan collisions)
        {
            ActorBody = collisions.RegisterGroup("ActorBody");
            ActorTrigger = collisions.RegisterGroup("ActorTrigger");
            DoorOpenTrigger = collisions.RegisterGroup("DoorOpenTrigger");
            ItemPickupTrigger = collisions.RegisterGroup("ItemPickupTrigger");
            Projectile = collisions.RegisterGroup("Projectile");
            FullObstacle = collisions.RegisterGroup("FullObstacle");
            ActorOnlyObstacle = collisions.RegisterGroup("ActorOnlyObstacle");
            WorldExitTrigger = collisions.RegisterGroup("WorldExitTrigger");
            TeleportEntryTrigger = collisions.RegisterGroup("TeleportEntryTrigger");
        }

        #endregion Public Methods
    }
}