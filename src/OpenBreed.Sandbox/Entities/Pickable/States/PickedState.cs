using OpenBreed.Fsm;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Pickable.States
{
    public class PickedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string STAMP_PREFIX = "Tiles/Stamps/Pickable/L4";
        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;
        #endregion Private Fields

        #region Public Constructors

        public PickedState(IFsmMan fsmMan, IStampMan stampMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Picked;

        /// <summary>
        /// TODO: Insecure, encapsulate this somehow
        /// </summary>
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{stateName}").Id;

            entity.PutStamp(stampId, 0, pos.Value);
            entity.SetText(0, $"{className} - Picked");

            Console.WriteLine($"Picked up '{className}'.");
            //TODO: Destroy item entity here
            //entity.Destroy();
        }

        public void LeaveState(Entity entity)
        {

        }

        #endregion Public Methods
    }
}