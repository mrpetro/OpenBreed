using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class UpdatableSystemBase<TSystem> : MatchingSystemBase<TSystem>, IUpdatableSystem where TSystem : IMatchingSystem
    {
        #region Protected Fields

        protected readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion Protected Fields

        #region Public Methods

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

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