using OpenBreed.Game.Common;
using OpenBreed.Game.Entities.Components;
using System;

namespace OpenBreed.Game.Physics.Components
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

        public void Deinitialize(IWorldSystem system)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IWorldSystem system)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}