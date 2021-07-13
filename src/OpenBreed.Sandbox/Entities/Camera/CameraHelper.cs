using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Common;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraHelper
    {
        public const string CAMERA_FADE_OUT = "Animations/Camera/Effects/FadeOut";
        public const string CAMERA_FADE_IN = "Animations/Camera/Effects/FadeIn";

        public static void CreateAnimations(ICore core)
        {

            var animationMan = core.GetManager<IClipMan>();
            var animatorMan = core.GetManager<IFrameUpdaterMan>();

            animatorMan.Register("Camera.Brightness", (FrameUpdater<float>)OnFrameUpdate);

            var dataLoaderFactory = core.GetManager<IDataLoaderFactory>();
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            var cameraFadeOut = animationLoader.Load("Animations/Camera/Effects/FadeOut");
            var cameraFadeIn = animationLoader.Load("Animations/Camera/Effects/FadeIn");

            //var cameraEffectFadeOut = core.GetManager<IAnimationMan>().Create(CAMERA_FADE_OUT, 10.0f);
            //var fo = cameraEffectFadeOut.AddTrack<float>(FrameInterpolation.Linear, OnFrameUpdate, 1.0f);
            //fo.AddFrame(0.0f, 10.0f);

            //var cameraEffectFadeIn = core.GetManager<IAnimationMan>().Create(CAMERA_FADE_IN, 10.0f);
            //var fi = cameraEffectFadeIn.AddTrack<float>(FrameInterpolation.Linear, OnFrameUpdate, 0.0f);
            //fi.AddFrame(1.0f, 10.0f);
        }

        private static void OnFrameUpdate(Entity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }
    }
}
