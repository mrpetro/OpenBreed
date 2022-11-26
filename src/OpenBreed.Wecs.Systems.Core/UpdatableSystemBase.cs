using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class UpdatableSystemBase : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

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

        protected abstract void UpdateEntity(IEntity entity, IWorldContext context);

        protected override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        protected override void OnAddEntity(IEntity entity) => entities.Add(entity);

        protected override void OnRemoveEntity(IEntity entity) => entities.Remove(entity);

        #endregion Protected Methods
    }
}