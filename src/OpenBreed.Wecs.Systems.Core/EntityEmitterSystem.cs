using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(EntityEmitterComponent))]
    public class EntityEmitterSystem : IMatchingSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly IEntityFactory entityFactory;
        private readonly IEventsMan eventsMan;
        private readonly ITriggerMan triggerMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public EntityEmitterSystem(
            IEntityFactory entityFactory,
            IEventsMan eventsMan,
            ITriggerMan triggerMan,
            IWorldMan worldMan)
        {
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
            this.eventsMan = eventsMan ?? throw new System.ArgumentNullException(nameof(eventsMan));
            this.triggerMan = triggerMan ?? throw new System.ArgumentNullException(nameof(triggerMan));
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var emitEntityComponent = entity.Get<EntityEmitterComponent>();

            if (emitEntityComponent is null)
                return;

            var toEmit = emitEntityComponent.ToEmit;

            for (int i = 0; i < toEmit.Count; i++)
            {
                var entityEmit = toEmit[i];

                var pc = entity.Get<PositionComponent>();

                var templateBuilder = entityFactory.Create(entityEmit.TemplateName);

                foreach (var option in entityEmit.Options)
                {
                    templateBuilder.SetParameter(option.Key, option.Value);
                }

                var emittedEntity = templateBuilder.Build();
                emittedEntity.Add(SourceEntityComponent.Create(entity.Id));

                triggerMan.OnEntityEnteredWorld(emittedEntity, (e, args) =>
                {
                    eventsMan.Raise(new EmitEntityEvent(emittedEntity.Id, entity.Id));
                }, singleTime: true);

                worldMan.RequestAddEntity(emittedEntity, context.WorldId);
            }

            toEmit.Clear();
        }

        #endregion Private Methods
    }
}