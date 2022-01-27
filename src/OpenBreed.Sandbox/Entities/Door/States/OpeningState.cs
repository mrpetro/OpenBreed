using System;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Audio.Interface.Managers;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string SOUND_PREFIX = "Vanilla/Common";
        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;
        private readonly IClipMan<Entity> clipMan;
        private readonly ISoundMan soundMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState(IFsmMan fsmMan, IStampMan stampMan, IClipMan<Entity> clipMan, ISoundMan soundMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
            this.clipMan = clipMan;
            this.soundMan = soundMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)FunctioningState.Opening;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var metadata = entity.Get<MetadataComponent>();
            var level = metadata.Level;
            var className = metadata.Name;
            var flavor = metadata.Flavor;

            var stateName = fsmMan.GetStateName(FsmId, Id);
            var clipId = clipMan.GetByName($"{level}/{className}/{stateName}/{flavor}").Id;
            var stampId = stampMan.GetByName($"{level}/{className}/{flavor}/Opened").Id;
            var soundId = soundMan.GetByName($"{SOUND_PREFIX}/{className}/{stateName}");

            entity.SetSpriteOn();
            entity.PlayAnimation(0, clipId);
            entity.PutStamp(stampId, 0, pos.Value);
            //entity.SetText(0, "Door - Opening");
            entity.EmitSound(soundId);

            triggerMan.OnEntityAnimFinished(entity, OnAnimStopped, singleTime: true);
        }

        public void LeaveState(Entity entity)
        {
            entity.SetSpriteOff();

            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.FullObstacle);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(Entity entity, AnimFinishedEventArgs e)
        {
            entity.SetState(FsmId, (int)FunctioningImpulse.StopOpening);
        }

        #endregion Private Methods
    }
}