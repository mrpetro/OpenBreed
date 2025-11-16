using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class UpdatableMatchingSystemBase : IMatchingSystem, IUpdatableSystem
    {
        private readonly IWorldMan worldMan;

        protected UpdatableMatchingSystemBase(IWorldMan worldMan)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
        }

        #region Public Methods

        public virtual void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            if (context.Paused)
            {
                foreach (var entity in entities)
                {
                    if (entity.Contains<PauseImmuneComponent>())
                        UpdateEntity(entity, context);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    UpdateEntity(entity, context);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(IEntity entity, IUpdateContext context);

        #endregion Protected Methods
    }
}