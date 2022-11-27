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

    public class WorldBuilder
    {
        #region Public Fields

        public int height;
        public int width;

        #endregion Public Fields

        #region Internal Fields

        internal readonly WorldMan worldMan;
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
            ILogger logger,
            ISystemFactory systemFactory)
        {
            this.worldMan = worldMan;
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

        public void AddModule<TModule>(TModule module)
        {
            var moduleType = typeof(TModule);

            if (modules.ContainsKey(moduleType))
                throw new InvalidOperationException($"Module with type '{moduleType}' already added.");

            modules.Add(moduleType, module);
        }

        public void AddSystem<TSystem>(Func<IWorld, ISystem> initializer = null) where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemInitializers.ContainsKey(systemType))
                throw new InvalidOperationException($"System with type '{systemType}' already added.");

            if (initializer is null)
                initializer = systemFactory.Create<TSystem>;

            systemInitializers.Add(systemType, initializer);
        }

        public IWorld Build()
        {
            if (worldMan.GetByName(name) != null)
                throw new InvalidOperationException($"World with name '{name}' already exist.");

            var newWorld = new World(this);
            worldMan.RegisterWorld(newWorld);
            return newWorld;
        }

        public WorldBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        #endregion Public Methods
    }
}