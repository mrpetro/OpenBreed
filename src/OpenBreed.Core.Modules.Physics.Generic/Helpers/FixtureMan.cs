using OpenBreed.Core.Common.Systems.Shapes;
using OpenBreed.Core.Modules.Physics.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public class FixtureMan
    {
        #region Private Fields

        private readonly List<Fixture> items = new List<Fixture>();
        private readonly Dictionary<string, Fixture> aliases = new Dictionary<string, Fixture>();

        #endregion Private Fields

        #region Internal Constructors

        internal FixtureMan(PhysicsModule module)
        {
            Debug.Assert(module != null);

            Module = module;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal PhysicsModule Module { get; }

        #endregion Internal Properties

        #region Public Methods

        public Fixture Create(string alias, string type, IShape shape)
        {
            Fixture result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            result = new Fixture(items.Count, shape);
            items.Add(result);
            aliases.Add(alias, result);
            return result;
        }

        public Fixture GetById(int id)
        {
            return items[id];
        }

        public Fixture GetByAlias(string alias)
        {
            Fixture result;
            aliases.TryGetValue(alias, out result);
            return result;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}