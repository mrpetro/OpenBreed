using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenTK.Graphics.ES20;
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
    public class Projectile2TriggerCollisionHandlerSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        private readonly IScriptMan scriptMan;
        private readonly IEntityTriggerMan entityTriggerMan;

        #endregion Private Fields

        #region Public Constructors

        public Projectile2TriggerCollisionHandlerSystem(
            IEventsMan eventsMan,
            IScriptMan scriptMan,
            IEntityTriggerMan entityTriggerMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.scriptMan = scriptMan ?? throw new ArgumentNullException(nameof(scriptMan));
            this.entityTriggerMan = entityTriggerMan ?? throw new ArgumentNullException(nameof(entityTriggerMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.Projectile;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.FullObstacle;
                yield return ColliderTypes.ActorBody;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture projectileFixture, IEntity projectileEntity, IFixture obstacleFixture, IEntity obstacleEntity, float dt, Vector2 projection)
        {
            var sourceEntityId = projectileEntity.GetSourceEntityId();

            //Ignore hitting source entity
            if (sourceEntityId == obstacleEntity.Id)
            {
                return;
            }

            if (entityTriggerMan.TryOnTrigger<IOnProjectileHitObstacleSystem>(
                "ObstacleCollision",
                obstacleEntity,
                projectileEntity,
                (system) => system.OnHit(projectileFixture, projectileEntity, obstacleFixture, obstacleEntity, projection)))
            {
                return;
            }

            //var actionNames = projectileEntity.GetActionsOnTrigger("ObstacleCollision");

            //bool result = true;

            //foreach (var actionName in actionNames)
            //{
            //    if (entityTriggerMan.TryGetTriggerSystem("ObstacleCollision", actionName, out IOnProjectileHitObstacleSystem system))
            //    {
            //        system.OnHit(projectileFixture, projectileEntity, obstacleFixture, obstacleEntity, projection);
            //        result = false;
            //    }
            //}

            //if (result)
            //{
            //    return;
            //}

            if (entityTriggerMan.TryOnTrigger("ObstacleCollision", obstacleEntity, projectileEntity))
            {
                return;
            }

            scriptMan.TryOnCollision(projectileEntity, obstacleEntity, projection);
        }

        #endregion Public Methods
    }
}