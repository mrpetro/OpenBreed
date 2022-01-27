using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using System;

namespace OpenBreed.Sandbox
{
    public class EntityJob : IJob
    {
        #region Private Fields

        private readonly Action action;

        #endregion Private Fields

        #region Public Constructors

        public EntityJob(Action action)
        {
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

    public class AnimStoppedEntityJob : IJob
    {
        #region Private Fields

        private readonly ITriggerMan triggerMan;
        private readonly Entity entity;
        private readonly Action action;

        #endregion Private Fields

        #region Public Constructors

        public AnimStoppedEntityJob(ITriggerMan triggerMan, Entity entity, Action action)
        {
            this.triggerMan = triggerMan;
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
            triggerMan.OnEntityAnimFinished(entity, OnTrigger, singleTime: true);

            action.Invoke();
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnTrigger(Entity e, AnimFinishedEventArgs args)
        {
            Complete(this);
        }

        #endregion Private Methods
    }
}