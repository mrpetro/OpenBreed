namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IComponentTemplate
    {
        #region Public Methods

        IEntityComponent ToComponent(IServiceProvider serviceProvider);

        #endregion Public Methods
    }
}