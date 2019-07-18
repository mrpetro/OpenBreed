using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Game.Entities.Actor.States
{
    public class StandingState : IState
    {
        public IEntity Entity { get; private set; }
        private IThrust thrust;
        private Animator<int> spriteAnimation;
        private IDirection direction;
        private readonly string animationId;
        private readonly Vector2 facingDirection;

        public StandingState(string id, string animationId, Vector2 facingDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.facingDirection = facingDirection;
        }

        public string Id { get; }

        public void EnterState()
        {
            thrust.Value = Vector2.Zero;
            Entity.PostMessage(new PlayAnimMsg(animationId));
            Entity.PostMessage(new SetTextMsg("Hero - Standing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();
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
