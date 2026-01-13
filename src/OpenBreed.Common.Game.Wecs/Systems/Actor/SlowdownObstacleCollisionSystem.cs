using OpenBreed.Common.Game;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenTK.Mathematics;

namespace OpenBreed.Common.Game.Wecs.Systems.Actor
{
    public class SlowdownObstacleCollisionSystem : IOnEntityCollisionSystem
    {
        #region Public Constructors

        public SlowdownObstacleCollisionSystem()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.SlowdownObstacle;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture aFixture, IEntity aEntity, IFixture bFixture, IEntity bEntity, float dt, Vector2 projection)
        {
            //if (entityA.State is "Slowdown")
            //    return;

            var velCmp = aEntity.Get<VelocityComponent>();

            velCmp.Value = Vector2.Multiply(velCmp.Value, 0.5f);

            //entityA.State = "Slowdown";

            Console.WriteLine("Slowdown");
        }

        #endregion Public Methods
    }
}