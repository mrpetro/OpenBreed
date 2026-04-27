using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Systems.Camera
{
    internal class CameraSettingPaletteSystem : IEventSystem<EntityEnteredEvent>
    {
        private readonly IGameServices services;

        public CameraSettingPaletteSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            EntityEnteredEvent e,
            IWorld world)
        {
            var camera = services.Entities.GetById(e.EntityId);

            if (camera.Tag is null || !camera.Tag.StartsWith("Camera."))
            {
                return;
            }

            var cameraWorld = services.Worlds.GetById(camera.WorldId);

            var paletteEntityTag = $"Palettes/{cameraWorld.Name}";

            var paletteEntity = services.Entities.GetByTag(paletteEntityTag).FirstOrDefault();

            if (paletteEntity is null)
            {
                services.Logger.LogError("Unable to set palette '{0}' on camera '{1}'.", paletteEntityTag, camera.Tag);
                return;
            }

            var pid = paletteEntity.GetPaletteId();
            camera.SetPaletteId(pid);

            services.Logger.LogTrace("Palette '{0}' set on camera '{1}'.", paletteEntityTag, camera.Tag);

        }
    }
}
