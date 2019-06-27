using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Animation.Messages;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation
{
    public class SpriteAnimSystem : WorldSystem, IAnimationSystem
    {

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<Animator<int>> animatorComps = new List<Animator<int>>();
        private readonly List<ISprite> spriteComps = new List<ISprite>();

        #endregion Private Fields

        #region Public Constructors

        public SpriteAnimSystem(ICore core) : base(core)
        {
            Require<ISprite>();
            Require<Animator<int>>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                AnimateEntity(dt, i);
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            animatorComps.Add(entity.Components.OfType<Animator<int>>().First());
            spriteComps.Add(entity.Components.OfType<ISprite>().First());
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        public void Play(Animator<int> animator, IAnimationData<int> data = null, float startPosition = 0.0f)
        {
            animator.Data = data;

            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Animator<int> animator)
        {
            animator.Paused = true;
        }

        public void Stop(Animator<int> animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
        }

        public void Animate(Animator<int> animator, float dt)
        {
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

            animator.Frame = animator.Data.GetFrame(animator.Position);
        }


        public override bool HandleMsg(IEntity sender, IEntityMsg message)
        {
            switch (message.Type)
            {
                case PlayAnimMsg.TYPE:
                    return HandlePlayAnimMsg(sender, (PlayAnimMsg)message);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandlePlayAnimMsg(IEntity sender, PlayAnimMsg message)
        {
            var index = entities.IndexOf(sender);
            if (index < 0)
                return false;

            Play(animatorComps[index], (IAnimationData<int>)Core.Animations.GetByName(message.Id));

            return true;
        }

        private void AnimateEntity(float dt, int index)
        {
            Animate(animatorComps[index], dt);
            spriteComps[index].ImageId = animatorComps[index].Frame;
        }

        #endregion Private Methods

    }
}