namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IEntityBuilder
    {
        IEntityBuilder SetTag(string entityTag);

        IEntity Build();
    }
}
