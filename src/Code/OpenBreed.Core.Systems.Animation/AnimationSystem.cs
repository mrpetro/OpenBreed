using OpenBreed.Core.Systems.Animation.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Animation
{
    public class AnimationSystem : WorldSystem<IAnimationComponent>, IAnimationSystem
    {
        #region Private Fields

        private List<IAnimationComponent> animators;

        #endregion Private Fields

        #region Public Constructors

        public AnimationSystem()
        {
            animators = new List<IAnimationComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(float dt)
        {
            for (int i = 0; i < animators.Count; i++)
                animators[i].Animate(dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void AddComponent(IAnimationComponent component)
        {
            animators.Add(component);
        }

        protected override void RemoveComponent(IAnimationComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}