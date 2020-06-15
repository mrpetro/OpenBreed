using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Core
{
    public class EntityJobEx2 : IJob
    {
        #region Private Fields

        private IEntity entity;
        private Action action;

        #endregion Private Fields

        #region Public Constructors

        public EntityJobEx2(IEntity entity, Action action)
        {
            this.entity = entity;
            this.action = action;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public void Execute()
        {
            action.Invoke();
            Complete(this);
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods
    }
}