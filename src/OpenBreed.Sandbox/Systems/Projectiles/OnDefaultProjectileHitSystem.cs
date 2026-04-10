using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Core.Abstractions;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Helpers;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Physics.Systems.Events;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnDefaultProjectileHitSystem : IOnProjectileHitObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public OnDefaultProjectileHitSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ObstacleCollision";

        public string ActionName => "ProjectileHit";

        #endregion Public Properties

        #region Public Methods

        public void OnHit(IFixture projectileFixture, IEntity projectileEntity, IFixture obstacleFixture, IEntity obstacleEntity, Vector2 projection)
        {
            var pos = projectileEntity.GetPosition();
            projectileEntity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                .SetOption("flavor", "Small")
                .SetOption("startX", pos.X)
                .SetOption("startY", pos.Y)
                .Finish();

            if (obstacleEntity.HasHealth())
            {
                projectileEntity.InflictDamage(10, obstacleEntity.Id);
            }

            services.Worlds.RequestRemoveEntity(projectileEntity);
            services.Entities.RequestErase(projectileEntity);
        }

        #endregion Public Methods
    }
}