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
        #region Protected Constructors

        protected MatchingSystemBase()
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void AddEntity(IEntity entity);

        public abstract bool ContainsEntity(IEntity entity);

        public abstract void RemoveEntity(IEntity entity);

        #endregion Public Methods
    }
}