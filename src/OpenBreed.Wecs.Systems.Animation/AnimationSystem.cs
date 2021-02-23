﻿using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Animation
{
    public class AnimationSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<int> entities = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly IAnimMan animMan;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationSystem(IEntityMan entityMan, IAnimMan animMan)
        {
            this.entityMan = entityMan;
            this.animMan = animMan;

            Require<AnimationComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<SetAnimCommand>(HandleSetAnimCommand);
            commands.Register<PlayAnimCommand>(HandlePlayAnimCommand);
            commands.Register<PauseAnimCommand>(HandlePauseAnimCommand);
            commands.Register<StopAnimCommand>(HandleStopAnimCommand);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entityMan.GetById(entities[i]);
                if (entity.Components.OfType<PauseImmuneComponent>().Any())
                    Animate(entity, dt);
            }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entityMan.GetById(entities[i]);
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

        private static bool HandlePauseAnimCommand(ICore core, PauseAnimCommand cmd)
        {
            var system = core.GetManager<ISystemFinder>().GetSystemByEntityId<AnimationSystem>(cmd.EntityId);
            if (system == null)
                return false;

            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            system.Pause(entity, animator);

            return true;
        }

        private static bool HandleStopAnimCommand(ICore core, StopAnimCommand cmd)
        {
            var system = core.GetManager<ISystemFinder>().GetSystemByEntityId<AnimationSystem>(cmd.EntityId);
            if (system == null)
                return false;

            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            system.Stop(entity, animator);

            return true;
        }

        private static bool HandleSetAnimCommand(ICore core, SetAnimCommand cmd)
        {
            var system = core.GetManager<ISystemFinder>().GetSystemByEntityId<AnimationSystem>(cmd.EntityId);
            if (system == null)
                return false;

            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();

            var animData = core.GetManager<IAnimMan>().GetByName(cmd.Id);

            if (animData == null)
                core.GetManager<ILogger>().Warning($"Animation with ID '{cmd.Id}' not found.");

            system.Play(entity, ac.Items[cmd.AnimatorId], animData.Id);

            return true;
        }

        private static bool HandlePlayAnimCommand(ICore core, PlayAnimCommand cmd)
        {
            var system = core.GetManager<ISystemFinder>().GetSystemByEntityId<AnimationSystem>(cmd.EntityId);
            if (system == null)
                return false;

            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.Items[cmd.AnimatorId];

            int animId = -1;

            if (cmd.Id != null)
            {
                var data = core.GetManager<IAnimMan>().GetByName(cmd.Id);

                if (data == null)
                    core.Logging.Warning($"Animation with ID = '{cmd.Id}' not found.");
                else
                    animId = data.Id;
            }
            else
            {
                animId = animator.AnimId;
            }

            system.Play(entity, animator, animId);

            return true;
        }

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

            var data = animMan.GetById(animator.AnimId);

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

            data.UpdateWithNextFrame(entity, animator);
        }

        private void RaiseAnimStoppedEvent(Entity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimStoppedEventArgs(animator));
        }

        #endregion Private Methods
    }
}