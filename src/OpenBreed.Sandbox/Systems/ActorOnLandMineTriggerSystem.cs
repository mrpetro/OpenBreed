using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems
{
    public class ActorOnLandMineTriggerSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorOnLandMineTriggerSystem(
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

        public void OnTrigger(IEntity actorEntity, IEntity triggerEntity)
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