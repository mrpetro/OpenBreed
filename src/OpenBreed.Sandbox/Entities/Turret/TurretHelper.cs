using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Turret
{
    public static class TurretHelper
    {
        #region Public Fields

        public const string SPRITE_TURRET = "Atlases/Sprites/Turret";

        public static void CreateAnimations(ICore core)
        {
            var animationGuarding0 = core.Animations.Create<int>("Animations/Turret/Guarding/0", OnFrameUpdate);
            animationGuarding0.AddFrame(0, 2.0f);
            var animationGuarding22_5 = core.Animations.Create<int>("Animations/Guarding/Guard/22.5", OnFrameUpdate);
            animationGuarding22_5.AddFrame(1, 2.0f);
            var animationGuarding45 = core.Animations.Create<int>("Animations/Turret/Guarding/45", OnFrameUpdate);
            animationGuarding45.AddFrame(2, 2.0f);
            var animationGuarding67_5 = core.Animations.Create<int>("Animations/Turret/Guarding/67.5", OnFrameUpdate);
            animationGuarding67_5.AddFrame(3, 2.0f);
            var animationGuarding90 = core.Animations.Create<int>("Animations/Guarding/Guard/90", OnFrameUpdate);
            animationGuarding90.AddFrame(4, 2.0f);
            var animationGuarding112_5 = core.Animations.Create<int>("Animations/Turret/Guarding/112.5", OnFrameUpdate);
            animationGuarding112_5.AddFrame(5, 2.0f);
            var animationGuarding135 = core.Animations.Create<int>("Animations/Turret/Guarding/135", OnFrameUpdate);
            animationGuarding135.AddFrame(6, 2.0f);
            var animationGuarding157_5 = core.Animations.Create<int>("Animations/Turret/Guarding/157.5", OnFrameUpdate);
            animationGuarding157_5.AddFrame(7, 2.0f);
            var animationGuarding180 = core.Animations.Create<int>("Animations/Turret/Guarding/180", OnFrameUpdate);
            animationGuarding180.AddFrame(8, 2.0f);
            var animationGuarding202_5 = core.Animations.Create<int>("Animations/Turret/Guarding/202.5", OnFrameUpdate);
            animationGuarding202_5.AddFrame(9, 2.0f);
            var animationGuarding225 = core.Animations.Create<int>("Animations/Guarding/Guard/225", OnFrameUpdate);
            animationGuarding225.AddFrame(10, 2.0f);
            var animationGuarding247_5 = core.Animations.Create<int>("Animations/Turret/Guarding/247.5", OnFrameUpdate);
            animationGuarding247_5.AddFrame(11, 2.0f);
            var animationGuarding270 = core.Animations.Create<int>("Animations/Turret/Guarding/270", OnFrameUpdate);
            animationGuarding270.AddFrame(12, 2.0f);
            var animationGuarding292_5 = core.Animations.Create<int>("Animations/Guarding/Guard/292.5", OnFrameUpdate);
            animationGuarding292_5.AddFrame(13, 2.0f);
            var animationGuarding315 = core.Animations.Create<int>("Animations/Turret/Guarding/315", OnFrameUpdate);
            animationGuarding315.AddFrame(14, 2.0f);
            var animationGuarding337_5 = core.Animations.Create<int>("Animations/Turret/Guarding/337.5", OnFrameUpdate);
            animationGuarding337_5.AddFrame(15, 2.0f);
        }
        private static void OnStop()
        {
            //Console.WriteLine("Rotation -> Stopped");
        }

        public static void CreateRotationFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<RotationState, RotationImpulse>("Turret.Rotation");

            stateMachine.AddState(new Actor.States.Rotation.IdleState());
            stateMachine.AddState(new Actor.States.Rotation.RotatingState());

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

            stateMachine.AddOnEnterState(RotationState.Idle, RotationImpulse.Stop, OnStop);
        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            //entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public static Entity Create(ICore core, Vector2 pos)
        {
            var entity = core.Entities.CreateFromTemplate("Turret");
            entity.Get<PositionComponent>().Value = pos;

            //entity.Subscribe<CollisionEventArgs>(OnCollision);

            return entity;
        }


        #endregion Public Fields
    }
}