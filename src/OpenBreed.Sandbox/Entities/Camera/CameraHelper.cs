using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
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
            var cameraEffectFadeOut = core.Animations.Anims.Create<float>(CAMERA_FADE_OUT);
            cameraEffectFadeOut.AddFrame(1.0f, 0.0f);
            cameraEffectFadeOut.AddFrame(0.0f, 10.0f);

            var cameraEffectFadeIn = core.Animations.Anims.Create<float>(CAMERA_FADE_IN);
            cameraEffectFadeIn.AddFrame(0.0f, 0.0f);
            cameraEffectFadeIn.AddFrame(1.0f, 10.0f);
        }

        public static IEntity AddCamera(ICore core, World world, float x, float y)
        {
            var cameraEntity = core.Entities.Create();

            cameraEntity.Add(new Animator(10.0f, true, 0, FrameTransition.LinearInterpolation));
            cameraEntity.Add(PositionComponent.Create(x, y));
            cameraEntity.Add(CameraComponent.Create(1.0f));
            cameraEntity.Add(new PauseImmuneComponent());
            world.AddEntity(cameraEntity);
            return cameraEntity;
        }
    }
}
