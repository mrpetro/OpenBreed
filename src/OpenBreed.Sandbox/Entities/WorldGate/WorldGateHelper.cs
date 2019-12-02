using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Messages;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Jobs;
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

        public static IEntity AddWorldExit(World world, int x, int y, string worldName, int entryId)
        {
            var core = world.Core;
            var teleportEntry = core.Entities.Create();

            teleportEntry.Add(Body.Create(1.0f, 1.0f, "Trigger", (e, c) => OnCollision(teleportEntry, e, c)));
            teleportEntry.Add(Position.Create(x * 16, y * 16));
            teleportEntry.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntry.Add(TextHelper.Create(core, new Vector2(0, 32), "WorldExit"));
            teleportEntry.Tag = new Tuple<string, int>(worldName, entryId);

            world.AddEntity(teleportEntry);

            return teleportEntry;
        }

        public static IEntity AddWorldEntry(World world, int x, int y, int entryEntityId)
        {
            var core = world.Core;
            var teleportEntity = core.Entities.Create();

            teleportEntity.Add(Position.Create(x * 16, y * 16));
            teleportEntity.Add(AxisAlignedBoxShape.Create(16, 16, 8, 8));
            teleportEntity.Add(TextHelper.Create(core, new Vector2(0, 32), "WorldEntry"));

            world.AddEntity(teleportEntity);

            return teleportEntity;
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCollision(IEntity exitEntity, IEntity targetEntity, Vector2 projection)
        {
            var cameraEntity = targetEntity.Tag as IEntity;

            if (cameraEntity == null)
                return;

            var exitInfo = (Tuple<string, int>)exitEntity.Tag;

            var jobChain = new JobChain();

            jobChain.Equeue(new WorldJob(cameraEntity.World, "Pause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_OUT));

            jobChain.Equeue(new EntityJob(cameraEntity, "LeaveWorld"));
            jobChain.Equeue(new EntityJob(targetEntity, "LeaveWorld"));

            jobChain.Equeue(new EntityJob(cameraEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));
            jobChain.Equeue(new EntityJob(targetEntity, "EnterWorld", exitInfo.Item1, exitInfo.Item2));

            //jobChain.Equeue(new TeleportJob(targetEntity, exitPos.Value + offset, true));
            jobChain.Equeue(new WorldJob(cameraEntity.World, "Unpause"));
            jobChain.Equeue(new CameraEffectJob(cameraEntity, CameraHelper.CAMERA_FADE_IN));

            exitEntity.Core.Jobs.Execute(jobChain);
        }



        #endregion Private Methods
    }
}
