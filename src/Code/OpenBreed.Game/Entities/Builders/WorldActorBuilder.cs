using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenTK;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldActorBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;
        internal Vector2 direction;
        internal SpriteAtlas spriteAtlas;
        internal IControlComponent controller;

        #endregion Internal Fields

        #region Public Constructors

        public WorldActorBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IEntity Build()
        {
            return new WorldActor(this);
        }

        public void SetSpriteAtlas(SpriteAtlas spriteAtlas)
        {
            this.spriteAtlas = spriteAtlas;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetController(IControlComponent controller)
        {
            this.controller = controller;
        }

        #endregion Internal Methods
    }
}