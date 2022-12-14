using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{
    public interface IBuilderModule
    {
        #region Public Methods

        void Build(int code, IWorld world, object[] args);

        #endregion Public Methods
    }

    internal class WorldBuilder : IWorldBuilder
    {
        #region Public Fields

        public int height;
        public int width;

        #endregion Public Fields

        #region Internal Fields

        internal readonly WorldMan worldMan;
        internal readonly IEntityToSystemMatcher entityToSystemMatcher;
        internal string name;
        internal Dictionary<Type, Func<IWorld, ISystem>> systemInitializers = new Dictionary<Type, Func<IWorld, ISystem>>();

        internal Dictionary<Type, object> modules = new Dictionary<Type, object>();

        #endregion Internal Fields

        #region Private Fields

        private readonly ILogger logger;
        private readonly ISystemFactory systemFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldBuilder(
            WorldMan worldMan,
            IEntityToSystemMatcher entityToSystemMatcher,
            ILogger logger,
            ISystemFactory systemFactory)
        {
            this.worldMan = worldMan;
            this.entityToSystemMatcher = entityToSystemMatcher;
            this.logger = logger;
            this.systemFactory = systemFactory;
        }

        #endregion Internal Constructors

        #region Public Methods

        internal IEnumerable<ISystem> CreateSystems(IWorld world)
        {
            foreach (var initializer in systemInitializers.Values)
                yield return initializer.Invoke(world);
        }

        public IWorldBuilder AddModule<TModule>(TModule module)
        {
            var moduleType = typeof(TModule);

            if (modules.ContainsKey(moduleType))
                throw new InvalidOperationException($"Module with type '{moduleType}' already added.");

            modules.Add(moduleType, module);
            return this;
        }

        public IWorldBuilder AddSystem<TSystem>() where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemInitializers.ContainsKey(systemType))
                throw new InvalidOperationException($"System with type '{systemType}' already added.");

            systemInitializers.Add(systemType, systemFactory.CreateSystem<TSystem>);
            return this;
        }

        public IWorld Build()
        {
            if (worldMan.GetByName(name) != null)
                throw new InvalidOperationException($"World with name '{name}' already exist.");

            var newWorld = new World(this);
            worldMan.RegisterWorld(newWorld);
            return newWorld;
        }

        public IWorldBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public IWorldBuilder SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        #endregion Public Methods
    }
}