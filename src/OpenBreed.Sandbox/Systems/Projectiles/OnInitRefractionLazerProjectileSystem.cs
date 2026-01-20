using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Animation.Systems.Events;
using OpenBreed.Wecs.Animation.Systems.Extensions;
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
    public class OnInitRefractionLazerProjectileSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public OnInitRefractionLazerProjectileSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "RefractionLazerProjectileInit";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity targetEntity, IEntity projectileEntity)
        {
            var dir = projectileEntity.GetThrust().Normalized();
            var degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y);
            FormattableString animName = $"Vanilla/Common/Projectile/RefractionLazer/High/{degree:0.0}";
            var animId = services.Clips.GetId(animName.ToString(CultureInfo.InvariantCulture));
            projectileEntity.PlayAnimation(0, animId);

            services.Triggers.OnLifetimeEnd(
                projectileEntity,
                Explode,
                true);

            void Explode(IEntity entity, LifetimeEndEvent e)
            {
                services.Entities.RequestErase(projectileEntity);

                var pos = entity.GetPosition();
                entity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                    .SetOption("startX", pos.X)
                    .SetOption("startY", pos.Y)
                    .SetOption("flavor", "Small")
                    .Finish();
            }
        }

        #endregion Public Methods
    }
}