using System;
using OpenBreed.Game.Control.Components;
using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
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

        public WorldActorBuilder(GameState core) : base(core)
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

        internal void SetController(IControlComponent controller)
        {
            this.controller = controller;
        }

        #endregion Public Methods
    }
}