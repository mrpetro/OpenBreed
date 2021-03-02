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
        private readonly IWorldMan worldMan;

        #endregion Internal Fields

        #region Private Fields

        private readonly ILogger logger;
        private Dictionary<int, Action<World, int, object[]>> codesToActions = new Dictionary<int, Action<World, int, object[]>>();

        private List<Tuple<int, object[]>> actions = new List<Tuple<int, object[]>>();

        #endregion Private Fields

        #region Public Constructors

        public WorldBuilder(IWorldMan worldMan, ILogger logger)
        {
            this.worldMan = worldMan;
            this.logger = logger;
        }

        public void AddSystem(ISystem system)
        {
            var systemType = system.GetType();

            if (systems.ContainsKey(systemType))
                throw new InvalidOperationException($"System with type '{systemType}' already added.");

            systems.Add(systemType, system);
        }

        #endregion Public Constructors

        #region Public Methods

        internal void InvokeActions(World world)
        {
            foreach (var action in actions)
            {
                Action<World, int, object[]> actionHandler = null;

                if (codesToActions.TryGetValue(action.Item1, out actionHandler))
                    actionHandler.Invoke(world, action.Item1, action.Item2);
                else
                    logger.Warning($"Unsupported action code '{action.Item1}'");
            }
        }

        public World Build(ICore core)
        {
            if(worldMan.GetByName(name) != null)
                throw new InvalidOperationException($"World with name '{name}' already exist.");

            return new World(this, core); 
        }

        public WorldBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public void RegisterCode(int code, Action<World, int, object[]> action)
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
    }
}