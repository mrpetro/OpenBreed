namespace OpenBreed.Wecs.Components.Gui
{
    public interface ICursorInputComponentTemplate : IComponentTemplate
    {
    }

    public class CursorInputComponent : IEntityComponent
    {
        #region Public Constructors

        public CursorInputComponent()
        {
        }

        #endregion Public Constructors
    }

    public sealed class CursorInputComponentFactory : ComponentFactoryBase<ICursorInputComponentTemplate>
    {
        #region Public Constructors

        public CursorInputComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICursorInputComponentTemplate template)
        {
            return new CursorInputComponent();
        }

        #endregion Protected Methods
    }
}