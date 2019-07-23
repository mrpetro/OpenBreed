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

            World.MessageBus.RegisterHandler(PlayAnimMsg.TYPE, this);
            World.MessageBus.RegisterHandler(PauseAnimMsg.TYPE, this);
            World.MessageBus.RegisterHandler(StopAnimMsg.TYPE, this);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                Animate(i, dt);
        }

        public void Play(Animator<T> animator, IAnimationData<T> data = null, float startPosition = 0.0f)
        {
            animator.Data = data;

            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Animator<T> animator)
        {
            animator.Paused = true;
        }

        public void Stop(Animator<T> animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
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
                    Stop(animator);
                    return;
                }
            }

            var newFrame = animator.Data.GetFrame(animator.Position, animator.Transition);

            if (!animator.Frame.Equals(newFrame))
            {
                animator.Frame = newFrame;

                World.RaiseSystemEvent(this, new FrameChangedEvent<T>(animator.Frame));

                entities[index].HandleSystemEvent?.Invoke(this, new FrameChangedEvent<T>(animator.Frame));
            }
        }

        public override bool HandleMsg(object sender, IMsg message)
        {
            switch (message.Type)
            {
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

            Stop(animatorComps[index]);

            return true;
        }

        private bool HandlePlayAnimMsg(object sender, PlayAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            Play(animatorComps[index], (IAnimationData<T>)Core.Animations.Anims.GetByName(message.Id));

            return true;
        }

        #endregion Private Methods
    }
}