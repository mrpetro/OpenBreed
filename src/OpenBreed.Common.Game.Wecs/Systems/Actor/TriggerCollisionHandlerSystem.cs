using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
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
    public class TriggerCollisionHandlerSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        private readonly IScriptMan scriptMan;

        #endregion Private Fields

        #region Public Constructors

        public TriggerCollisionHandlerSystem(IEventsMan eventsMan, IScriptMan scriptMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.scriptMan = scriptMan ?? throw new ArgumentNullException(nameof(scriptMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.Trigger;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture actorFixture, IEntity actorEntity, IFixture triggerFixture, IEntity triggerEntity, float dt, Vector2 projection)
        {
            eventsMan.Raise(new ActorCollisionEvent(actorEntity.Id, triggerEntity.Id));

            scriptMan.TryOnCollision(actorEntity, triggerEntity, projection);
            scriptMan.TryOnCollision(triggerEntity, actorEntity, projection);
        }

        #endregion Public Methods
    }
}