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
            entity.Subscribe(AnimChangedEvent.TYPE, OnAnimChanged);
            entity.Subscribe(AnimStoppedEvent.TYPE, OnAnimStopped);
            entity.PostMsg(new PlayAnimMsg(entity, animName));
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {
            entity.Unsubscribe(AnimChangedEvent.TYPE, OnAnimChanged);
            entity.Unsubscribe(AnimStoppedEvent.TYPE, OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, IEvent e)
        {
            Complete(this);
        }

        private void OnAnimChanged(object sender, IEvent e)
        {
            HandleFrameChangeEvent((IEntity)sender, (AnimChangedEvent)e);
        }

        private void HandleFrameChangeEvent(IEntity entity, AnimChangedEvent systemEvent)
        {
            var cameraCmp = entity.Components.OfType<ICameraComponent>().First();
            cameraCmp.Brightness = (float)systemEvent.Frame;
        }

        #endregion Private Methods
    }
}
