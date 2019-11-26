using System;
using System.Collections.Generic;
using OpenBreed.Core.Common;

namespace OpenBreed.Core
{
    public class WorldBuilder
    {
        public int Height;
        public int Width;
        public string Name;
        private ICore core;
        private Dictionary<int, Action<World, int, object[] >> codesToActions = new Dictionary<int, Action<World, int, object[]>>();

        private List<Tuple<int, object[]>> actions = new List<Tuple<int, object[]>>();

        public WorldBuilder(ICore core)
        {
            this.core = core;
        }

        public World Build()
        {
            var world = core.Worlds.Create(Name);

            foreach (var action in actions)
            {
                Action<World, int, object[]> actionHandler = null;

                if (codesToActions.TryGetValue(action.Item1, out actionHandler))
                    actionHandler.Invoke(world, action.Item1, action.Item2);
                else
                    core.Logging.Warning($"Unsupported action code '{action.Item1}'");
            }

            return world;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void RegisterCode(int code, Action<World, int, object[]> action)
        {
            codesToActions.Add(code, action);
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Add(int code, object[] args)
        {
            actions.Add(new Tuple<int, object[]>(code, args));
        }
    }
}