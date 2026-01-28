using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(typeof(OnTriggerComponent))]
    [SystemCategory(CommonCategories.General)]
    public class OnWorldUpdateTriggerSystem : IUpdatableSystem
    {
        private readonly IWorldMan worldMan;
        #region Private Fields

        private readonly IEntityTriggerMan entityTriggerMan;

        #endregion Private Fields

        #region Public Constructors

        public OnWorldUpdateTriggerSystem(
            IWorldMan worldMan,
            IEntityTriggerMan entityTriggerMan)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityTriggerMan = entityTriggerMan ?? throw new System.ArgumentNullException(nameof(entityTriggerMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                entityTriggerMan.TryOnTrigger<IOnWorldUpdateActionSystem>(
                    "UpdateWorld",
                    entity,
                    entity,
                    (system) => system.OnUpdate(entity, world));
            }
        }

        #endregion Public Methods
    }
}