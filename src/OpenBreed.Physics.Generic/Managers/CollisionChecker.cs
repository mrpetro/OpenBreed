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

        public bool CheckCircleVsBox(Vector2 posA, ICircleShape shapeA, Vector2 posB, IBoxShape shapeB, out Vector2 projection)
        {
            var posDiff = posB - posA;

            //ar bPos = shapeB.Center;
            //var aPos = new Vector2(shapeA.X, shapeA.Y);

            //var aPos = new Vector2(pointShapeA.X, pointShapeA.Y);
            //var bPos = new Vector2(pointShapeB.X, pointShapeB.Y);
            //projection = Vector2.Zero;
            //if (aPos == bPos)
            //    return true;

            projection = Vector2.Zero;
            return false;
        }

        public bool CheckCircleVsCircle(Vector2 posA, ICircleShape shapeA, Vector2 posB, ICircleShape shapeB, out Vector2 projection)
        {
            var posDiff = posB - posA;

            var bPos = shapeB.Center;
            //var aPos = new Vector2(shapeA.X, shapeA.Y);

            //var aPos = new Vector2(pointShapeA.X, pointShapeA.Y);
            //var bPos = new Vector2(pointShapeB.X, pointShapeB.Y);
            //projection = Vector2.Zero;
            //if (aPos == bPos)
            //    return true;

            projection = Vector2.Zero;
            return false;
        }

        public bool CheckPointVsBox(Vector2 posA, IPointShape pointShape, Vector2 posB, IBoxShape shapeB, out Vector2 projection)
        {
            //var aabbA = pointShape.GetAabb();
            var aabbB = new Box2(shapeB.X, shapeB.Y, shapeB.X + shapeB.Width, shapeB.Y + shapeB.Height);
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

        public bool CheckPointVsCircle(Vector2 posA, IPointShape shapeA, Vector2 posB, ICircleShape shapeB, out Vector2 projection)
        {
            var pointPos = posA + new Vector2(shapeA.X, shapeA.Y);
            var circlePos = posB + shapeB.Center;

            var pointPosT = pointPos - circlePos;

            var areColliding = pointPosT.Length <= shapeB.Radius / 2;

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