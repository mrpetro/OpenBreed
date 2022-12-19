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

        private readonly List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Protected Constructors

        protected UpdatableSystemBase(IWorld world) :
                    base(world)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public void Update(IWorldContext context)
        {
            if (context.Paused)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].Contains<PauseImmuneComponent>())
                        UpdateEntity(entities[i], context);
                }
            }
            else
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    UpdateEntity(entities[i], context);
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