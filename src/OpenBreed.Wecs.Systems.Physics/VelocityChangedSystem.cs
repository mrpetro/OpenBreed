using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Categories;
using OpenBreed.Wecs.Systems.Physics.Events;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(typeof(VelocityComponent))]
    [SystemCategory(CommonCategories.Physics)]
    public class VelocityChangedSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public VelocityChangedSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan) : base(worldMan)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var velocity = entity.Get<VelocityComponent>();
            eventsMan.Raise(new VelocityChangedEvent(entity.Id, velocity.Value));
        }

        #endregion Protected Methods
    }
}