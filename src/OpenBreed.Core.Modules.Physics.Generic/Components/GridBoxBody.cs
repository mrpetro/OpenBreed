using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Systems;

namespace OpenBreed.Core.Modules.Physics.Components
{
    public class GridBoxBody : IStaticBody
    {
        #region Private Fields

        private float size;
        private IPosition position;

        #endregion Private Fields

        #region Public Constructors

        public GridBoxBody(float size)
        {
            this.size = size;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void GetGridIndices(out int x, out int y)
        {
            x = (int)(position.Value.X / size);
            y = (int)(position.Value.Y / size);
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<IPosition>().First();

            Aabb = new Box2
            {
                Left = position.Value.X,
                Bottom = position.Value.Y,
                Right = position.Value.X + size,
                Top = position.Value.Y + size,
            };
        }

        #endregion Public Methods
    }
}