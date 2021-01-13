using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Physics.Interface;

namespace OpenBreed.Sandbox.Entities
{
    public static class ColliderTypes
    {
        #region Public Properties

        public static int ActorBody { get; private set; }
        public static int DoorOpener { get; private set; }
        public static int DoorOpenTrigger { get; private set; }
        public static int StaticObstacle { get; private set; }
        public static int WorldExitTrigger { get; private set; }
        public static int TeleportEntryTrigger { get; private set; }
        public static int Projectile { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void Initialize(ICollisionMan collisions)
        {
            ActorBody = collisions.CreateColliderType("ActorBody");
            DoorOpener = collisions.CreateColliderType("DoorOpener");
            DoorOpenTrigger = collisions.CreateColliderType("Door");
            Projectile = collisions.CreateColliderType("Projectile");
            StaticObstacle = collisions.CreateColliderType("StaticObstacle");
            WorldExitTrigger = collisions.CreateColliderType("WorldExitTrigger");
            TeleportEntryTrigger = collisions.CreateColliderType("TeleportEntryTrigger");
        }

        #endregion Public Methods
    }
}