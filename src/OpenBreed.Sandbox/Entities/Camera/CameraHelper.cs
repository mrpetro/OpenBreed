using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Animation.Interface;
using OpenBreed.Ecsw.Entities;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraHelper
    {
        public const string CAMERA_FADE_OUT = "Animations/Camera/Effects/FadeOut";
        public const string CAMERA_FADE_IN = "Animations/Camera/Effects/FadeIn";

        public static void CreateAnimations(ICore core)
        {
            var cameraEffectFadeOut = core.GetManager<IAnimMan>().Create(CAMERA_FADE_OUT, 10.0f);
            var fo = cameraEffectFadeOut.AddPart<float>(OnFrameUpdate, 1.0f);
            fo.AddFrame(0.0f, 10.0f);

            var cameraEffectFadeIn = core.GetManager<IAnimMan>().Create(CAMERA_FADE_IN, 10.0f);
            var fi = cameraEffectFadeIn.AddPart<float>(OnFrameUpdate, 0.0f);
            fi.AddFrame(1.0f, 10.0f);
        }

        private static void OnFrameUpdate(Entity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }
    }
}
