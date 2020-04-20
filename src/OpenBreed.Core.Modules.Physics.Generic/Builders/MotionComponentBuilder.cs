using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class MotionComponentBuilder : BaseComponentBuilder<MotionComponentBuilder, MotionComponent>
    {
        #region Protected Constructors

        protected MotionComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new MotionComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new MotionComponent();
        }

        public override void SetProperty(object key, object value)
        {
        }

        #endregion Public Methods
    }
}