using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems
{
    public class AnimSystem<T> : WorldSystem, IAnimationSystem, IMsgListener
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<Animator> animatorComps = new List<Animator>();
        private readonly MsgHandler msgHandler;

        #endregion Private Fields

        #region Public Constructors

        public AnimSystem(ICore core) : base(core)
        {
            msgHandler = new MsgHandler(this);
            Require<Animator>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {

            base.Initialize(world);

            World.MessageBus.RegisterHandler(SetAnimMsg.TYPE, msgHandler);
            World.MessageBus.RegisterHandler(PlayAnimMsg.TYPE, msgHandler);
            World.MessageBus.RegisterHandler(PauseAnimMsg.TYPE, msgHandler);
            World.MessageBus.RegisterHandler(StopAnimMsg.TYPE, msgHandler);
        }

        public void Update(float dt)
        {
            msgHandler.PostEnqueued();

            for (int i = 0; i < entities.Count; i++)
                Animate(i, dt);
        }

        public void Set(Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            //animator.Data = data;

            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Play(Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            if (animId != -1)
                animator.AnimId = animId;

            //if (data != null)
            //    animator.Data = data;

            animator.AnimId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Animator animator)
        {
            animator.Paused = true;
        }

        public void Stop(IEntity entity, Animator animator)
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
                    Stop(entities[index], animator);
                    return;
                }
            }

            object nextFrame;

            if (data.TryGetNextFrame(animator.Position, animator.Frame, out nextFrame, animator.Transition))
            {
                animator.Frame = nextFrame;
                RaiseAnimChangedEvent(entities[index], animator);
            }
        }

        public override bool RecieveMsg(object sender, IMsg message)
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
            animatorComps.Add(entity.Components.OfType<Animator>().First());
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

        private void RaiseAnimStoppedEvent(IEntity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimStoppedEvent(animator));
        }

        private void RaiseAnimChangedEvent(IEntity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimChangedEvent(animator.Frame));
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

            var animData = Core.Animations.Anims.GetByName(message.Name);

            if (animData == null)
                Core.Logging.Warning($"Animation with ID = '{message.Name}' not found.");

            Play(animatorComps[index], animData.Id);

            return true;
        }

        private bool HandlePlayAnimMsg(object sender, PlayAnimMsg message)
        {
            var index = entities.IndexOf(message.Entity);
            if (index < 0)
                return false;

            var animator = animatorComps[index];

            int animId = -1;

            if (message.Name != null)
            {
                var data = Core.Animations.Anims.GetByName(message.Name);

                if (data == null)
                    Core.Logging.Warning($"Animation with ID = '{message.Name}' not found.");
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