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

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState()
        {
            animPrefix = "Animations";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)FunctioningState.Opening;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOnCommand(entity.Id));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(FsmId, Id);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}/{className}/{stateName}", 0));

            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);

            var colCmp = entity.Get<CollisionComponent>();
            colCmp.ColliderTypes.Remove(ColliderTypes.StaticObstacle);
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
            entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.StopOpening));
        }

        #endregion Private Methods
    }
}