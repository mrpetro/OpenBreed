using OpenBreed.Game.States;
using System;

namespace OpenBreed.Game.Entities.Builders
{
    public abstract class WorldEntityBuilder : IEntityBuilder
    {
        #region Internal Fields

        internal World world;

        #endregion Internal Fields

        #region Public Constructors

        public WorldEntityBuilder(GameState core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public GameState Core { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract IEntity Build();

        public void SetWorld(World world)
        {
            this.world = world;
        }

        #endregion Public Methods
    }
}