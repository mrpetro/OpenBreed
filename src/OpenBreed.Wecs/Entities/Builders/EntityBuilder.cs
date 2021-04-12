using OpenBreed.Core;
using System;

namespace OpenBreed.Wecs.Entities.Builders
{
    public abstract class EntityBuilder : IEntityBuilder
    {
        protected readonly IEntityMan entityMan;
        #region Public Constructors

        public EntityBuilder(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
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