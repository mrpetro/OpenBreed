using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Dynamic body
    /// Borrowing calculations from amazing N tutorials implementation for now:
    /// https://www.metanetsoftware.com/technique/tutorialA.html
    /// </summary>
    public class DynamicBody : IDynamicBody
    {
        #region Private Fields

        private float DRAG = 0.0f;//0 means full drag, 1 is no drag
        private float FRICTION = 0.05f;
        private float BOUNCE = 0.0f;

        #endregion Private Fields

        //must be in [0,1], where 1 means full bounce. but 1 seems to

        #region Public Constructors

        public DynamicBody()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// DEBUG only
        /// </summary>
        public bool Collides { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public List<Tuple<int, int>> Boxes { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public Vector2 Projection { get; private set; }

        public IShapeComponent Shape { get; private set; }
        public Position Position { get; private set; }
        public Velocity Velocity { get; private set; }

        public Box2 Aabb
        {
            get
            {
                return Shape.Aabb.Translated(Position.Value);
            }
        }

        public Type SystemType { get { return typeof(PhysicsSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            Position = entity.Components.OfType<Position>().First();
            Velocity = entity.Components.OfType<Velocity>().First();
            Shape = entity.Components.OfType<IShapeComponent>().First();
        }

        public void CollideVsStatic(IStaticBody staticBody)
        {
            //if (!Aabb.CollidesWith(staticBody.Aabb))
            //    return;

            //TODO: Checking/resolving collisions between actual bodies needs to be implemented

            //NOTE: For now only Aabb boxes are being processed
            var dPos = Aabb.GetCenter();
            var dHalfWidth = Aabb.Width / 2.0f;
            var dHalfHeight = Aabb.Height / 2.0f;

            var sPos = staticBody.Aabb.GetCenter();
            var sHalfWidth = staticBody.Aabb.Width / 2.0f;
            var sHalfHeight = staticBody.Aabb.Height / 2.0f;

            var tx = sPos.X;
            var ty = sPos.Y;
            var txw = sHalfWidth;
            var tyw = sHalfHeight;

            var dx = dPos.X - tx;//tile->obj delta
            var px = (txw + dHalfWidth) - Math.Abs(dx);//penetration depth in x

            if (0 < px)
            {
                var dy = dPos.Y - ty;//tile->obj delta
                var py = (tyw + dHalfHeight) - Math.Abs(dy);//pen depth in y

                if (0 < py)
                {
                    //object may be colliding with tile; call tile-specific collision function

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

                    Collides = true;

                    Projection = new Vector2(px, py);

                    ResolveVsAABB(Projection, staticBody);
                }
            }
        }

        public void IntegrateVerlet()
        {
            var d = DRAG;
            var g = 0.0f;

            var o = Position.Value;
            var p = o + Velocity.Value;

            float px, py;

            var ox = o.X; //we can't swap buffers since mcs/sticks point directly to vector2s..
            var oy = o.Y;

            px = p.X;
            py = p.Y;

            Position.Value = new Vector2(px, py);

            //integrate
            p.X += (d * px) - (d * ox);
            p.Y += (d * py) - (d * oy) + g;
        }

        public void CollideVsDynamic(IDynamicBody dynamicBody)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private void ResolveVsAABB(Vector2 projection, IStaticBody staticBody)
        {
            var normalized = projection.Normalized();
            ReportVsStatic(Projection, normalized, staticBody);
        }

        private void ReportVsStatic(Vector2 projection, Vector2 normal, IStaticBody staticBody)
        {
            var o = Position.Value;
            var p = o + Velocity.Value;

            //find component of velocity parallel to collision normal
            var dp = Vector2.Dot(Velocity.Value, normal);
            var n = Vector2.Multiply(normal, dp);
            var t = Vector2.Subtract(Velocity.Value, n);

            //we only want to apply collision response forces if the object is travelling into, and not out of, the collision
            float b, bx, by, f, fx, fy;
            if (dp < 0)
            {
                f = FRICTION;
                fx = t.X * f;
                fy = t.Y * f;

                b = 1 + BOUNCE;//this bounce constant should be elsewhere, i.e inside the object/tile/etc..

                bx = (n.X * b);
                by = (n.Y * b);
            }
            else
            {
                //moving out of collision, do not apply forces
                bx = by = fx = fy = 0;
            }

            Position.Value += projection;
            Position.Value += new Vector2(bx, by);
            Position.Value += new Vector2(fx, fy);
        }

        #endregion Private Methods
    }
}