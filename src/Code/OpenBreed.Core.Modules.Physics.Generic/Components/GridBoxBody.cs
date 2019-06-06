using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Components
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
            x = (int)(position.Current.X / size);
            y = (int)(position.Current.Y / size);
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();

            Aabb = new Box2
            {
                Left = position.Current.X,
                Bottom = position.Current.Y,
                Right = position.Current.X + size,
                Top = position.Current.Y + size,
            };
        }

        #endregion Public Methods
    }
}