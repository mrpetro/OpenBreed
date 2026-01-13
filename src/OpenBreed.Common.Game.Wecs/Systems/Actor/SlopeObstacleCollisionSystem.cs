using OpenBreed.Common.Game;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenTK.Mathematics;

namespace OpenBreed.Common.Game.Wecs.Systems.Actor
{
    public class SlopeObstacleCollisionSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly DynamicResolver dynamicResolver;

        #endregion Private Fields

        #region Public Constructors

        public SlopeObstacleCollisionSystem(DynamicResolver dynamicResolver)
        {
            this.dynamicResolver = dynamicResolver;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.SlopeObstacle;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture aFixture, IEntity aEntity, IFixture bFixture, IEntity bEntity, float dt, Vector2 projection)
        {
            var metadata = bEntity.Get<MetadataComponent>();

            Vector2 slopeDirection;

            switch (metadata.Flavor)
            {
                case "DownLeft":
                    slopeDirection = new Vector2(0, 1);
                    break;

                case "UpLeft":
                    slopeDirection = new Vector2(0, -1);
                    break;

                case "UpRight":
                    slopeDirection = new Vector2(0, -1);
                    break;

                case "DownRight":
                    slopeDirection = new Vector2(0, 1);
                    break;

                default:
                    slopeDirection = new Vector2(0, 0);
                    break;
            }

            dynamicResolver.ResolveVsSlope(aEntity, bEntity, projection, slopeDirection);
        }

        #endregion Public Methods
    }
}