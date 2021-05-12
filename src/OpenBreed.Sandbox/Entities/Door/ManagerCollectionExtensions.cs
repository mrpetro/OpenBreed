using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Components.States;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Door
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupDoorStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();
            var collisionMan = managerCollection.GetManager<ICollisionMan>();
            var stampMan = managerCollection.GetManager<IStampMan>();


            var fsm = managerCollection.GetManager<IFsmMan>().Create<FunctioningState, FunctioningImpulse>("Door.Functioning");

            fsm.AddState(new OpeningState(fsmMan, commandsMan));
            fsm.AddState(new OpenedAwaitClose(fsmMan, commandsMan, stampMan));
            fsm.AddState(new ClosingState(fsmMan, commandsMan));
            fsm.AddState(new ClosedState(fsmMan, commandsMan, collisionMan, stampMan));

            fsm.AddTransition(FunctioningState.Closed, FunctioningImpulse.Open, FunctioningState.Opening);
            fsm.AddTransition(FunctioningState.Opening, FunctioningImpulse.StopOpening, FunctioningState.Opened);
            fsm.AddTransition(FunctioningState.Opened, FunctioningImpulse.Close, FunctioningState.Closing);
            fsm.AddTransition(FunctioningState.Closing, FunctioningImpulse.StopClosing, FunctioningState.Closed);
        }
    }
}
