using System;

namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IComponentFactoryProvider
    {
        IComponentFactory GetFactory(Type componentType);
    }
}
