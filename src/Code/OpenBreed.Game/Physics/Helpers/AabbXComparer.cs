using OpenBreed.Game.Physics.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Physics.Helpers
{
    public class AabbXComparer : IComparer
    {
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
    }
}
