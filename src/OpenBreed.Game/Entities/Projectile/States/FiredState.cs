using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Projectile.States
{
    public class FiredState : IState
    {
        public IEntity Entity { get; private set; }
        private IThrust thrust;
        private Animator<int> spriteAnimation;
        private IDirection direction;
        private readonly string animationId;
        private readonly Vector2 fireDirection;

        public FiredState(string id, string animationId, Vector2 fireDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.fireDirection = fireDirection;
        }

        public string Id { get; }

        public void EnterState()
        {
            thrust.Value = fireDirection;

            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Projectile - Fired"));
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
                case "Destroy":
                    {
                        var walkDirection = (Vector2)arguments[0];

                        if (walkDirection.X == 1 && walkDirection.Y == 0)
                            return "Destroy_Right";
                        else if (walkDirection.X == 1 && walkDirection.Y == -1)
                            return "Destroy_Right_Down";
                        else if (walkDirection.X == 0 && walkDirection.Y == -1)
                            return "Destroy_Down";
                        else if (walkDirection.X == -1 && walkDirection.Y == -1)
                            return "Destroy_Down_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 0)
                            return "Destroy_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 1)
                            return "Destroy_Left_Up";
                        else if (walkDirection.X == 0 && walkDirection.Y == 1)
                            return "Destroy_Up";
                        else if (walkDirection.X == 1 && walkDirection.Y == 1)
                            return "Destroy_Up_Right";
                        break;
                    }
                default:
                    break;
            }

            return null;
        }
    }
}
