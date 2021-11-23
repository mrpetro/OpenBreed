using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common;
using OpenBreed.Fsm;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Pickable.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Pickable
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupPickableStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var collisionMan = managerCollection.GetManager<ICollisionMan>();
            var stampMan = managerCollection.GetManager<IStampMan>();
            var clipMan = managerCollection.GetManager<IClipMan>();
            var soundMan = managerCollection.GetManager<ISoundMan>();

            var fsm = managerCollection.GetManager<IFsmMan>().Create<FunctioningState, FunctioningImpulse>("Pickable.Functioning");

            fsm.AddState(new LyingState(fsmMan, collisionMan, stampMan));
            fsm.AddState(new PickedState(fsmMan, stampMan, soundMan));

            fsm.AddTransition(FunctioningState.Lying, FunctioningImpulse.Pick, FunctioningState.Picked);
        }
    }
}
