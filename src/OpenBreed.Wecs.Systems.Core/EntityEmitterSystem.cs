using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(EntityEmitterComponent))]
    public class EntityEmitterSystem : UpdatableSystemBase<EntityEmitterSystem>
    {
        #region Private Fields

        private readonly IEntityFactory entityFactory;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public EntityEmitterSystem(
            IWorld world,
            IEntityFactory entityFactory,
            IEventsMan eventsMan) : base(world)
        {
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var emitEntityComponent = entity.Get<EntityEmitterComponent>();

            if (emitEntityComponent is null)
                return;

            var toEmit = emitEntityComponent.ToEmit;

            for (int i = 0; i < toEmit.Count; i++)
            {
                //var entityTemplate = entityFactory.Create(toEmit[i]);

                //entityTemplate.Se
                //soundMan.PlaySample(toEmit[i]);
                eventsMan.Raise(null, new EmitEntityEvent(entity.Id, 0));
            }

            toEmit.Clear();
        }

        #endregion Protected Methods
    }
}