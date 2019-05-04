using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Game.Physics.Components
{
    public class DynamicBody : IDynamicBody
    {
        #region Public Constructors

        /// <summary>
        /// DEBUG only
        /// </summary>
        public bool Collides { get; set; }

        public List<Tuple<int, int>> Boxes { get; set; }

        public DynamicBody()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IShapeComponent Shape { get; private set; }
        public Position Position { get; private set; }

        public Box2 Aabb
        {
            get
            {
                return Shape.Aabb.Translated(new Vector2(Position.X, Position.Y));
            }
        }

        public Type SystemType { get { return typeof(PhysicsSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            Position = entity.Components.OfType<Position>().First();
            Shape = entity.Components.OfType<IShapeComponent>().First();
        }

        public void Resolve(IPhysicsComponent other)
        {
            Collides = true;
        }

        #endregion Public Methods
    }
}