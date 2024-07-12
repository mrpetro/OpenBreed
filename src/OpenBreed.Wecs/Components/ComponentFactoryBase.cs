namespace OpenBreed.Wecs.Components
{
    public abstract class ComponentFactoryBase<TComponentTemplate> : IComponentFactory<TComponentTemplate> where TComponentTemplate : IComponentTemplate
    {
        #region Public Methods

        public IEntityComponent Create(IComponentTemplate template) => Create((TComponentTemplate)template);

        #endregion Public Methods

        #region Protected Methods

        protected abstract IEntityComponent Create(TComponentTemplate template);

        #endregion Protected Methods
    }
}