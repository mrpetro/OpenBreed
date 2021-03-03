using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Button.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Entities.Button
{
    public class ButtonHelper
    {
        public static void AddButton(World world, int x, int y)
        {
            //var button = core.GetManager<IEntityMan>().CreateFromTemplate("Button");

            //button.Get<PositionComponent>().Value = new Vector2(0, 0);

            //world.Core.Commands.Post(new AddEntityCommand(world.Id, button.Id));
            //world.AddEntity(button);
        }

        public static void CreateFsm(ICore core)
        {
            var buttonFsm = core.GetManager<IFsmMan>().Create<ButtonState, ButtonImpulse>("Button");

            buttonFsm.AddState(new IdleState());
            buttonFsm.AddState(new PressedState());

            buttonFsm.AddTransition(ButtonState.Pressed, ButtonImpulse.Unpress, ButtonState.Idle);
            buttonFsm.AddTransition(ButtonState.Idle, ButtonImpulse.Press, ButtonState.Pressed);

            //buttonFsm.AddOnEnterState(ButtonState.Idle, ButtonImpulse.Unpress, OnButtonEnterIdleWithUnpress);
            //buttonFsm.AddOnEnterState(ButtonState.Pressed, ButtonImpulse.Press, OnButtonEnterPressedWithPress);
            //buttonFsm.AddOnLeaveState(ButtonState.Pressed, ButtonImpulse.Unpress, OnButtonLeavePressedWithUnpress);
            //buttonFsm.AddOnLeaveState(ButtonState.Idle, ButtonImpulse.Press, OnButtonLeaveIdleWithPress);
        }

        private static void OnButtonEnterIdleWithUnpress(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            entity.Core.Commands.Post(new SpriteOnCommand(entity.Id));
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, "ToDefine", 0));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        private static void OnButtonEnterPressedWithPress(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Core.Commands.Post(new SpriteOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, -1000000, 0, pos.Value));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));
        }


        private static void OnButtonLeavePressedWithUnpress(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
        }

        private static void OnButtonLeaveIdleWithPress(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        private static void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            var entity = sender as Entity;

            var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Button").Id;
            entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, fsmId, (int)ButtonImpulse.Press));
        }
    }
}
