namespace OpenBreed.Wecs.Entities.Builders
{
    public abstract class EntityBuilder : IEntityBuilder
    {
        #region Protected Fields

        protected readonly IEntityMan entityMan;
        protected string tag;

        #endregion Protected Fields

        #region Public Constructors

        public EntityBuilder(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public abstract IEntity Build();

        public IEntityBuilder SetTag(string tag)
        {
            this.tag = tag;
            return this;
        }

        #endregion Public Methods
    }
}