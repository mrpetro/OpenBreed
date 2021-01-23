using OpenBreed.Core;

namespace OpenBreed.Wecs.Components
{
    public abstract class ComponentFactoryBase<TComponentTemplate> : IComponentFactory where TComponentTemplate : IComponentTemplate
    {
        #region Protected Fields

        protected readonly ICore core;

        #endregion Protected Fields

        #region Public Constructors

        protected ComponentFactoryBase(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntityComponent Create(IComponentTemplate template) => Create((TComponentTemplate)template);

        #endregion Public Methods

        #region Protected Methods

        protected abstract IEntityComponent Create(TComponentTemplate template);

        #endregion Protected Methods
    }
}