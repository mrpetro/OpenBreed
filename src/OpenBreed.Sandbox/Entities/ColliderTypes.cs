using OpenBreed.Core.Managers;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Entities
{
    public static class ColliderTypes
    {
        #region Public Properties

        public static int ActorBody { get; private set; }
        public static int ActorTrigger { get; private set; }
        public static int DoorOpenTrigger { get; private set; }
        public static int FullObstacle { get; private set; }
        public static int SlopeObstacle { get; private set; }
        public static int SlowdownObstacle { get; private set; }
        public static int ActorOnlyObstacle { get; private set; }
        public static int WorldExitTrigger { get; private set; }
        public static int TeleportEntryTrigger { get; private set; }
        public static int Projectile { get; private set; }
        public static int Pickable { get; private set; }
        public static int Trigger { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterAbtaColliders(this ICollisionMan<IEntity> collisions)
        {
            ActorBody = collisions.RegisterGroup("ActorBody");
            ActorTrigger = collisions.RegisterGroup("ActorTrigger");
            DoorOpenTrigger = collisions.RegisterGroup("DoorOpenTrigger");
            Projectile = collisions.RegisterGroup("Projectile");
            FullObstacle = collisions.RegisterGroup("FullObstacle");
            SlopeObstacle = collisions.RegisterGroup("SlopeObstacle");
            ActorOnlyObstacle = collisions.RegisterGroup("ActorOnlyObstacle");
            SlowdownObstacle = collisions.RegisterGroup("SlowdownObstacle");
            WorldExitTrigger = collisions.RegisterGroup("WorldExitTrigger");
            TeleportEntryTrigger = collisions.RegisterGroup("TeleportEntryTrigger");
            Trigger = collisions.RegisterGroup("Trigger");
        }

        #endregion Public Methods
    }
}