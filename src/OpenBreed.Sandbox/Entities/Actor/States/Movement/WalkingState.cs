using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState
    {
        #region Private Fields

        private readonly string animPrefix;
        private Direction direction;
        private ISpriteComponent sprite;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState(string id, string animPrefix)
        {
            Name = id;
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            var direction = Entity.Components.OfType<Direction>().First();
            var movement = Entity.Components.OfType<Motion>().First();
            Entity.Components.OfType<Thrust>().First().Value = direction.Value * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);

            Entity.PostMsg(new PlayAnimMsg(Entity, $"{animPrefix}/{Name}/{animDirPostfix}"));
            Entity.PostMsg(new TextSetMsg(Entity.World.Id, Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(AnimChangedEvent.TYPE, OnFrameChanged);
            Entity.Subscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }
     
        public void Initialize(IEntity entity)
        {
            Entity = entity;
            direction = Entity.Components.OfType<Direction>().FirstOrDefault();
            sprite = entity.Components.OfType<ISpriteComponent>().First();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimChangedEvent.TYPE, OnFrameChanged);
            Entity.Unsubscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Stop":
                    {
                        return "Standing";

                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameChanged(object sender, IEvent e)
        {
            HandleFrameChangeEvent((AnimChangedEvent)e);
        }

        private void OnControlDirectionChanged(object sender, IEvent e)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)e);
        }

        private void HandleFrameChangeEvent(AnimChangedEvent systemEvent)
        {
            sprite.ImageId = (int)systemEvent.Frame;
        }

        private void HandleControlDirectionChangedEvent(ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Walk"));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Stop"));
        }

        #endregion Private Methods
    }
}