using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class UpdatableMatchingSystemBase<TSystem> : MatchingSystemBase<TSystem>, IUpdatableSystem, IMatchingSystem where TSystem : IMatchingSystem
    {
        #region Protected Fields


        #endregion Protected Fields

        #region Public Methods

        public virtual void Update(IUpdateContext context)
        {
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