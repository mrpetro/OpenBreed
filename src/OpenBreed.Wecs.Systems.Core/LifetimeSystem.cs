using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(LifetimeComponent))]
    public class LifetimeSystem : UpdatableSystemBase<LifetimeSystem>
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public LifetimeSystem(
            IWorld world,
            IWorldMan worldMan,
            IEntityMan entityMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var lc = entity.Get<LifetimeComponent>();

            lc.TimeLeft -= context.Dt;

            if (lc.TimeLeft > 0.0f)
            {
                return;
            }

            lc.TimeLeft = 0.0f;
            worldMan.RequestRemoveEntity(entity);
            entityMan.RequestDestroy(entity);
        }

        #endregion Protected Methods
    }
}