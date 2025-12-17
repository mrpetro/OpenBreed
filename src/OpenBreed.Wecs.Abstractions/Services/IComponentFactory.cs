namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IComponentFactory
    {
        IEntityComponent Create(IComponentTemplate data);
    }

    public interface IComponentFactory<TComponentTemplate> : IComponentFactory where TComponentTemplate : IComponentTemplate
    {
    }
}
