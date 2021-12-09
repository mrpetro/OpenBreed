using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{
    public interface IBuilderModule
    {
        #region Public Methods

        void Build(int code, World world, object[] args);

        #endregion Public Methods
    }

    public class WorldBuilder
    {
        #region Public Fields

        public int height;
        public int width;

        #endregion Public Fields

        #region Internal Fields

        internal readonly IWorldMan worldMan;
        internal string name;
        internal Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();
        internal Dictionary<Type, object> modules = new Dictionary<Type, object>();

        #endregion Internal Fields

        #region Private Fields

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldBuilder(IWorldMan worldMan, ILogger logger)
        {
            this.worldMan = worldMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void AddModule<TModule>(TModule module)
        {
            var moduleType = typeof(TModule);

            if (systems.ContainsKey(moduleType))
                throw new InvalidOperationException($"Module with type '{moduleType}' already added.");

            modules.Add(moduleType, module);
        }

        public void AddSystem(ISystem system)
        {
            var systemType = system.GetType();

            if (systems.ContainsKey(systemType))
                throw new InvalidOperationException($"System with type '{systemType}' already added.");

            systems.Add(systemType, system);
        }

        public World Build()
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