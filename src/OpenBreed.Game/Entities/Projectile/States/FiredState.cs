using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Projectile.States
{
    public class FiredState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private readonly Vector2 fireDirection;
        private IThrust thrust;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string id, string animationId, Vector2 fireDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.fireDirection = fireDirection;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            thrust.Value = fireDirection;

            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Projectile - Fired"));
            Entity.Subscribe(CollisionEvent.TYPE, OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
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

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, IEvent e)
        {
            HandleCollisionEvent((CollisionEvent)e);
        }

        private void HandleCollisionEvent(CollisionEvent e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Open"));
        }

        #endregion Private Methods
    }
}