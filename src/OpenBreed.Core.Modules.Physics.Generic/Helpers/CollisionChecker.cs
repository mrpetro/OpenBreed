using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public static class CollisionChecker
    {
        public static bool Check(Vector2 posA, AxisAlignedBoxShape shapeA, Vector2 posB, AxisAlignedBoxShape shapeB, out Vector2 projection)
        {
            var aabbA = shapeA.Aabb;
            var aabbB = shapeB.Aabb;
            aabbB.Translate(posB - posA);

            var aPos = aabbA.GetCenter();
            var aHalfWidth = aabbA.Width / 2.0f;
            var aHalfHeight = aabbA.Height / 2.0f;

            var bPos = aabbB.GetCenter();
            var bHalfWidth = aabbB.Width / 2.0f;
            var bHalfHeight = aabbB.Height / 2.0f;

            var dx = aPos.X - bPos.X;

            //Calculate depth in x
            var px = (bHalfWidth + aHalfWidth) - Math.Abs(dx);

            if (0 < px)
            {
                var dy = aPos.Y - bPos.Y;

                //Calculate depth in y
                var py = (bHalfHeight + aHalfHeight) - Math.Abs(dy);

                if (0 < py)
                {
                    //calculate projection vectors
                    if (px < py)
                    {
                        //project in x
                        if (dx < 0)
                        {
                            //project to the left
                            px *= -1;
                            py = 0;
                        }
                        else
                        {
                            //proj to right
                            py = 0;
                        }
                    }
                    else
                    {
                        //project in y
                        if (dy < 0)
                        {
                            //project up
                            px = 0;
                            py *= -1;
                        }
                        else
                        {
                            //project down
                            px = 0;
                        }
                    }

                    projection = new Vector2(px, py);
                    return true;
                }
            }

            projection = Vector2.Zero;
            return false;
        }

        public static bool Check(Vector2 posA, IShapeComponent shapeA, Vector2 posB, IShapeComponent shapeB, out Vector2 projection)
        {
            if (shapeA is AxisAlignedBoxShape && shapeB is AxisAlignedBoxShape)
                return Check(posA, (AxisAlignedBoxShape)shapeA, posB, (AxisAlignedBoxShape)shapeB, out projection);
            else
                throw new NotImplementedException();
        }
    }
}
