using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenBreed.Wecs.Components.Physics.Builders
{
    public class BodyFixtureBuilder : IBuilder<BodyFixture>
    {
        #region Private Fields

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly IFixtureMan fixtureMan;
        private readonly List<int> groupIds = new List<int>();
        private readonly IShapeMan shapeMan;
        private IShape shape;

        #endregion Private Fields

        #region Public Constructors

        public BodyFixtureBuilder(
            IFixtureMan fixtureMan,
            IShapeMan shapeMan,
            ICollisionMan<IEntity> collisionMan)
        {
            this.fixtureMan = fixtureMan;
            this.shapeMan = shapeMan;
            this.collisionMan = collisionMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public BodyFixture Build()
        {
            var fixtureId = fixtureMan.NewId();

            var newFixture =  new BodyFixture(
                fixtureId,
                shape,
                groupIds);

            fixtureMan.Add(newFixture);

            return newFixture;
        }

        public void ClearGroups()
        {
            this.groupIds.Clear();
        }

        public void AddGroup(string groupName)
        {
            var groupId = collisionMan.GetGroupId(groupName);
            this.groupIds.Add(groupId);
        }

        public void SetShape(string shapeName)
        {
            this.shape = shapeMan.GetByTag(shapeName);
        }

        #endregion Public Methods
    }
}