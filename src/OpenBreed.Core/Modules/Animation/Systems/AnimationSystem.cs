﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems
{
    public class AnimationSystem : WorldSystem, ICommandExecutor, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<int> entities = new List<int>();

        //private readonly List<AnimationComponent> animationComps = new List<AnimationComponent>();
        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public AnimationSystem(AnimationSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);
            Require<AnimationComponent>();
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
                    Animate(entity, dt);
            }
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = Core.Entities.GetById(entities[i]);
                Debug.Assert(entity != null);

                Animate(entity, dt);
            }
        }

        public void Set(Entity entity, Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Play(Entity entity, Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            if (animId != -1)
                animator.AnimId = animId;

            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Entity entity, Animator animator)
        {
            animator.Paused = true;
        }

        public void Stop(Entity entity, Animator animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
            RaiseAnimStoppedEvent(entity, animator);
        }

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case SetAnimCommand.TYPE:
                    return HandleSetAnimCommand((SetAnimCommand)cmd);

                case PlayAnimCommand.TYPE:
                    return HandlePlayAnimCommand((PlayAnimCommand)cmd);

                case PauseAnimCommand.TYPE:
                    return HandlePauseAnimCommand((PauseAnimCommand)cmd);

                case StopAnimCommand.TYPE:
                    return HandleStopAnimCommand((StopAnimCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity.Id);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Animate(Entity entity, float dt)
        {
            var ac = entity.Get<AnimationComponent>();

            //Update all animators with delta time
            for (int i = 0; i < ac.Items.Count; i++)
                UpdateAnimator(entity, ac.Items[i], dt);
        }

        private void UpdateAnimator(Entity entity, Animator animator, float dt)
        {
            if (animator.Paused)
                return;

            if (animator.AnimId < 0)
                return;

            var data = Core.Animations.GetById(animator.AnimId);

            animator.Position += animator.Speed * dt;

            if (animator.Position > data.Length)
            {
                if (animator.Loop)
                    animator.Position = animator.Position - data.Length;
                else
                {
                    Stop(entity, animator);
                    return;
                }
            }

            object nextFrame;

            if (data.UpdateWithNextFrame(entity, animator, out nextFrame))
            {
                animator.Frame = nextFrame;
                RaiseAnimChangedEvent(entity, animator);
            }
        }

        private void RaiseAnimStoppedEvent(Entity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimStoppedEventArgs(animator));
        }

        private void RaiseAnimChangedEvent(Entity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimChangedEventArgs(animator.Frame));
        }

        private bool HandlePauseAnimCommand(PauseAnimCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            Pause(entity, animator);

            return true;
        }

        private bool HandleStopAnimCommand(StopAnimCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            Stop(entity, animator);

            return true;
        }

        private bool HandleSetAnimCommand(SetAnimCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();

            var animData = Core.Animations.GetByName(cmd.Id);

            if (animData == null)
                Core.Logging.Warning($"Animation with ID '{cmd.Id}' not found.");

            Play(entity, ac.Items[cmd.AnimatorId], animData.Id);

            return true;
        }

        private bool HandlePlayAnimCommand(PlayAnimCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            int animId = -1;

            if (cmd.Id != null)
            {
                var data = Core.Animations.GetByName(cmd.Id);

                if (data == null)
                    Core.Logging.Warning($"Animation with ID = '{cmd.Id}' not found.");
                else
                    animId = data.Id;
            }
            else
            {
                animId = animator.AnimId;
            }

            Play(entity, animator, animId);

            return true;
        }

        #endregion Private Methods
    }
}