using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public class SequenceUpdateSystem<TSystem> : SystemBase<SequenceUpdateSystem<TSystem>>, IUpdatableSystem where TSystem :IEntityUpdateSubsystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IEntityUpdateSubsystem entityUpdateSystem;

        #endregion Private Fields

        #region Public Constructors

        public SequenceUpdateSystem(IEntityUpdateSubsystem entityUpdateSystem)
        {
            this.entityUpdateSystem = entityUpdateSystem;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IWorldContext context)
        {
            if (context.Paused)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].Contains<PauseImmuneComponent>())
                        entityUpdateSystem.Update(entities[i], context);
                }
            }
            else
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    entityUpdateSystem.Update(entities[i], context);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        #endregion Protected Methods
    }
}