using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    public class PausingSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public PausingSystem(IWorldMan worldMan)
        {
            this.worldMan = worldMan;

            RequireEntityWith<PauserComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, float dt)
        {
            var pauserComponent = entity.TryGet<PauserComponent>();

            if (pauserComponent is null)
                return;

            try
            {
                var world = worldMan.GetById(pauserComponent.WorldId);

                if (world is null)
                    return;

                if (pauserComponent.Pause == world.Paused)
                    return;

                if (pauserComponent.Pause)
                    world.Pause();
                else
                    world.Unpause();
            }
            finally
            {
                entity.Remove<PauserComponent>();
            }
        }

        #endregion Protected Methods
    }
}