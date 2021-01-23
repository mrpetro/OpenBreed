using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class FixtureMan : IFixtureMan
    {
        #region Private Fields

        private readonly List<Fixture> items = new List<Fixture>();
        private readonly Dictionary<string, Fixture> aliases = new Dictionary<string, Fixture>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal FixtureMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IFixture Create(string alias, string type, IShape shape)
        {
            Fixture result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            result = new Fixture(items.Count, shape);
            items.Add(result);
            aliases.Add(alias, result);
            return result;
        }

        public IFixture GetById(int id)
        {
            return items[id];
        }

        public IFixture GetByAlias(string alias)
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