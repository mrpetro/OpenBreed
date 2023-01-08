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

        protected SystemBase()
        {
        }

        #endregion Protected Constructors
    }

    public abstract class SystemBase : ISystem
    {
        #region Private Fields

        #endregion Private Fields

        #region Protected Constructors

        protected SystemBase()
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        public int PhaseId { get; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        public abstract bool ContainsEntity(IEntity entity);

        public abstract void AddEntity(IEntity entity);

        public abstract void RemoveEntity(IEntity entity);

        #endregion Protected Methods
    }
}