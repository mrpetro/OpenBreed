using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Actor.States.Attacking
{
    public class AimingState : IState
    {
        public IEntity Entity { get; private set; }
        private IDirection direction;
        private readonly string animationId;
        private readonly Vector2 facingDirection;

        public AimingState(string id, string animationId, Vector2 facingDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.facingDirection = facingDirection;
        }

        public string Id { get; }

        public void EnterState()
        {
           // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            //Entity.PostMsg(new TextSetMsg(Entity, "Hero - Standing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            direction = entity.Components.OfType<IDirection>().First();
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
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
