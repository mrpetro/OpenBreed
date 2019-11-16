using OpenBreed.Core;
using OpenBreed.Core.Common;
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
    public class CameraGuyHelper
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
            var cameraGuy = core.Entities.Create();

            cameraGuy.Add(new Animator(10.0f, true, 0, FrameTransition.LinearInterpolation));
            cameraGuy.Add(Position.Create(x, y));
            cameraGuy.Add(CameraComponent.Create(1.0f));
            world.AddEntity(cameraGuy);
            return cameraGuy;
        }
    }
}
