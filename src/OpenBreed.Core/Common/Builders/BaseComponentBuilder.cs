using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Builders
{
    /// <summary>
    /// Base entity component builder abstract class
    /// </summary>
    public abstract class BaseComponentBuilder : IComponentBuilder
    {
        #region Protected Constructors

        protected BaseComponentBuilder(ICore core)
        {
            Core = core;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected ICore Core { get; }

        #endregion Protected Properties

        #region Public Methods

        public abstract IEntityComponent Build();

        public abstract void SetProperty(object key, object value);

        #endregion Public Methods
    }
}