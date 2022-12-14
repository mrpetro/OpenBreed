using OpenBreed.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    public abstract class SystemBase<TSystem> : SystemBase where TSystem : ISystem
    {
        #region Protected Constructors

        protected SystemBase(IWorld world) : base(world)
        {
        }

        #endregion Protected Constructors
    }

    public abstract class SystemBase : ISystem
    {
        #region Private Fields

        private readonly HashSet<IEntity> toAdd = new HashSet<IEntity>();

        private readonly HashSet<IEntity> toRemove = new HashSet<IEntity>();

        #endregion Private Fields

        #region Protected Constructors

        protected SystemBase(IWorld world)
        {
            World = world;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        public int PhaseId { get; }

        /// <summary>
        /// World which owns this system
        /// </summary>
        public IWorld World { get; }

        #endregion Public Properties

        #region Public Methods

        public void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                toRemove.ForEach(entity => OnRemoveEntity(entity));
                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                toAdd.ForEach(entity => OnAddEntity(entity));
                toAdd.Clear();
            }
        }

        public bool HasEntity(IEntity entity)
        {
            if (toAdd.Contains(entity))
                return true;

            return ContainsEntity(entity);
        }

        public void RequestAddEntity(IEntity entity)
        {
            toAdd.Add(entity);
        }

        public void RequestRemoveEntity(IEntity entity)
        {
            toRemove.Add(entity);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract bool ContainsEntity(IEntity entity);

        protected abstract void OnAddEntity(IEntity entity);

        protected abstract void OnRemoveEntity(IEntity entity);

        #endregion Protected Methods
    }
}