using System;

namespace OpenBreed.Core.Entities.Builders
{
    public abstract class WorldEntityBuilder : IEntityBuilder
    {
        #region Public Constructors

        public WorldEntityBuilder(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract IEntity Build();

        #endregion Public Methods
    }
}