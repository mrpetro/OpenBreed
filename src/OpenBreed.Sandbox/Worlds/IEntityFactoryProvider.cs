using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Builders;
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Worlds
{
    public interface IEnityFactory
    {
        Entity Create();
    }

    public interface IEntityFactoryProvider
    {
        IEnityFactory GetFactory(Type type);
    }


    public class EntityFactoryProvider : IEntityFactoryProvider
    {
        #region Private Fields

        private readonly Dictionary<Type, Func<IEnityFactory>> factoryInitializers = new Dictionary<Type, Func<IEnityFactory>>();

        #endregion Private Fields

        #region Public Methods

        public void Register<TEntityFactory>(Func<IEnityFactory> builderInitializer) where TEntityFactory : IEnityFactory
        {
            factoryInitializers.Add(typeof(TEntityFactory), builderInitializer);
        }

        public IEnityFactory GetFactory(Type type)
        {
            if (!factoryInitializers.TryGetValue(type, out Func<IEnityFactory> initializer))
                return null;

            return initializer.Invoke();
        }

        #endregion Public Methods
    }
}