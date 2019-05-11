using OpenBreed.Core.Systems.Physics.Components;
using System.Collections;

namespace OpenBreed.Core.Systems.Physics.Helpers
{
    public class AabbXComparer : IComparer
    {
        #region Public Methods

        int IComparer.Compare(object a, object b)
        {
            var aabb1 = (IDynamicBody)a;
            var aabb2 = (IDynamicBody)b;
            if (aabb1.Aabb.Left < aabb2.Aabb.Left)
                return -1;
            if (aabb1.Aabb.Left == aabb2.Aabb.Left)
                return 0;
            else
                return 1;
        }

        #endregion Public Methods
    }
}