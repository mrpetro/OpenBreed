using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Physics.Systems.Extensions;

namespace OpenBreed.Wecs.Physics.Systems
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    [SystemCategory(CommonCategories.Physics)]
    public class UpdateDynamicBodySystem : IUpdatableSystem
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public UpdateDynamicBodySystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int PhaseId => throw new System.NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = this.worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                if (entity.WorldId != context.WorldId)
                    continue;

                entity.UpdateDynamics(entityMan);
            }
        }

        #endregion Public Methods
    }
}