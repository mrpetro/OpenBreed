using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Managers
{
    public interface ICollisionChecker
    {
        #region Public Methods

        bool Check(Vector2 posA, IShape shapeA, Vector2 posB, IShape shapeB, out Vector2 projection);

        #endregion Public Methods
    }
}