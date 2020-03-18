using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Common.Components;

namespace OpenBreed.Core.Modules.Animation.Systems
{
    public class AnimationSystem : WorldSystem, IAnimationSystem, ICommandExecutor
    {
        #region Private Fields

        private readonly List<int> entities = new List<int>();
        private readonly List<AnimatorComponent> animatorComps = new List<AnimatorComponent>();
        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public AnimationSystem(AnimationSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);
            Require<AnimatorComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(SetAnimCommand.TYPE, cmdHandler);
            World.RegisterHandler(PlayAnimCommand.TYPE, cmdHandler);
            World.RegisterHandler(PauseAnimCommand.TYPE, cmdHandler);
            World.RegisterHandler(StopAnimCommand.TYPE, cmdHandler);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = Core.Entities.GetById(entities[i]);
                if (entity.Components.OfType<PauseImmuneComponent>().Any())
                    Animate(i, dt);
            }
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
                Animate(i, dt);
        }

        public void Set(AnimatorComponent animator, int animId = -1, float startPosition = 0.0f)
        {
            //animator.Data = data;

            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Play(AnimatorComponent animator, int animId = -1, float startPosition = 0.0f)
        {
            if (animId != -1)
                animator.AnimId = animId;

            //if (data != null)
            //    animator.Data = data;

            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(AnimatorComponent animator)
        {
            animator.Paused = true;
        }

        public void Stop(IEntity entity, AnimatorComponent animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
            RaiseAnimStoppedEvent(entity, animator);
        }

        public void Animate(int index, float dt)
        {
            var animator = animatorComps[index];

            if (animator.Paused)
                return;

            if (animator.AnimId < 0)
                return;

            var data = Core.Animations.Anims.GetById(animator.AnimId);

            animator.Position += animator.Speed * dt;

            if (animator.Position > data.Length)
            {
                if (animator.Loop)
                    animator.Position = animator.Position - data.Length;
                else
                {
                    var entity = Core.Entities.GetById(entities[index]);
                    Stop(entity, animator);
                    return;
                }
            }

            object nextFrame;

            if (data.TryGetNextFrame(animator.Position, animator.Frame, out nextFrame, animator.Transition))
            {
                animator.Frame = nextFrame;

                var entity = Core.Entities.GetById(entities[index]);
                RaiseAnimChangedEvent(entity, animator);
            }
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case SetAnimCommand.TYPE:
                    return HandleSetAnimCommand(sender, (SetAnimCommand)cmd);

                case PlayAnimCommand.TYPE:
                    return HandlePlayAnimCommand(sender, (PlayAnimCommand)cmd);

                case PauseAnimCommand.TYPE:
                    return HandlePauseAnimCommand(sender, (PauseAnimCommand)cmd);

                case StopAnimCommand.TYPE:
                    return HandleStopAnimCommand(sender, (StopAnimCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity.Id);
            animatorComps.Add(entity.GetComponent<AnimatorComponent>());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            animatorComps.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseAnimStoppedEvent(IEntity entity, AnimatorComponent animator)
        {
            entity.RaiseEvent(AnimationEventTypes.ANIMATION_STOPPED, new AnimStoppedEventArgs(animator));
        }

        private void RaiseAnimChangedEvent(IEntity entity, AnimatorComponent animator)
        {
            entity.RaiseEvent(AnimationEventTypes.ANIMATION_CHANGED, new AnimChangedEventArgs(animator.Frame));
        }

        private bool HandlePauseAnimCommand(object sender, PauseAnimCommand cmd)
        {
            var index = entities.IndexOf(cmd.EntityId);
            if (index < 0)
                return false;

            Pause(animatorComps[index]);

            return true;
        }

        private bool HandleStopAnimCommand(object sender, StopAnimCommand cmd)
        {
            var index = entities.IndexOf(cmd.EntityId);
            if (index < 0)
                return false;

            var entity = Core.Entities.GetById(cmd.EntityId);
            Stop(entity, animatorComps[index]);

            return true;
        }

        private bool HandleSetAnimCommand(object sender, SetAnimCommand cmd)
        {
            var index = entities.IndexOf(cmd.EntityId);
            if (index < 0)
                return false;

            var animData = Core.Animations.Anims.GetByName(cmd.Id);

            if (animData == null)
                Core.Logging.Warning($"Animation with ID = '{cmd.Id}' not found.");

            Play(animatorComps[index], animData.Id);

            return true;
        }

        private bool HandlePlayAnimCommand(object sender, PlayAnimCommand cmd)
        {
            var index = entities.IndexOf(cmd.EntityId);
            if (index < 0)
                return false;

            var animator = animatorComps[index];

            int animId = -1;

            if (cmd.Id != null)
            {
                var data = Core.Animations.Anims.GetByName(cmd.Id);

                if (data == null)
                    Core.Logging.Warning($"Animation with ID = '{cmd.Id}' not found.");
                else
                    animId = data.Id;
            }
            else
                animId = animator.AnimId;

            Play(animator, animId);

            return true;
        }

        #endregion Private Methods
    }
}