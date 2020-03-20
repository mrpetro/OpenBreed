
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState
    {
        #region Private Fields

        private readonly string animPrefix;
        private DirectionComponent direction;
        private SpriteComponent sprite;

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
            var direction = Entity.GetComponent<DirectionComponent>();
            var movement = Entity.GetComponent<MotionComponent>();
            Entity.GetComponent<ThrustComponent>().Value = direction.Value * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);

            Entity.PostCommand(new PlayAnimCommand(Entity.Id, $"{animPrefix}/{Name}/{animDirPostfix}"));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe<AnimChangedEventArgs>(OnFrameChanged);
            Entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }
     
        public void Initialize(IEntity entity)
        {
            Entity = entity;
            direction = Entity.TryGetComponent<DirectionComponent>();
            sprite = entity.GetComponent<SpriteComponent>();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<AnimChangedEventArgs>(OnFrameChanged);
            Entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
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

        private void OnFrameChanged(object sender, AnimChangedEventArgs e)
        {
            sprite.ImageId = (int)e.Frame;
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            if (e.Direction != Vector2.Zero)
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Movement", "Walk"));
            else
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Movement", "Stop"));
        }

        #endregion Private Methods
    }
}