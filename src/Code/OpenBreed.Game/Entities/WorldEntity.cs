using OpenBreed.Game.Entities.Builders;

namespace OpenBreed.Game.Entities
{
    public class WorldEntity : EntityBase
    {
        #region Public Constructors

        public WorldEntity(WorldEntityBuilder builder) : base(builder.Core)
        {
            World = builder.world;
        }

        #endregion Public Constructors

        #region Public Properties

        public World World { get; }

        #endregion Public Properties
    }
}