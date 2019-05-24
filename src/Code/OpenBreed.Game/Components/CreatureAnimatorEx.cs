//using OpenBreed.Core.Systems.Common.Components;
//using OpenBreed.Core.Entities;
//using OpenBreed.Core.Systems.Animation;
//using OpenBreed.Core.Systems.Animation.Components;
//using OpenBreed.Core.Systems.Rendering.Components;
//using System;
//using System.Linq;

//namespace OpenBreed.Game.Components
//{
//    public enum CreatureMovementState
//    {
//        Standing_Right,
//        Standing_Right_Right_Up,
//        Standing_Right_Up,
//        Standing_Right_Up_Left,
//        Standing_Right_Left,
//        Standing_Right_Left_Down,
//        Standing_Right_Down,
//        Standing_Right_Down_Right,
//        Walking_Right,
//        Walking_Right_Up,
//        Walking_Up,
//        Walking_Up_Left,
//        Walking_Left,
//        Walking_Left_Down,
//        Walking_Down,
//        Walking_Down_Right
//    }

//    public class CreatureAnimatorEx : SpriteAnimator
//    {
//        #region Private Fields

//        private Direction direction;
//        private Position position;
//        private ISprite sprite;

//        #endregion Private Fields

//        #region Public Properties

//        public Type SystemType { get { return typeof(AnimationSystem); } }

//        #endregion Public Properties

//        #region Public Methods

//        public void Animate(float dt)
//        {
//            var dir = this.direction.Current;
//            var pos = this.position.Current;

//            if (dir.X > 0 && dir.Y == 0)
//                Play("WALK_RIGHT");
//            else if (dir.X > 0 && dir.Y > 0)
//                Play("WALK_RIGHT_UP");
//            else if (dir.X == 0 && dir.Y > 0)
//                Play("WALK_UP");
//            else if (dir.X < 0 && dir.Y > 0)
//                Play("WALK_UP_LEFT");
//            else if (dir.X < 0 && dir.Y == 0)
//                Play("WALK_LEFT");
//            else if (dir.X < 0 && dir.Y < 0)
//                Play("WALK_LEFT_DOWN");
//            else if (dir.X == 0 && dir.Y < 0)
//                Play("WALK_DOWN");
//            else if (dir.X > 0 && dir.Y < 0)
//                Play("WALK_DOWN_RIGHT");

//            base.Animate(dt);
//        }

//        public void Deinitialize(IEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Initialize(IEntity entity)
//        {
//            direction = entity.Components.OfType<Direction>().First();
//            position = entity.Components.OfType<Position>().First();
//            sprite = entity.Components.OfType<ISprite>().First();
//        }

//        #endregion Public Methods
//    }
//}