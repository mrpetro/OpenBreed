using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Components.Shapes;
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
        public static bool Check(Vector2 posA, BoxShape shapeA, Vector2 posB, BoxShape shapeB, out Vector2 projection)
        {
            var aabbA = shapeA.GetAabb();
            var aabbB = shapeB.GetAabb();
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

        public static bool Check(Vector2 posA, Fixture fixtureA, Vector2 posB, Fixture fixtureB, out Vector2 projection)
        {
            if (fixtureA.Shape is BoxShape && fixtureB.Shape is BoxShape)
                return Check(posA, (BoxShape)fixtureA.Shape, posB, (BoxShape)fixtureB.Shape, out projection);
            else
                throw new NotImplementedException();
        }
    }
}
