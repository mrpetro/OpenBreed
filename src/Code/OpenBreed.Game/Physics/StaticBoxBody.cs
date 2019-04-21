using OpenBreed.Game.Common;
using OpenBreed.Game.Entities.Components;
using System;

namespace OpenBreed.Game.Physics
{
    public class StaticBoxBody : IBodyComponent
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

        public void Deinitialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}