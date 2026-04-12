namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IEntityBuilder
    {
        #region Public Methods

        IEntityBuilder SetTag(string entityTag);

        IEntity Build();

        IEntityBuilder AddComponent<TEntityComponent>(TEntityComponent component) where TEntityComponent : IEntityComponent;

        #endregion Public Methods
    }
}