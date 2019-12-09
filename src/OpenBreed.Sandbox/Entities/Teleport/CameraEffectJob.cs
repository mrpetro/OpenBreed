using OpenBreed.Core;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public class CameraEffectJob : IJob
    {
        #region Private Fields

        private IEntity entity;
        private string animName;

        #endregion Private Fields

        #region Public Constructors

        public CameraEffectJob(IEntity entity, string animName)
        {
            this.entity = entity;
            this.animName = animName;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            entity.Subscribe(AnimationEventTypes.ANIMATION_CHANGED, OnAnimChanged);
            entity.Subscribe(AnimationEventTypes.ANIMATION_STOPPED, OnAnimStopped);
            entity.PostMsg(new PlayAnimMsg(entity.Id, animName));
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {
            entity.Unsubscribe(AnimationEventTypes.ANIMATION_CHANGED, OnAnimChanged);
            entity.Unsubscribe(AnimationEventTypes.ANIMATION_STOPPED, OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, EventArgs eventArgs)
        {
            Complete(this);
        }

        private void OnAnimChanged(object sender, EventArgs eventArgs)
        {
            HandleFrameChangeEvent((IEntity)sender, (AnimChangedEventArgs)eventArgs);
        }

        private void HandleFrameChangeEvent(IEntity entity, AnimChangedEventArgs systemEvent)
        {
            var cameraCmp = entity.Components.OfType<ICameraComponent>().First();
            cameraCmp.Brightness = (float)systemEvent.Frame;
        }

        #endregion Private Methods
    }
}
