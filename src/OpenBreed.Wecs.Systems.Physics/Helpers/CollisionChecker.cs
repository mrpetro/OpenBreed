using OpenBreed.Core.Managers;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Helpers
{
    public static class CollisionChecker
    {
        public static bool Check(Vector2 posA, BoxShape shapeA, Vector2 posB, BoxShape shapeB, out Vector2 projection)
        {
            var aabbA = shapeA.GetAabb();
            var aabbB = shapeB.GetAabb();
            aabbB.Translate(posB - posA);

            var aPos = aabbA.GetCenter();
            var aHalfWidth = aabbA.Size.X / 2.0f;
            var aHalfHeight = aabbA.Size.Y / 2.0f;

            var bPos = aabbB.GetCenter();
            var bHalfWidth = aabbB.Size.X / 2.0f;
            var bHalfHeight = aabbB.Size.Y / 2.0f;

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

        public static bool Check(Vector2 posA, PointShape pointShape, Vector2 posB, BoxShape shapeB, out Vector2 projection)
        {
            //var aabbA = pointShape.GetAabb();
            var aabbB = shapeB.GetAabb();
            aabbB.Translate(posB - posA);

            //var aPos = new Vector2(pointShape.X, pointShape.Y);
            var aPos = new Vector2(pointShape.X, pointShape.Y);
            //var aHalfWidth = aabbA.Width / 2.0f;
            //var aHalfHeight = aabbA.Height / 2.0f;

            var bPos = aabbB.GetCenter();
            var bHalfWidth = aabbB.Size.X / 2.0f;
            var bHalfHeight = aabbB.Size.Y / 2.0f;

            var dx = aPos.X - bPos.X;

            //Calculate depth in x
            var px = (bHalfWidth) - Math.Abs(dx);

            if (0 < px)
            {
                var dy = aPos.Y - bPos.Y;

                //Calculate depth in y
                var py = (bHalfHeight) - Math.Abs(dy);

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

        public static bool Check(Vector2 posA, PointShape pointShapeA, Vector2 posB, PointShape pointShapeB, out Vector2 projection)
        {
            var aPos = new Vector2(pointShapeA.X, pointShapeA.Y);
            var bPos = new Vector2(pointShapeB.X, pointShapeB.Y);
            projection = Vector2.Zero;
            if (aPos == bPos)
                return true;

            return false;
        }

        public static bool Check(Vector2 posA, IShape shapeA, Vector2 posB, IShape shapeB, out Vector2 projection)
        {
            if (shapeA is BoxShape && shapeB is BoxShape)
                return Check(posA, (BoxShape)shapeA, posB, (BoxShape)shapeB, out projection);
            else if (shapeA is PointShape && shapeB is BoxShape)
                return Check(posA, (PointShape)shapeA, posB, (BoxShape)shapeB, out projection);
            else if (shapeA is PointShape && shapeB is PointShape)
                return Check(posA, (PointShape)shapeA, posB, (PointShape)shapeB, out projection);
            else if (shapeA is BoxShape && shapeB is PointShape)
                return Check(posB, (PointShape)shapeB, posA, (BoxShape)shapeA, out projection);
            else
                throw new NotImplementedException();
        }

    }
}
