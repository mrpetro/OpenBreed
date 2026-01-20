using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(typeof(OnTriggerComponent))]
    [SystemCategory(CommonCategories.General)]
    public class OnAddEntityTriggerSystem : IOnAddEntitySystem, IMatchingSystem
    {
        #region Private Fields

        private readonly IEntityTriggerMan entityTriggerMan;

        #endregion Private Fields

        #region Public Constructors

        public OnAddEntityTriggerSystem(
            IEntityTriggerMan entityTriggerMan)
        {
            this.entityTriggerMan = entityTriggerMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnAddEntity(IWorld world, IEntity entity)
        {
            if (entityTriggerMan.TryOnTrigger<IOnAddEntityActionSystem>(
                "EnterWorld",
                entity,
                entity,
                (system) => system.OnAddEntity(world, entity)))
            {
                return;
            }
        }

        #endregion Public Methods
    }
}