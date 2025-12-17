using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    public class UpdateDynamicBodySystem : IMatchingSystem, IUpdatableSystem
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