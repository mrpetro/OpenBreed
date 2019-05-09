using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Game.Physics.Components
{
    /// <summary>
    /// Dynamic body
    /// Borrowing calculations from amazing N tutorials implementation for now:
    /// https://www.metanetsoftware.com/technique/tutorialA.html
    /// </summary>
    public class DynamicBody : IDynamicBody
    {
        private float DRAG = 0.0f;//0 means full drag, 1 is no drag
        private float FRICTION = 0.05f;
        private float BOUNCE = 0.0f;//must be in [0,1], where 1 means full bounce. but 1 seems to 

        #region Public Constructors

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

        public DynamicBody()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IShapeComponent Shape { get; private set; }
        public DynamicPosition Position { get; private set; }

        public Box2 Aabb
        {
            get
            {
                return Shape.Aabb.Translated(Position.Current);
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
            Position = entity.Components.OfType<DynamicPosition>().First();
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

        private void ResolveVsAABB(Vector2 projection, IStaticBody staticBody)
        {
            var normalized = projection.Normalized();
            ReportVsStatic(Projection, normalized, staticBody);
        }

        public void IntegrateVerlet()
        {
            var d = DRAG;
            var g = 0.0f;

            var p = Position.Current;
            var o = Position.Old;

            float px, py;

            var ox = o.X; //we can't swap buffers since mcs/sticks point directly to vector2s..
            var oy = o.Y;

            px = p.X;
            py = p.Y;

            Position.Old = new Vector2(px, py);

            //integrate	
            p.X += (d * px) - (d * ox);
            p.Y += (d * py) - (d * oy) + g;
        }

        private void ReportVsStatic(Vector2 projection, Vector2 normal, IStaticBody staticBody)
        {
            var p = Position.Current;
            var o = Position.Old;

            //calc velocity
            var vx = p.X - o.X;
            var vy = p.Y - o.Y;

            //find component of velocity parallel to collision normal
            var dp = (vx * normal.X + vy * normal.Y);
            var nx = dp * normal.X;//project velocity onto collision normal

            var ny = dp * normal.Y;//nx,ny is normal velocity

            var tx = vx - nx;//px,py is tangent velocity
            var ty = vy - ny;

            //we only want to apply collision response forces if the object is travelling into, and not out of, the collision
            float b, bx, by, f, fx, fy;
            if (dp < 0)
            {
                f = FRICTION;
                fx = tx * f;
                fy = ty * f;

                b = 1 + BOUNCE;//this bounce constant should be elsewhere, i.e inside the object/tile/etc..

                bx = (nx * b);
                by = (ny * b);

            }
            else
            {
                //moving out of collision, do not apply forces
                bx = by = fx = fy = 0;

            }


            Position.Current += projection;

            Position.Old += projection;
            Position.Old += new Vector2(bx, by);
            Position.Old += new Vector2(fx, fy);
        }

        public void CollideVsDynamic(IDynamicBody dynamicBody)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}