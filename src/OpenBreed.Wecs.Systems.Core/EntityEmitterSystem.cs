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
    public class EntityEmitterSystem : UpdatableSystemBase<EntityEmitterSystem>
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
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.triggerMan = triggerMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
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

                triggerMan.OnEntityEnteredWorld(emittedEntity, (e,args) =>
                {
                    eventsMan.Raise(entity, new EmitEntityEvent(emittedEntity.Id, entity.Id));

                }, singleTime: true);

                worldMan.RequestAddEntity(emittedEntity, context.World.Id);

                
            }

            toEmit.Clear();
        }

        #endregion Protected Methods
    }
}