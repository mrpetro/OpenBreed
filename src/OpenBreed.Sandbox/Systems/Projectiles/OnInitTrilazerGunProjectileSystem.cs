using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
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
    public class OnInitTrilazerGunProjectileSystem : IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public OnInitTrilazerGunProjectileSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            [EntityTriggerActionFilter("EnterWorld", "TrilazerGunProjectileInit")]
            EntityEnteredEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var dir = entity.GetThrust().Normalized();
            var degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y);
            FormattableString animName = $"Vanilla/Common/Projectile/TrilazerGun/High/{degree:0.0}";
            var animId = services.Clips.GetId(animName.ToString(CultureInfo.InvariantCulture));
            entity.PlayAnimation(0, animId);

            services.Triggers.OnLifetimeEnd(
                entity,
                Explode,
                true);

            void Explode(IEntity entity, LifetimeEndEvent e)
            {
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