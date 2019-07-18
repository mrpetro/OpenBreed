using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Physics.Components
{
    public class GridBoxBody : IStaticBody
    {
        #region Private Fields

        private float size;

        #endregion Private Fields

        #region Public Constructors

        public GridBoxBody(float size)
        {
            this.size = size;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}