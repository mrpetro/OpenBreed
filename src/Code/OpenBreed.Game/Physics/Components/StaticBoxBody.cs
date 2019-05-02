using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using System;
using System.Linq;

namespace OpenBreed.Game.Physics.Components
{
    public class StaticBoxBody : IPhysicsComponent
    {
        #region Private Fields

        private int size = 16;
        private Transformation transformation;

        #endregion Private Fields

        #region Public Constructors

        public StaticBoxBody()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Type SystemType { get { return typeof(PhysicsSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void GetMapIndices(out int x, out int y)
        {
            var pos = transformation.Value.ExtractTranslation();
            x = (int)pos.X / size;
            y = (int)pos.Y / size;
        }

        public void Initialize(IEntity entity)
        {
            transformation = entity.Components.OfType<Transformation>().First();
        }

        #endregion Public Methods
    }
}