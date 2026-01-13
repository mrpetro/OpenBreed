using OpenBreed.Physics.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems.Abstractions
{
    public interface IOnEntityCollisionSystem : ISystem
    {
        #region Public Properties

        int ColliderTypeA { get; }
        IEnumerable<int> ColliderTypesB { get; }

        #endregion Public Properties

        #region Public Methods

        void OnCollision(IFixture fixtureA, IEntity entityA, IFixture fixtureB, IEntity entityB, float dt, Vector2 projection);

        #endregion Public Methods
    }
}