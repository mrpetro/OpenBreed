using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Rendering.Components;
using System;
using System.Linq;

namespace OpenBreed.Game.Animation.Components
{
    public class CreatureAnimator : IAnimationComponent
    {
        #region Private Fields

        private Direction direction;
        private Sprite sprite;

        #endregion Private Fields

        #region Public Properties

        public Type SystemType { get { return typeof(AnimationSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Animate(float dt)
        {
            if (direction.Data.X > 0 && direction.Data.Y == 0)
                sprite.ImageId = 0;
            else if (direction.Data.X > 0 && direction.Data.Y > 0)
                sprite.ImageId = 7;
            else if (direction.Data.X == 0 && direction.Data.Y > 0)
                sprite.ImageId = 6;
            else if (direction.Data.X < 0 && direction.Data.Y > 0)
                sprite.ImageId = 5;
            else if (direction.Data.X < 0 && direction.Data.Y == 0)
                sprite.ImageId = 4;
            else if (direction.Data.X < 0 && direction.Data.Y < 0)
                sprite.ImageId = 3;
            else if (direction.Data.X == 0 && direction.Data.Y < 0)
                sprite.ImageId = 2;
            else if (direction.Data.X > 0 && direction.Data.Y < 0)
                sprite.ImageId = 1;
        }

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            direction = entity.Components.OfType<Direction>().First();
            sprite = entity.Components.OfType<Sprite>().First();
        }

        #endregion Public Methods
    }
}