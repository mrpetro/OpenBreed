using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnActorTouchLandMineTriggerSystem : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchLandMineTriggerSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "Detonate";

        #endregion Public Properties

        #region Public Methods

        public void OnTouch(
            IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity triggerEntity,
            Vector2 projection)
        {
            if (!actorEntity.HasHealth())
            {
                return;
            }

            var mineEntity = triggerEntity;

            Explode(mineEntity, actorEntity);
        }

        #endregion Public Methods

        #region Private Methods

        void Explode(IEntity mineEntity, IEntity actorEntity)
        {
            var mapEntity = services.Entities.GetMapEntity(mineEntity.WorldId);
            var stampId = services.Stamps.GetByName("Vanilla/L1/MineCrater").Id;
            var pos = mineEntity.GetPosition();

            mapEntity.PutStampAtEntityPosition(mineEntity, stampId, 0);

            var soundId = services.Sounds.GetByName("Vanilla/Common/LandMine/Explosion");

            var duration = services.Sounds.GetDuration(soundId);

            mineEntity.EmitSound(soundId);

            mineEntity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                .SetOption("flavor", "Big")
                .SetOption("startX", pos.X + 8)
                .SetOption("startY", pos.Y + 8)
                .Finish();


            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var nextDoorCell = services.Entities.GetEntityByDataGrid(services.Worlds, mineEntity, i, j);

                    services.Factory.CreateSlowdown(services.Worlds, mineEntity, mineEntity.WorldId, i, j);

                    if (nextDoorCell != null)
                    {
                        services.Worlds.RequestRemoveEntity(nextDoorCell);
                        services.Entities.RequestErase(nextDoorCell);
                    }
                }

            }

            if (actorEntity.HasHealth())
            {
                mineEntity.InflictDamage(10, actorEntity.Id);
            }

            services.Logger.LogInformation("Boom!");
        }



        #endregion Private Methods
    }
}