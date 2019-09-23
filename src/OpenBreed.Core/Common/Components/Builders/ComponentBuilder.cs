using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components.Builders
{
    public abstract class ComponentBuilder
    {
        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Protected Constructors

        protected ComponentBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Protected Constructors

        #region Public Properties

        public abstract string Type { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract IEntityComponent Build();

        #endregion Public Methods
    }
}