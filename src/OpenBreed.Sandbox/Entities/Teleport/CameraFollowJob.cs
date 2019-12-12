using OpenBreed.Core;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public class CameraFollowJob : IJob
    {
        #region Private Fields

        private IEntity cameraEntity;
        private IEntity targetEntity;

        #endregion Private Fields

        #region Public Constructors

        public CameraFollowJob(IEntity cameraEntity, IEntity targetEntity)
        {
            this.cameraEntity = cameraEntity;
            this.targetEntity = targetEntity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            var cameraPos = cameraEntity.Components.OfType<Position>().First();
            var targetPos = targetEntity.Components.OfType<Position>().First();
            var targetAabb = targetEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var offset = new Vector2(targetAabb.Width / 2.0f, targetAabb.Height / 2.0f);
            cameraPos.Value = targetPos.Value + offset;
        }

        public void Update(float dt)
        {
            var cameraPos = cameraEntity.Components.OfType<Position>().First();
            var targetPos = targetEntity.Components.OfType<Position>().First();
            var targetAabb = targetEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var offset = new Vector2(targetAabb.Width / 2.0f, targetAabb.Height / 2.0f);
            cameraPos.Value = targetPos.Value + offset;
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
