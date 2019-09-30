using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems
{
    public class AnimSystem<T> : WorldSystem, IAnimationSystem, IMsgHandler
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<Animator<T>> animatorComps = new List<Animator<T>>();

        #endregion Private Fields

        #region Public Constructors

        public AnimSystem(ICore core) : base(core)
        {
            Require<Animator<int>>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(SetAnimMsg.TYPE, this);
            World.MessageBus.RegisterHandler(PlayAnimMsg.TYPE, this);
            World.MessageBus.RegisterHandler(PauseAnimMsg.TYPE, this);
            World.MessageBus.RegisterHandler(StopAnimMsg.TYPE, this);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                Animate(i, dt);
        }

        public void Set(Animator<T> animator, IAnimationData<T> data = null, float startPosition = 0.0f)
        {
            animator.Data = data;

            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Play(Animator<T> animator, IAnimationData<T> data = null, float startPosition = 0.0f)
        {
            if(data != null)
                animator.Data = data;

            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Animator<T> animator)
        {
            animator.Paused = true;
        }

        public void Stop(IEntity entity, Animator<T> animator)
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

            if (animator.Data == null)
                return;

            animator.Position += animator.Speed * dt;

            if (animator.Position > animator.Data.Length)
            {
                if (animator.Loop)
                    animator.Position = animator.Position - animator.Data.Length;
                else
                {
                    Stop(entities[index], animator);
                    return;
                }
            }

            var newFrame = animator.Data.GetFrame(animator.Position, animator.Transition);

            if (!animator.Frame.Equals(newFrame))
            {
                animator.Frame = newFrame;
                RaiseAnimChangedEvent(entities[index], animator);
            }
        }

        public override bool HandleMsg(object sender, IMsg message)
        {
            switch (message.Type)
            {
                case SetAnimMsg.TYPE:
                    return HandleSetAnimMsg(sender, (SetAnimMsg)message);

                case PlayAnimMsg.TYPE:
                    return HandlePlayAnimMsg(sender, (PlayAnimMsg)message);

                case PauseAnimMsg.TYPE:
                    return HandlePauseAnimMsg(sender, (PauseAnimMsg)message);

                case StopAnimMsg.TYPE:
                    return HandleStopAnimMsg(sender, (StopAnimMsg)message);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
            animatorComps.Add(entity.Components.OfType<Animator<T>>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            animatorComps.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseAnimStoppedEvent(IEntity entity, Animator<T> animator)
        {
            entity.RaiseEvent(new AnimStoppedEvent<T>(animator));
        }

        private void RaiseAnimChangedEvent(IEntity entity, Animator<T> animator)
        {
            entity.RaiseEvent(new AnimChangedEvent<T>(animator.Frame));
        }

        private bool HandlePauseAnimMsg(object sender, PauseAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            Pause(animatorComps[index]);

            return true;
        }

        private bool HandleStopAnimMsg(object sender, StopAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            Stop(message.Entity, animatorComps[index]);

            return true;
        }

        private bool HandleSetAnimMsg(object sender, SetAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            var animData = (IAnimationData<T>)Core.Animations.Anims.GetByName(message.Name);

            if (animData == null)
                Core.Logging.Warning($"Animation with ID = '{message.Name}' not found.");

            Play(animatorComps[index], animData);

            return true;
        }

        private bool HandlePlayAnimMsg(object sender, PlayAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            var animator = animatorComps[index];

            var animData = message.Name != null ? (IAnimationData<T>)Core.Animations.Anims.GetByName(message.Name) : animator.Data;

            if (animData == null)
                Core.Logging.Warning($"Animation with ID = '{message.Name}' not found.");

            Play(animator, animData);

            return true;
        }

        #endregion Private Methods
    }
}