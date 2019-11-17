using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.WorldGate
{
    public class WorldGateHelper
    {
        #region Public Methods

        public static IEntity AddWorldExit(ICore core, World world, int x, int y, int exitEntityId)
        {
            var teleportEntry = core.Entities.Create();

            teleportEntry.Add(Body.Create(1.0f, 1.0f, "Trigger", (e, c) => OnCollision(teleportEntry, e, c)));
            teleportEntry.Add(Position.Create(x * 16, y * 16));
            teleportEntry.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntry.Add(TextHelper.Create(core, new Vector2(-10, 10), "WorldExit"));
            teleportEntry.Tag = exitEntityId;

            world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddWorldEntry(ICore core, World world, int x, int y)
        {
            var teleportEntity = core.Entities.Create();

            teleportEntity.Add(Position.Create(x * 16, y * 16));
            teleportEntity.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntity.Add(TextHelper.Create(core, new Vector2(-10, 10), "WorldEntry"));

            world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            var cameraEntity = otherEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var exitEntityId = (int)thisEntity.Tag;
            var existEntity = thisEntity.Core.Entities.GetById(exitEntityId);
            var exitPos = existEntity.Components.OfType<Position>().First();
            var thisAabb = thisEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var otherAabb = otherEntity.Components.OfType<IShapeComponent>().First().Aabb;
            var offset = new Vector2((32 - otherAabb.Width) / 2.0f, (32 - otherAabb.Height) / 2.0f);

            //Vanilla game
            //1. Pause game
            //2. Camera fade-out
            //3. Teleport character
            //4. Unpause game
            //5. Camera fade-in

            var jobChain = new JobChain();
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));
            jobChain.Equeue(new TeleportJob(otherEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Unpause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));

            thisEntity.Core.Jobs.Execute(jobChain);
        }



        #endregion Private Methods
    }
}
