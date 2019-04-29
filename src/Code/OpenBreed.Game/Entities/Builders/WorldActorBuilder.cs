using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
using OpenTK;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldActorBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;
        internal SpriteAtlas spriteAtlas;
        internal int spriteId;

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

        public void SetSpriteId(int spriteId)
        {
            this.spriteId = spriteId;
        }
        public void SetSpriteAtlas(SpriteAtlas spriteAtlas)
        {
            this.spriteAtlas = spriteAtlas;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        #endregion Public Methods
    }
}