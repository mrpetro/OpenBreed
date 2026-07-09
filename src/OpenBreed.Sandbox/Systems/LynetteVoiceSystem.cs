using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenBreed.Sandbox.Systems
{
    public class LynetteVoiceSystem : IEventSystem<LevelStartedEvent>,  IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private const string NotifiedOnOtherPlayerDead = "NotifiedOnOtherPlayerDead";
        private const string NotifiedLazersOperational = "NotifiedLazersOperational";
        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public LynetteVoiceSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            LevelStartedEvent e,
            IWorld world)
        {
            var commentator = services.Entities.GetLynette();

            if (commentator.TryGetMetadata(NotifiedOnOtherPlayerDead, out bool result) && result)
            {
                return;
            }

            var task = Core.Task.Create((t) => services.Say(t, commentator, "Vanilla/Common/Speech/Nash"));

            task.Then((t) => services.Say(t, commentator, "Vanilla/Common/Speech/IsDead"))
                .Then((t) => services.Say(t, commentator, "Vanilla/Common/Speech/YoureOnYourOwn"));

            task.Start();

            commentator.SetMetadata(NotifiedOnOtherPlayerDead, true);
        }

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            [EntityWithTagFilter("TurretLazer")]
            EntityEnteredEvent e,
            IWorld world)
        {
            var commentator = services.Entities.GetLynette();

            if (commentator.TryGetMetadata(NotifiedLazersOperational, out bool result) && result)
            {
                return;
            }

            var task = Core.Task.Create((t) => services.Say(t, commentator, "Vanilla/Common/Speech/Warning"));
            task.Then((t) => services.Say(t, commentator, "Vanilla/Common/Speech/LazersOperational"));
            task.Start();

            commentator.SetMetadata(NotifiedLazersOperational, true);
        }

        #endregion Public Methods
    }
}