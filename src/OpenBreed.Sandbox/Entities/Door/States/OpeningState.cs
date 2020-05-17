using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Door.States;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Animations";

        #endregion Private Fields

        #region Public Constructors

        public OpeningState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)FunctioningState.Opening;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOnCommand(entity.Id));

            var className = entity.GetComponent<ClassComponent>().Name;
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            entity.PostCommand(new PlayAnimCommand(entity.Id, $"{ANIM_PREFIX}/{className}/{stateName}", 0));

            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        //private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        //{
        //    var entity = sender as IEntity;

        //    entity.PostCommand(new SpriteSetCommand(entity.Id, (int)e.Frame));
        //}

        #region Private Methods

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            var entity = sender as IEntity;
            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.StopOpening));
        }

        #endregion Private Methods
    }
}