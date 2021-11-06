using OpenBreed.Wecs.Systems.Rendering.Commands;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;
        private readonly string stampPrefix;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState(IFsmMan fsmMan, ICommandsMan commandsMan, IStampMan stampMan)
        {
            this.animPrefix = "Animations";
            this.stampPrefix = "Tiles/Stamps";
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)FunctioningState.Opening;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.SetSpriteOn();

            var pos = entity.Get<PositionComponent>();

            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            commandsMan.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}.{className}.{stateName}", 0));
            var stampId = stampMan.GetByName($"{stampPrefix}/{className}/Opened").Id;

            entity.PutStamp(stampId, 0, pos.Value);
            entity.SetText(0, "Door - Opening");

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.SetSpriteOff();

            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);

            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.StaticObstacle);
        }

        #endregion Public Methods

        #region Private Methods

        //private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        //{
        //    var entity = sender as Entity;

        //    entity.PostCommand(new SpriteSetCommand(entity.Id, (int)e.Frame));
        //}

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            var entity = sender as Entity;
            entity.SetState(FsmId, (int)FunctioningImpulse.StopOpening);
        }

        #endregion Private Methods
    }
}