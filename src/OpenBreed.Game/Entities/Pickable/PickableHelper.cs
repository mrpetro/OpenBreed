using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Pickable.States;
using OpenBreed.Game.Helpers;
using OpenTK;

namespace OpenBreed.Game.Entities.Pickable
{
    public class PickableHelper
    {
        #region Public Methods

        public static StateMachine CreateFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            var idleStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Items/Ammo/Idle");
            var pickedStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Items/Ammo/Picked");

            stateMachine.AddState(new IdleState("Idle", idleStamp.Id));
            stateMachine.AddState(new PickingState("Picking", pickedStamp.Id));

            return stateMachine;
        }

        public static void AddItem(ICore core, World world, int x, int y)
        {
            var item = core.Entities.Create();

            //item.Add(new Animator(5.0f, false));
            item.Add(Body.Create(1.0f, 1.0f, "Static", (e, c) => OnCollision(item, e, c)));
            item.Add(Position.Create(x * 16, y * 16));
            item.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            item.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = PickableHelper.CreateFSM(item);
            doorSm.SetInitialState("Idle");
            world.AddEntity(item);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));
        }

        #endregion Private Methods
    }
}