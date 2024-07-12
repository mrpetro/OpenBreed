using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Generic.Managers
{
    public class CollisionChecker : ICollisionChecker
    {
        #region Public Methods

        public bool Check(Vector2 posA, IShape shapeA, Vector2 posB, IShape shapeB, out Vector2 projection)
        {
            switch (shapeA)
            {
                case IBoxShape boxShape:
                    return CheckBoxVsShape(posA, boxShape, posB, shapeB, out projection);

                case IPointShape pointShape:
                    return CheckPointVsShape(posA, pointShape, posB, shapeB, out projection);

                case ICircleShape circleShape:
                    return CheckCircleVsShape(posA, circleShape, posB, shapeB, out projection);

                default:
                    throw new NotImplementedException();
            }
        }

        public bool CheckBoxVsBox(Vector2 posA, IBoxShape shapeA, Vector2 posB, IBoxShape shapeB, out Vector2 projection)
        {
            var aabbA = shapeA.ToBox2();
            var aabbB = shapeB.ToBox2();
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

        public bool CheckCircleVsBox(Vector2 posA, ICircleShape shapeA, Vector2 posB, IBoxShape shapeB, out Vector2 projection)
        {
            projection = Vector2.Zero;
            var aabbB = shapeB.ToBox2();
            aabbB.Translate(posB);

            var cDiff = shapeA.Center + posA;

            // temporary variables to set edges for testing
            var testX = cDiff.X;
            var testY = cDiff.Y;
            var cx = cDiff.X;
            var cy = cDiff.Y;
            var rx = aabbB.Min.X;
            var ry = aabbB.Min.Y;
            var rw = aabbB.Size.X;
            var rh = aabbB.Size.Y;
            var radius = shapeA.Radius;

            if (cx < rx)
            {
                testX = rx;
            }
            else if (cx > rx + rw)
            {
                testX = rx + rw;
            }

            if (cy < ry)
            {
                testY = ry;
            }
            else if (cy > ry + rh)
            {
                testY = ry + rh;
            }


            var distX = cx - testX;
            var distY = cy - testY;
            var distance = Math.Sqrt((distX * distX) + (distY * distY));

            if (distance < radius)
            {
                return true;
            }

            return false;
        }

        public bool CheckCircleVsCircle(
            Vector2 posA,
            ICircleShape shapeA,
            Vector2 posB,
            ICircleShape shapeB,
            out Vector2 projection)
        {
            var posDelta = posB - posA;

            var centerBt = shapeB.Center + posDelta;

            var radiusSum = shapeA.Radius + shapeB.Radius;

            var distance = centerBt.Length;

            projection = Vector2.Zero;
            return radiusSum > distance;
        }

        public bool CheckPointVsBox(Vector2 posA, IPointShape shapeA, Vector2 posB, IBoxShape shapeB, out Vector2 projection)
        {
            var aabbB = shapeB.ToBox2();
            aabbB.Translate(posB - posA);

            var aPos = shapeA.ToVector2();

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

        public bool CheckPointVsCircle(Vector2 posA, IPointShape shapeA, Vector2 posB, ICircleShape shapeB, out Vector2 projection)
        {
            var pointPos = posA + new Vector2(shapeA.X, shapeA.Y);
            var circlePos = posB + shapeB.Center;

            var pointPosT = pointPos - circlePos;

            var areColliding = pointPosT.Length <= shapeB.Radius;

            projection = Vector2.Zero;

            return areColliding;
        }

        public bool CheckPointVsPoint(Vector2 posA, IPointShape pointShapeA, Vector2 posB, IPointShape pointShapeB, out Vector2 projection)
        {
            var pointPosA = posA + new Vector2(pointShapeA.X, pointShapeA.Y);
            var pointPosB = posB + new Vector2(pointShapeB.X, pointShapeB.Y);
            projection = Vector2.Zero;
            if (pointPosA == pointPosB)
                return true;

            return false;
        }

        #endregion Public Methods

        #region Private Methods

        private bool CheckBoxVsShape(Vector2 posA, IBoxShape boxShapeA, Vector2 posB, IShape shapeB, out Vector2 projection)
        {
            switch (shapeB)
            {
                case IBoxShape boxShapeB:
                    return CheckBoxVsBox(posA, boxShapeA, posB, boxShapeB, out projection);

                case IPointShape pointShapeB:
                    return CheckPointVsBox(posB, pointShapeB, posA, boxShapeA, out projection);

                case ICircleShape circleShapeB:
                    return CheckCircleVsBox(posB, circleShapeB, posA, boxShapeA, out projection);

                default:
                    throw new NotImplementedException();
            }
        }

        private bool CheckCircleVsShape(Vector2 posA, ICircleShape circleShapeA, Vector2 posB, IShape shapeB, out Vector2 projection)
        {
            switch (shapeB)
            {
                case IBoxShape boxShapeB:
                    return CheckCircleVsBox(posA, circleShapeA, posB, boxShapeB, out projection);

                case IPointShape pointShapeB:
                    return CheckPointVsCircle(posB, pointShapeB, posA, circleShapeA, out projection);

                case ICircleShape circleShapeB:
                    return CheckCircleVsCircle(posB, circleShapeB, posA, circleShapeA, out projection);

                default:
                    throw new NotImplementedException();
            }
        }

        private bool CheckPointVsShape(Vector2 posA, IPointShape pointShapeA, Vector2 posB, IShape shapeB, out Vector2 projection)
        {
            switch (shapeB)
            {
                case IBoxShape boxShapeB:
                    return CheckPointVsBox(posA, pointShapeA, posB, boxShapeB, out projection);

                case IPointShape pointShapeB:
                    return CheckPointVsPoint(posB, pointShapeB, posA, pointShapeA, out projection);

                case ICircleShape circleShapeB:
                    return CheckPointVsCircle(posA, pointShapeA, posB, circleShapeB, out projection);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Private Methods
    }
}