using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public static class DynamicHelper
    {
        /// <summary>
        /// Generic Coefficient of restitution between Dynamic and Static Body
        /// Values:
        ///     0 - perfectly inelastic collision
        ///     1 - perfectly elastic collision
        /// </summary>
        private const float GENERIC_COR = 0.0f;


        #region Internal Methods

        internal static void ResolveVsAABB(IDynamicBody dynamicBody, IPosition position, IVelocity velocity, IStaticBody staticBody, float dt)
        {
            ResolveVsGridCell(dynamicBody, position, velocity, dynamicBody.Projection, staticBody, dt);
        }

        /// <summary>
        /// Check/Report collision with dynamic body
        /// </summary>
        /// <param name="dynamicBody">Dynamic body to check and report collision</param>
        internal static void CollideVsDynamic(IDynamicBody dynamicBodyA, IDynamicBody dynamicBodyB)
        {
        }

        internal static void CollideDynamicVsStatic(IDynamicBody dynamicBody, IPosition position, IVelocity velocity, Box2 dynamicAabb, IStaticBody staticBody, float dt)
        {
            //if (!Aabb.CollidesWith(staticBody.Aabb))
            //    return;

            //TODO: Checking/resolving collisions between actual bodies needs to be implemented

            //NOTE: For now only Aabb boxes are being processed
            var dPos = dynamicAabb.GetCenter();
            var dHalfWidth = dynamicAabb.Width / 2.0f;
            var dHalfHeight = dynamicAabb.Height / 2.0f;

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

                    dynamicBody.Collides = true;

                    dynamicBody.Projection = new Vector2(px, py);

                    DynamicHelper.ResolveVsAABB(dynamicBody, position, velocity, staticBody, dt);
                }
            }
        }

        internal static void ResolveVsGridCell(IDynamicBody dynamicBody, IPosition position, IVelocity velocity, Vector2 projection, IStaticBody staticBody, float dt)
        {

            dynamicBody.OldPosition = position.Value;
            position.Value += dynamicBody.Projection;

            var normal = projection.Normalized();

            //find component of velocity parallel to collision normal
            var dp = Vector2.Dot(velocity.Value, normal);

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dp < 0)
            {
                var cor = GENERIC_COR;

                var vn = Vector2.Multiply(normal, dp);
                var vt = velocity.Value - vn;

                velocity.Value = vt - cor * vn;
            }

            //dynamicBody.OldPosition = position.Value;
            //velocity.Value = Vector2.Zero;
        }

        #endregion Internal Methods
    }
}