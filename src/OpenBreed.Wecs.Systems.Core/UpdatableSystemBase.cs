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

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Methods

        public void Update(IWorldContext context)
        {
            if (context.Paused)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].Contains<PauseImmuneComponent>())
                        UpdateEntity(entities[i], context.Dt);
                }
            }
            else
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    UpdateEntity(entities[i], context.Dt);
                }
            }

        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(Entity entity, float dt);

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity) => entities.Add(entity);

        protected override void OnRemoveEntity(Entity entity) => entities.Remove(entity);

        #endregion Protected Methods
    }
}