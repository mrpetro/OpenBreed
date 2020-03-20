using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Button.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Button
{
    public class ButtonHelper
    {
        public static void AddButton(World world, int x, int y)
        {
            var core = world.Core;

            var door = core.Entities.CreateFromTemplate("Button");

            door.GetComponent<PositionComponent>().Value = new Vector2(0, 0);

            var doorSm = CreateFSM(door);
            doorSm.SetInitialState(IdleState.NAME);

            world.AddEntity(door);
        }

        public static StateMachine CreateFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("CursorHandling");

            stateMachine.AddState(new IdleState());
            stateMachine.AddState(new PressedState());


            return stateMachine;
        }
    }
}
