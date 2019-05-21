using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Movement.Components;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Core.Systems.Rendering.Components;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenBreed.Game.Components;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldActorBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Position position;
        internal Direction direction;
        internal IShapeComponent shape;
        internal IAnimationComponent animator;
        internal IPhysicsComponent body;
        internal IRenderComponent sprite;
        internal SpriteAtlas spriteAtlas;
        internal IControllerComponent controller;
        internal CreatureMovement movement;

        #endregion Internal Fields

        #region Public Constructors

        public WorldActorBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IEntity Build()
        {
            if (position == null)
                CoreTools.ThrowComponentRequiredException<Position>();

            if (direction == null)
                CoreTools.ThrowComponentRequiredException<Direction>();

            return new WorldActor(this);
        }

        public void SetAnimator(IAnimationComponent animator)
        {
            this.animator = animator;
        }

        public void SetShape(IShapeComponent shape)
        {
            this.shape = shape;
        }

        public void SetMovement(CreatureMovement movement)
        {
            this.movement = movement;
        }

        public void SetBody(IPhysicsComponent body)
        {
            this.body = body;
        }

        public void SetSpriteAtlas(SpriteAtlas spriteAtlas)
        {
            this.spriteAtlas = spriteAtlas;
        }

        public void SetPosition(Position position)
        {
            this.position = position;
        }

        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        public void SetController(IControllerComponent controller)
        {
            this.controller = controller;
        }

        public void SetSprite(IRenderComponent sprite)
        {
            this.sprite = sprite;
        }

        #endregion Public Methods
    }
}