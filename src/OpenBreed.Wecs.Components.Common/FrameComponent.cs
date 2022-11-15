using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IFrameComponentTemplate : IComponentTemplate
    {
    }

    public class FrameComponent : IEntityComponent
    {
        #region Public Constructors

        public FrameComponent()
        {
            Target = -1;
        }

        /// <summary>
        /// Target frame 
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Current frame
        /// </summary>
        public int Current { get; set; }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties
    }

    public sealed class FrameComponentFactory : ComponentFactoryBase<IFrameComponentTemplate>
    {
        #region Internal Constructors

        public FrameComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IFrameComponentTemplate template)
        {
            return new FrameComponent();
        }

        #endregion Protected Methods
    }
}