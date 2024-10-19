using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using System.Runtime.CompilerServices;

namespace OpenBreed.Common.Game
{
    public static class ColliderTypes
    {
        #region Public Properties

        public static int ActorBody { get; private set; }
        public static int ActorSight { get; private set; }
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
            ActorBody = collisions.RegisterGroup(nameof(ActorBody));
            ActorSight = collisions.RegisterGroup(nameof(ActorSight));
            ActorTrigger = collisions.RegisterGroup(nameof(ActorTrigger));
            DoorOpenTrigger = collisions.RegisterGroup(nameof(DoorOpenTrigger));
            Projectile = collisions.RegisterGroup(nameof(Projectile));
            FullObstacle = collisions.RegisterGroup(nameof(FullObstacle));
            SlopeObstacle = collisions.RegisterGroup(nameof(SlopeObstacle));
            ActorOnlyObstacle = collisions.RegisterGroup(nameof(ActorOnlyObstacle));
            SlowdownObstacle = collisions.RegisterGroup(nameof(SlowdownObstacle));
            WorldExitTrigger = collisions.RegisterGroup(nameof(WorldExitTrigger));
            TeleportEntryTrigger = collisions.RegisterGroup(nameof(TeleportEntryTrigger));
            Trigger = collisions.RegisterGroup(nameof(Trigger));
        }

        #endregion Public Methods
    }
}