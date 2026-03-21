using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using System;
using System.Windows;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnLevelStartedSystem : IEventSystem<LevelStartedEvent>
    {
        private readonly IGameServices services;
        private readonly IEntityClass actorClass;

        public OnLevelStartedSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public int PlaySound(IEntity entity, string sampleName)
        {
            var soundId = services.Sounds.GetByName(sampleName);

            var duration = services.Sounds.GetDuration(soundId);

            entity.EmitSound(soundId);

            return duration;
        }

        public void OnEvent(LevelStartedEvent e)
        {
            var commentator = services.Entities.GetCommentator();

            var task = Core.Task.Create((t) => services.Say(t, commentator, "Vanilla/Common/Speech/Nash"));

            task.Then((t) => services.Say(t, commentator, "Vanilla/Common/Speech/IsDead"))
                .Then((t) => services.Say(t, commentator, "Vanilla/Common/Speech/YoureOnYourOwn"));

            task.Start();
        }
    }
}