using OpenBreed.Fsm;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Pickable.States
{
    public class PickedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string STAMP_PREFIX = "L4";
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
            var metadata = entity.Get<ClassComponent>();
            var className = metadata.Name;
            var flavor = metadata.Flavor;
            var stateName = fsmMan.GetStateName(FsmId, Id);

            if (flavor != "Trigger")
            {
                int stampId;

                if (flavor is null)
                    stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{stateName}").Id;
                else
                    stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{flavor}/{stateName}").Id;

                entity.PutStamp(stampId, 0, pos.Value);
                entity.EmitSound(PickableHelper.SOUND_PICK_KEYS);
            }

            entity.SetText(0, $"{className} - Picked");

            Console.WriteLine($"Picked up '{className}'.");

            //entity.LeaveWorld();
            //entity.Destroy();
        }

        public void LeaveState(Entity entity)
        {

        }

        #endregion Public Methods
    }
}