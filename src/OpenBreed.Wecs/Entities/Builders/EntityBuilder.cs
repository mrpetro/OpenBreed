namespace OpenBreed.Wecs.Entities.Builders
{
    public abstract class EntityBuilder : IEntityBuilder
    {
        #region Protected Fields

        protected readonly IEntityMan entityMan;

        #endregion Protected Fields

        #region Public Constructors

        public EntityBuilder(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public abstract Entity Build();

        #endregion Public Methods
    }
}