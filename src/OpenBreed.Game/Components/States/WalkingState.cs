using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Animation.Messages;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Movement.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Components.States
{
    public class WalkingState : IState
    {
        private IEntity entity;
        private IThrust thrust;
        private Motion creatureMovement;
        private Animator<int> spriteAnimation;
        private IDirection direction;
        private readonly string animationId;
        private readonly Vector2 walkDirection;
        public WalkingState(string id, string animationId, Vector2 walkDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.walkDirection = walkDirection;
        }

        public string Id { get; }

        public void EnterState()
        {
            thrust.Value = walkDirection * creatureMovement.Speed;

            entity.PostMessage(new PlayAnimMsg(animationId));
        }

        public void Initialize(IEntity entity)
        {
            this.entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
            direction = entity.Components.OfType<IDirection>().First();
            creatureMovement = entity.Components.OfType<Motion>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Stop":
                    {
                        if (walkDirection.X == 1 && walkDirection.Y == 0)
                            return "Standing_Right";
                        else if (walkDirection.X == 1 && walkDirection.Y == -1)
                            return "Standing_Right_Down";
                        else if (walkDirection.X == 0 && walkDirection.Y == -1)
                            return "Standing_Down";
                        else if (walkDirection.X == -1 && walkDirection.Y == -1)
                            return "Standing_Down_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 0)
                            return "Standing_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 1)
                            return "Standing_Left_Up";
                        else if (walkDirection.X == 0 && walkDirection.Y == 1)
                            return "Standing_Up";
                        else if (walkDirection.X == 1 && walkDirection.Y == 1)
                            return "Standing_Up_Right";
                        break;
                    }
                case "Walk":
                    {
                        var walkDirection = (Vector2)arguments[0];

                        if (walkDirection.X == 1 && walkDirection.Y == 0)
                            return "Walking_Right";
                        else if (walkDirection.X == 1 && walkDirection.Y == -1)
                            return "Walking_Right_Down";
                        else if (walkDirection.X == 0 && walkDirection.Y == -1)
                            return "Walking_Down";
                        else if (walkDirection.X == -1 && walkDirection.Y == -1)
                            return "Walking_Down_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 0)
                            return "Walking_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 1)
                            return "Walking_Left_Up";
                        else if (walkDirection.X == 0 && walkDirection.Y == 1)
                            return "Walking_Up";
                        else if (walkDirection.X == 1 && walkDirection.Y == 1)
                            return "Walking_Up_Right";
                        break;
                    }
                default:
                    break;
            }

            return null;
        }
    }
}
