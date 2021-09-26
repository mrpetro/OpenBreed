using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Common
{
    public class BuilderFactory : IBuilderFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Func<IBuilder>> initializers = new Dictionary<Type, Func<IBuilder>>();

        #endregion Private Fields

        #region Public Methods

        public TBuilder GetBuilder<TBuilder>()
        {
            if (initializers.TryGetValue(typeof(TBuilder), out Func<IBuilder> initializer))
                return (TBuilder)initializer.Invoke();
            else
                throw new InvalidOperationException($"Builder of type '{typeof(TBuilder)}' is not registered");
        }

        public void Register<TBuilder>(Func<IBuilder> initializer)
        {
            Debug.Assert(!initializers.ContainsKey(typeof(TBuilder)), $"Builder of type '{typeof(TBuilder)}' already registered.");
            initializers.Add(typeof(TBuilder), initializer);
        }

        #endregion Public Methods
    }
}