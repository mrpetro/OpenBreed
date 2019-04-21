using OpenBreed.Game.Entities.Components;
using OpenTK;
using System;

namespace OpenBreed.Game.Common
{
    public class Transformation : ITransformComponent
    {
        #region Public Fields

        public Matrix4 Value;

        #endregion Public Fields

        #region Public Constructors

        public Transformation(float x, float y)
        {
            Value = Matrix4.CreateTranslation(x, y, 0.0f);
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 Matrix { get { return Value; } }

        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IComponentSystem system)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IComponentSystem system)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}