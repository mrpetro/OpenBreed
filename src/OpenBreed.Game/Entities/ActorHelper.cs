﻿using OpenBreed.Core;
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

        public static Animator<int> CreateAnimation(ICore core)
        {
            var animationStandingRight = core.Animations.Create<int>("STANDING_RIGHT");
            animationStandingRight.AddFrame(0, 2.0f);
            var animationStandingRightDown = core.Animations.Create<int>("STANDING_RIGHT_DOWN");
            animationStandingRightDown.AddFrame(1, 2.0f);
            var animationStandingDown = core.Animations.Create<int>("STANDING_DOWN");
            animationStandingDown.AddFrame(2, 2.0f);
            var animationStandingDownLeft = core.Animations.Create<int>("STANDING_DOWN_LEFT");
            animationStandingDownLeft.AddFrame(3, 2.0f);
            var animationStandingLeft = core.Animations.Create<int>("STANDING_LEFT");
            animationStandingLeft.AddFrame(4, 2.0f);
            var animationStandingLeftUp = core.Animations.Create<int>("STANDING_LEFT_UP");
            animationStandingLeftUp.AddFrame(5, 2.0f);
            var animationStandingUp = core.Animations.Create<int>("STANDING_UP");
            animationStandingUp.AddFrame(6, 2.0f);
            var animationStandingUpRight = core.Animations.Create<int>("STANDING_UP_RIGHT");
            animationStandingUpRight.AddFrame(7, 2.0f);

            var animationWalkingRight = core.Animations.Create<int>("WALKING_RIGHT");
            animationWalkingRight.AddFrame(0, 1.0f);
            animationWalkingRight.AddFrame(8, 1.0f);
            animationWalkingRight.AddFrame(16, 1.0f);
            animationWalkingRight.AddFrame(24, 1.0f);
            animationWalkingRight.AddFrame(32, 1.0f);
            var animationWalkingRightDown = core.Animations.Create<int>("WALKING_RIGHT_DOWN");
            animationWalkingRightDown.AddFrame(1, 1.0f);
            animationWalkingRightDown.AddFrame(9, 1.0f);
            animationWalkingRightDown.AddFrame(17, 1.0f);
            animationWalkingRightDown.AddFrame(25, 1.0f);
            animationWalkingRightDown.AddFrame(33, 1.0f);
            var animationWalkingDown = core.Animations.Create<int>("WALKING_DOWN");
            animationWalkingDown.AddFrame(2, 1.0f);
            animationWalkingDown.AddFrame(10, 1.0f);
            animationWalkingDown.AddFrame(18, 1.0f);
            animationWalkingDown.AddFrame(26, 1.0f);
            animationWalkingDown.AddFrame(34, 1.0f);
            var animationWalkingDownLeft = core.Animations.Create<int>("WALKING_DOWN_LEFT");
            animationWalkingDownLeft.AddFrame(3, 1.0f);
            animationWalkingDownLeft.AddFrame(11, 1.0f);
            animationWalkingDownLeft.AddFrame(19, 1.0f);
            animationWalkingDownLeft.AddFrame(27, 1.0f);
            animationWalkingDownLeft.AddFrame(35, 1.0f);
            var animationWalkingLeft = core.Animations.Create<int>("WALKING_LEFT");
            animationWalkingLeft.AddFrame(4, 1.0f);
            animationWalkingLeft.AddFrame(12, 1.0f);
            animationWalkingLeft.AddFrame(20, 1.0f);
            animationWalkingLeft.AddFrame(28, 1.0f);
            animationWalkingLeft.AddFrame(36, 1.0f);
            var animationWalkingLeftUp = core.Animations.Create<int>("WALKING_LEFT_UP");
            animationWalkingLeftUp.AddFrame(5, 1.0f);
            animationWalkingLeftUp.AddFrame(13, 1.0f);
            animationWalkingLeftUp.AddFrame(21, 1.0f);
            animationWalkingLeftUp.AddFrame(29, 1.0f);
            animationWalkingLeftUp.AddFrame(37, 1.0f);
            var animationWalkingUp = core.Animations.Create<int>("WALKING_UP");
            animationWalkingUp.AddFrame(6, 1.0f);
            animationWalkingUp.AddFrame(14, 1.0f);
            animationWalkingUp.AddFrame(22, 1.0f);
            animationWalkingUp.AddFrame(30, 1.0f);
            animationWalkingUp.AddFrame(38, 1.0f);
            var animationWalkingUpRight = core.Animations.Create<int>("WALKING_UP_RIGHT");
            animationWalkingUpRight.AddFrame(7, 1.0f);
            animationWalkingUpRight.AddFrame(15, 1.0f);
            animationWalkingUpRight.AddFrame(23, 1.0f);
            animationWalkingUpRight.AddFrame(31, 1.0f);
            animationWalkingUpRight.AddFrame(39, 1.0f);

            var animation = new Animator<int>(10.0f, true);
            return animation;
        }

        public static StateMachine CreateStateMachine(IEntity entity)
        {
            var stateMachine = new StateMachine(entity);

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