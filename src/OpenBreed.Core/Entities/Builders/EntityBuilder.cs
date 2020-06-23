using System;

namespace OpenBreed.Core.Entities.Builders
{
    public abstract class EntityBuilder : IEntityBuilder
    {
        #region Public Constructors

        public EntityBuilder(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract Entity Build();

        #endregion Public Methods
    }
}