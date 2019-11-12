using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
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
        public static IEntity AddCamera(ICore core, World world, float x, float y)
        {
            var cameraGuy = core.Entities.Create();
            cameraGuy.Add(Position.Create(x, y));
            cameraGuy.Add(CameraComponent.Create(1.0f));
            world.AddEntity(cameraGuy);
            return cameraGuy;
        }
    }
}
