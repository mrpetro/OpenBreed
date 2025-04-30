using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Extensions
{
    public static class Box2Extensions
    {
        public static Box2 AsBox2(this Box2i box)
        {
            return new Box2(box.Min, box.Max);
        }
    }
}
