using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Physics.Components
{
    public class GridBoxBody : IStaticBody
    {

        #region Private Fields

        private float size;
        private Position position;

        #endregion Private Fields

        #region Public Constructors

        public GridBoxBody(float size)
        {
            this.size = size;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; private set; }

        public Type SystemType { get { return typeof(PhysicsSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void GetGridIndices(out int x, out int y)
        {
            x = (int)(position.X / size);
            y = (int)(position.Y / size);
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();

            Aabb = new Box2
            {
                Left = position.X,
                Bottom = position.Y,
                Right = position.X + size,
                Top = position.Y + size,
            };
        }

        public void Resolve(IPhysicsComponent other)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}