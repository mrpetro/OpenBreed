using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraHelper
    {
        public const string CAMERA_FADE_OUT = "Animations/Camera/Effects/FadeOut";
        public const string CAMERA_FADE_IN = "Animations/Camera/Effects/FadeIn";

        public static void CreateAnimations(ICore core)
        {
            var cameraEffectFadeOut = core.Animations.Create<float>(CAMERA_FADE_OUT, OnFrameUpdate);
            cameraEffectFadeOut.AddFrame(1.0f, 0.0f);
            cameraEffectFadeOut.AddFrame(0.0f, 10.0f);

            var cameraEffectFadeIn = core.Animations.Create<float>(CAMERA_FADE_IN, OnFrameUpdate);
            cameraEffectFadeIn.AddFrame(0.0f, 0.0f);
            cameraEffectFadeIn.AddFrame(1.0f, 10.0f);
        }

        private static void OnFrameUpdate(IEntity entity, float nextValue)
        {
            var cameraCmp = entity.GetComponent<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }
    }
}
