using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{
    public class WorldBuilder
    {
        #region Public Fields

        public int height;
        public int width;

        #endregion Public Fields

        #region Internal Fields

        internal string name;
        internal Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();

        #endregion Internal Fields

        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly ILogger logger;
        private Dictionary<int, Action<ICore, World, int, object[]>> codesToActions = new Dictionary<int, Action<ICore, World, int, object[]>>();

        private List<Tuple<int, object[]>> actions = new List<Tuple<int, object[]>>();

        #endregion Private Fields

        #region Public Constructors

        public WorldBuilder(IWorldMan worldMan, ILogger logger)
        {
            this.worldMan = worldMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddSystem(ISystem system)
        {
            var systemType = system.GetType();

            if (systems.ContainsKey(systemType))
                throw new InvalidOperationException($"System with type '{systemType}' already added.");

            systems.Add(systemType, system);
        }

        public World Build(ICore core)
        {
            if (worldMan.GetByName(name) != null)
                throw new InvalidOperationException($"World with name '{name}' already exist.");

            var newWorld = new World(this);
            worldMan.RegisterWorld(newWorld);
            InvokeActions(core, newWorld);

            return newWorld;
        }

        public WorldBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public void RegisterCode(int code, Action<ICore, World, int, object[]> action)
        {
            codesToActions.Add(code, action);
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Add(int code, object[] args)
        {
            actions.Add(new Tuple<int, object[]>(code, args));
        }

        #endregion Public Methods

        #region Private Methods

        private void InvokeActions(ICore core, World world)
        {
            foreach (var action in actions)
            {
                Action<ICore, World, int, object[]> actionHandler = null;

                if (codesToActions.TryGetValue(action.Item1, out actionHandler))
                    actionHandler.Invoke(core, world, action.Item1, action.Item2);
                else
                    logger.Warning($"Unsupported action code '{action.Item1}'");
            }
        }

        #endregion Private Methods
    }
}