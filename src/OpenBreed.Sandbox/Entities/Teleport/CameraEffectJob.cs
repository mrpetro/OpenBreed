using OpenBreed.Core;

using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
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
            entity.Core.Logging.Verbose($"Executing camera effect '{animName}.'");
            entity.Subscribe<AnimChangedEventArgs>(OnAnimChanged);
            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
            entity.PostCommand(new PlayAnimCommand(entity.Id, animName));
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {
            entity.Unsubscribe<AnimChangedEventArgs>(OnAnimChanged);
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            Complete(this);
        }

        private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        {
            var cameraCmp = entity.GetComponent<CameraComponent>();
            cameraCmp.Brightness = (float)e.Frame;
        }

        #endregion Private Methods
    }
}
