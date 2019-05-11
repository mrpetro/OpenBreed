using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Components.Shapes;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Core.Systems.Rendering.Components;
using OpenBreed.Game.Components;
using OpenBreed.Game.Entities.Builders;
using OpenTK;

namespace OpenBreed.Game.Entities
{
    public class WorldActor : WorldEntity
    {
        #region Private Fields

        private Vector2 position;

        #endregion Private Fields

        #region Public Constructors

        public WorldActor(WorldActorBuilder builder) : base(builder)
        {
            this.position = builder.position;

            var position = new DynamicPosition(builder.position);
            var direction = new Direction(builder.direction);
            Components.Add(position);
            Components.Add(direction);
            Components.Add(new Sprite(builder.spriteAtlas));
            Components.Add(new SpriteDebug());
            Components.Add(new CreatureMovement());
            Components.Add(new CreatureAnimator());
            Components.Add(new AxisAlignedBoxShape(32, 32));
            Components.Add(new DynamicBody());

            if (builder.controller != null)
                Components.Add(builder.controller);
        }

        #endregion Public Constructors
    }
}