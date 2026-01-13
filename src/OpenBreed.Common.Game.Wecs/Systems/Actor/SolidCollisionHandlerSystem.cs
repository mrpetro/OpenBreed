using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Actor
{
    /// <summary>
    /// System that handles collisions between actor and trigger.
    /// </summary>
    public class SolidCollisionHandlerSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        private readonly IScriptMan scriptMan;
        private readonly DynamicResolver dynamicResolver;

        #endregion Private Fields

        #region Public Constructors

        public SolidCollisionHandlerSystem(DynamicResolver dynamicResolver)
        {
            this.dynamicResolver = dynamicResolver ?? throw new ArgumentNullException(nameof(dynamicResolver));
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.FullObstacle;
                yield return ColliderTypes.ActorBody;
                yield return ColliderTypes.ActorOnlyObstacle;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture sFixture, IEntity aEntity, IFixture bFixture, IEntity bEntity, float dt, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(aEntity, bEntity, dt, projection);
        }

        #endregion Public Methods
    }
}