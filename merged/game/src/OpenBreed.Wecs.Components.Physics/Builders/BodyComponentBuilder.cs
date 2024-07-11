using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics.Builders
{
    public class BodyComponentBuilder : IBuilder<BodyComponent>
    {
        #region Internal Fields

        internal readonly List<BodyFixture> Fixtures = new List<BodyFixture>();
        internal float CofFactor;
        internal float CorFactor;

        #endregion Internal Fields

        #region Private Fields

        private readonly IShapeMan shapeMan;
        private readonly ICollisionMan<IEntity> collisionMan;

        #endregion Private Fields

        #region Internal Constructors

        internal BodyComponentBuilder(IShapeMan shapeMan, ICollisionMan<IEntity> collisionMan)
        {
            this.shapeMan = shapeMan;
            this.collisionMan = collisionMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public BodyComponent Build()
        {
            return new BodyComponent(this);
        }

        public void SetCofFactor(float cofFactor)
        {
            CofFactor = cofFactor;
        }

        public void SetCorFactor(float corFactor)
        {
            CorFactor = corFactor;
        }

        public void AddFixture(BodyFixture fixture)
        {
            Fixtures.Add(fixture);
        }

        #endregion Public Methods
    }
}
