using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using System;

namespace OpenBreed.Game.Physics.Components
{
    public class StaticBoxBody : IPhysicsComponent
    {
        #region Public Constructors

        public StaticBoxBody(ITransformComponent transform)
        {
            Transform = transform;
        }

        #endregion Public Constructors

        #region Public Properties

        public Type SystemType { get { return typeof(PhysicsSystem); } }
        public ITransformComponent Transform { get; }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            //throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}