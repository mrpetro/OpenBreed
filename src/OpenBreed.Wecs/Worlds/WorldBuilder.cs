﻿using Microsoft.Extensions.Logging;
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
        #region Internal Fields

        internal readonly WorldMan worldMan;
        internal readonly IEntityToSystemMatcher entityToSystemMatcher;
        internal string name;
        internal Dictionary<Type, Func<ISystem>> systemInitializers = new Dictionary<Type, Func<ISystem>>();

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
                yield return initializer.Invoke();
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

        #endregion Public Methods
    }
}