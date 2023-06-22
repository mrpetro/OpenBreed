using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class UpdatableSystemBase<TSystem> : SystemBase<TSystem>, IUpdatableSystem where TSystem : ISystem
    {
        #region Private Fields

        private readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion Private Fields

        #region Public Methods

        public virtual void Update(IWorldContext context)
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

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        protected abstract void UpdateEntity(IEntity entity, IWorldContext context);

        #endregion Protected Methods
    }
}