using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems.Animation.Components;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation
{
    public class AnimationSystem : WorldSystemEx, IUpdatableSystemEx
    {
        #region Private Fields

        private List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public AnimationSystem(ICore core) : base(core)
        {
            Require<ISprite>();
            Require<Animation<int>>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                AnimateEntity(dt, entities[i]);
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods

        #region Private Methods

        private void AnimateEntity(float dt, IEntity entity)
        {
            var sprite = entity.Components.OfType<ISprite>().First();
            var animation = entity.Components.OfType<Animation<int>>().First();

            animation.Animate(dt);
            sprite.ImageId = animation.Frame;
        }

        #endregion Private Methods
    }
}