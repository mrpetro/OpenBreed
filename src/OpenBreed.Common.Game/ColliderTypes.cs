using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Abstractions.Primitives;
using System.Runtime.CompilerServices;

namespace OpenBreed.Common.Game
{
    public static class ColliderTypes
    {
        #region Public Properties

        public static int ActorBody { get; private set; }
        public static int EnemyBody { get; private set; }
        public static int EnemyTarget { get; private set; }
        public static int ActorSight { get; private set; }
        public static int ActorTrigger { get; private set; }
        public static int FullObstacle { get; private set; }
        public static int SlopeObstacle { get; private set; }
        public static int SlowdownObstacle { get; private set; }
        public static int ActorOnlyObstacle { get; private set; }
        public static int WorldExitTrigger { get; private set; }
        public static int TeleportTrigger { get; private set; }
        public static int Projectile { get; private set; }
        public static int Trigger { get; private set; }
        public static int OpenDoorTrigger { get; private set; }
        public static int PickupItemTrigger { get; private set; }
        public static int ReadSmartCardTrigger { get; private set; }
        public static int DetonateTrigger { get; private set; }
        public static int ExitMapTrigger { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterAbtaColliders(this ICollisionMan<IEntity> collisions)
        {
            ActorBody = collisions.RegisterGroup(nameof(ActorBody));
            EnemyBody = collisions.RegisterGroup(nameof(EnemyBody));
            EnemyTarget = collisions.RegisterGroup(nameof(EnemyTarget));
            ActorSight = collisions.RegisterGroup(nameof(ActorSight));
            ActorTrigger = collisions.RegisterGroup(nameof(ActorTrigger));
            Projectile = collisions.RegisterGroup(nameof(Projectile));
            FullObstacle = collisions.RegisterGroup(nameof(FullObstacle));
            SlopeObstacle = collisions.RegisterGroup(nameof(SlopeObstacle));
            ActorOnlyObstacle = collisions.RegisterGroup(nameof(ActorOnlyObstacle));
            SlowdownObstacle = collisions.RegisterGroup(nameof(SlowdownObstacle));
            WorldExitTrigger = collisions.RegisterGroup(nameof(WorldExitTrigger));
            TeleportTrigger = collisions.RegisterGroup(nameof(TeleportTrigger));
            Trigger = collisions.RegisterGroup(nameof(Trigger));
            OpenDoorTrigger = collisions.RegisterGroup(nameof(OpenDoorTrigger));
            PickupItemTrigger = collisions.RegisterGroup(nameof(PickupItemTrigger));
            ReadSmartCardTrigger = collisions.RegisterGroup(nameof(ReadSmartCardTrigger));
            DetonateTrigger = collisions.RegisterGroup(nameof(DetonateTrigger));
            ExitMapTrigger = collisions.RegisterGroup(nameof(ExitMapTrigger));
        }

        #endregion Public Methods
    }
}