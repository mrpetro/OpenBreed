using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections;
using System.Diagnostics;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class FixtureMan : IFixtureMan
    {
        #region Private Fields

        private readonly IdMap<IFixture> items = new IdMap<IFixture>();
        private readonly Hashtable tagsToIds = new Hashtable();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FixtureMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Add(IFixture fixture)
        {
            items.Insert(fixture.Id, fixture);
        }

        #endregion Public Properties

        #region Public Methods

        public IFixture GetById(int id)
        {
            if (items.TryGetValue(id, out IFixture entity))
                return entity;
            else
                return null;
        }

        public int NewId()
        {
            return items.NewId();
        }

        public void Remove(int fixtureId)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}