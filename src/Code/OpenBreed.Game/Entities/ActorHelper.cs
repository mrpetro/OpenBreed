using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Game.Components;
using OpenBreed.Game.Components.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public static class ActorHelper
    {

        public static SpriteAnimator CreateAnimator()
        {
            var animationStandingRight = new Animation<int>();
            animationStandingRight.AddFrame(0, 2.0f);
            var animationStandingRightDown = new Animation<int>();
            animationStandingRightDown.AddFrame(1, 2.0f);
            var animationStandingDown = new Animation<int>();
            animationStandingDown.AddFrame(2, 2.0f);
            var animationStandingDownLeft = new Animation<int>();
            animationStandingDownLeft.AddFrame(3, 2.0f);
            var animationStandingLeft = new Animation<int>();
            animationStandingLeft.AddFrame(4, 2.0f);
            var animationStandingLeftUp = new Animation<int>();
            animationStandingLeftUp.AddFrame(5, 2.0f);
            var animationStandingUp = new Animation<int>();
            animationStandingUp.AddFrame(6, 2.0f);
            var animationStandingUpRight = new Animation<int>();
            animationStandingUpRight.AddFrame(7, 2.0f);

            var animationWalkingRight = new Animation<int>();
            animationWalkingRight.AddFrame(0, 2.0f);
            var animationWalkingRightDown = new Animation<int>();
            animationWalkingRightDown.AddFrame(1, 2.0f);
            var animationWalkingDown = new Animation<int>();
            animationWalkingDown.AddFrame(2, 2.0f);
            var animationWalkingDownLeft = new Animation<int>();
            animationWalkingDownLeft.AddFrame(3, 2.0f);
            var animationWalkingLeft = new Animation<int>();
            animationWalkingLeft.AddFrame(4, 2.0f);
            var animationWalkingLeftUp = new Animation<int>();
            animationWalkingLeftUp.AddFrame(5, 2.0f);
            var animationWalkingUp = new Animation<int>();
            animationWalkingUp.AddFrame(6, 2.0f);
            var animationWalkingUpRight = new Animation<int>();
            animationWalkingUpRight.AddFrame(7, 2.0f);

            var animator = new SpriteAnimator(4.0f, true);
            animator.AddAnimation("STANDING_RIGHT", animationStandingRight);
            animator.AddAnimation("STANDING_RIGHT_DOWN", animationStandingRightDown);
            animator.AddAnimation("STANDING_DOWN", animationStandingDown);
            animator.AddAnimation("STANDING_DOWN_LEFT", animationStandingDownLeft);
            animator.AddAnimation("STANDING_LEFT", animationStandingLeft);
            animator.AddAnimation("STANDING_LEFT_UP", animationStandingLeftUp);
            animator.AddAnimation("STANDING_UP", animationStandingUp);
            animator.AddAnimation("STANDING_UP_RIGHT", animationStandingUpRight);
            animator.AddAnimation("WALKING_RIGHT", animationWalkingRight);
            animator.AddAnimation("WALKING_RIGHT_DOWN", animationWalkingRightDown);
            animator.AddAnimation("WALKING_DOWN", animationWalkingDown);
            animator.AddAnimation("WALKING_DOWN_LEFT", animationWalkingDownLeft);
            animator.AddAnimation("WALKING_LEFT", animationWalkingLeft);
            animator.AddAnimation("WALKING_LEFT_UP", animationWalkingLeftUp);
            animator.AddAnimation("WALKING_UP", animationWalkingUp);
            animator.AddAnimation("WALKING_UP_RIGHT", animationWalkingUpRight);

            return animator;
        }

        public static StateMachine CreateStateMachine()
        {
            var stateMachine = new StateMachine();

            stateMachine.AddState(new StandingState("Standing_Right", "STANDING_RIGHT", new Vector2(1, 0)));
            stateMachine.AddState(new StandingState("Standing_Right_Down", "STANDING_RIGHT_DOWN", new Vector2(1, -1)));
            stateMachine.AddState(new StandingState("Standing_Down", "STANDING_DOWN", new Vector2(0, -1)));
            stateMachine.AddState(new StandingState("Standing_Down_Left", "STANDING_DOWN_LEFT", new Vector2(-1, -1)));
            stateMachine.AddState(new StandingState("Standing_Left", "STANDING_LEFT", new Vector2(-1, 0)));
            stateMachine.AddState(new StandingState("Standing_Left_Up", "STANDING_LEFT_UP", new Vector2(-1, 1)));
            stateMachine.AddState(new StandingState("Standing_Up", "STANDING_UP", new Vector2(0, 1)));
            stateMachine.AddState(new StandingState("Standing_Up_Right", "STANDING_UP_RIGHT", new Vector2(1, 1)));

            stateMachine.AddState(new WalkingState("Walking_Right", "WALKING_RIGHT", new Vector2(1, 0)));
            stateMachine.AddState(new WalkingState("Walking_Right_Down", "WALKING_RIGHT_DOWN", new Vector2(1, -1)));
            stateMachine.AddState(new WalkingState("Walking_Down", "WALKING_DOWN", new Vector2(0, -1)));
            stateMachine.AddState(new WalkingState("Walking_Down_Left", "WALKING_DOWN_LEFT", new Vector2(-1, -1)));
            stateMachine.AddState(new WalkingState("Walking_Left", "WALKING_LEFT", new Vector2(-1, 0)));
            stateMachine.AddState(new WalkingState("Walking_Left_Up", "WALKING_LEFT_UP", new Vector2(-1, 1)));
            stateMachine.AddState(new WalkingState("Walking_Up", "WALKING_UP", new Vector2(0, 1)));
            stateMachine.AddState(new WalkingState("Walking_Up_Right", "WALKING_UP_RIGHT", new Vector2(1, 1)));

            return stateMachine;
        }
    }
}
