using OpenBreed.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    public abstract class MatchingSystemBase<TSystem> : MatchingSystemBase where TSystem : IMatchingSystem
    {
        #region Protected Constructors

        protected MatchingSystemBase()
        {
        }

        #endregion Protected Constructors
    }

    public abstract class MatchingSystemBase : IMatchingSystem
    {
        #region Protected Fields

        protected readonly List<IEntity> entities = new List<IEntity>();

        #endregion Protected Fields

        #region Protected Constructors

        protected MatchingSystemBase()
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public virtual void AddEntity(IEntity entity) => entities.Add(entity);

        public virtual bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public virtual void RemoveEntity(IEntity entity) => entities.Remove(entity);

        #endregion Public Methods
    }
}