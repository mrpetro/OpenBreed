using OpenBreed.Game.States;
using OpenTK;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldCreatureBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;

        #endregion Internal Fields

        #region Public Constructors

        public WorldCreatureBuilder(GameState core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IEntity Build()
        {
            return new WorldCreature(this);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        #endregion Public Methods
    }
}