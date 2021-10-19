using OpenBreed.Core.Managers;
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
        public static int StaticObstacle { get; private set; }
        public static int WorldExitTrigger { get; private set; }
        public static int TeleportEntryTrigger { get; private set; }
        public static int Projectile { get; private set; }


        //public static int Solid { get; private set; }
        //public static int Sensor { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void Initialize(ICollisionMan collisions)
        {
            ActorBody = collisions.RegisterGroup("ActorBody");
            ActorTrigger = collisions.RegisterGroup("ActorTrigger");
            DoorOpenTrigger = collisions.RegisterGroup("DoorOpenTrigger");
            Projectile = collisions.RegisterGroup("Projectile");
            StaticObstacle = collisions.RegisterGroup("StaticObstacle");
            WorldExitTrigger = collisions.RegisterGroup("WorldExitTrigger");
            TeleportEntryTrigger = collisions.RegisterGroup("TeleportEntryTrigger");

            //ActorBody = collisions.CreateColliderType("ActorBody");
            //DoorOpenTrigger = collisions.CreateColliderType("Door");
            //Projectile = collisions.CreateColliderType("Projectile");
            //StaticObstacle = collisions.CreateColliderType("StaticObstacle");
            //WorldExitTrigger = collisions.CreateColliderType("WorldExitTrigger");
            //TeleportEntryTrigger = collisions.CreateColliderType("TeleportEntryTrigger");
        }

        #endregion Public Methods
    }
}