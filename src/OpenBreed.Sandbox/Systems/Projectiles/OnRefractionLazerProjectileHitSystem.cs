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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnRefractionLazerProjectileHitSystem : IOnProjectileHitObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public OnRefractionLazerProjectileHitSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ObstacleCollision";

        public string ActionName => "RefractionLazerProjectileOnHit";


        #endregion Public Properties

        #region Public Methods

        public void OnHit(IFixture projectileFixture, IEntity projectileEntity, IFixture obstacleFixture, IEntity obstacleEntity, Vector2 projection)
        {
            var pos = projectileEntity.GetPosition() + projection;
            projectileEntity.SetPosition(pos.X, pos.Y);

            var v = projectileEntity.GetThrust();
            var n = -projection.Normalized();

            var vp = -2 * (Vector2.Dot(n, v)) * n + v;

            projectileEntity.SetThrust(vp.X, vp.Y);
            var dir = vp.Normalized();
            var degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y);

            FormattableString animName = $"Vanilla/Common/Projectile/RefractionLazer/High/{degree:0.0}";
            var animId = services.Clips.GetId(animName.ToString(CultureInfo.InvariantCulture));
            projectileEntity.PlayAnimation(0, animId);

            if (obstacleEntity.HasHealth())
            {
                projectileEntity.InflictDamage(10, obstacleEntity.Id);
            }
        }

        #endregion Public Methods
    }
}