using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public static class DynamicHelper
    {
        #region Private Fields

        /// <summary>
        /// Generic Coefficient of restitution between Dynamic and Static Body
        /// Values:
        ///     0 - perfectly inelastic collision
        ///     1 - perfectly elastic collision
        /// </summary>
        public const float GENERIC_COR = 1.0f;

        #endregion Private Fields

        #region Internal Methods

        internal static bool TestVsDynamic(PhysicsSystem system, DynamicPack bodyA, DynamicPack bodyB, float dt, out Vector2 projection)
        {
            return CollisionChecker.Check(bodyA.Position.Value, bodyA.Shape, bodyB.Position.Value, bodyB.Shape, out projection);
        }

        internal static bool TestVsStatic(PhysicsSystem system, DynamicPack bodyA, StaticPack bodyB, float dt, out Vector2 projection)
        {
            return CollisionChecker.Check(bodyA.Position.Value, bodyA.Shape, bodyB.Position.Value, bodyB.Shape, out projection);
        }

        /// <summary>
        /// Resolve collision between two dynamic bodies. Simplified pseudo physics version.
        /// </summary>
        /// <param name="bodyA">First dynamic body</param>
        /// <param name="bodyB">Second dynamic body</param>
        /// <param name="projection">Given collision projection vector</param>
        /// <param name="dt">Delta time</param>
        internal static void ResolveVsDynamic(DynamicPack bodyA, DynamicPack bodyB, Vector2 projection, float dt)
        {
            bodyA.Entity.DebugData = new object[] { "COLLISION_PAIR", bodyA.Aabb.GetCenter(), bodyB.Aabb.GetCenter() };
            bodyB.Entity.DebugData = new object[] { "COLLISION_PAIR", bodyA.Aabb.GetCenter(), bodyB.Aabb.GetCenter() };

            bodyA.Position.Value += projection / 2;
            bodyB.Position.Value -= projection / 2;

            var normalA = projection.Normalized();
            var normalB = -projection.Normalized();

            //find component of velocity parallel to collision normal
            var dpA = Vector2.Dot(bodyA.Velocity.Value, normalA);
            var dpB = Vector2.Dot(bodyB.Velocity.Value, normalB);

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dpA < 0)
            {
                var corA = GENERIC_COR * bodyA.Body.CorFactor;
                var vnA = Vector2.Multiply(normalA, dpA);
                var vtA = bodyA.Velocity.Value - vnA;
                bodyA.Velocity.Value = vtA - corA * vnA;
            }

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dpB < 0)
            {
                var corB = GENERIC_COR * bodyB.Body.CorFactor;
                var vnB = Vector2.Multiply(normalB, dpB);
                var vtB = bodyB.Velocity.Value - vnB;
                bodyB.Velocity.Value = vtB - corB * vnB;
            }
        }

        /// <summary>
        /// Resolve collision of moving body vs static body. Simplified pseudo physics version.
        /// </summary>
        /// <param name="entityA">Given dynamic body</param>
        /// <param name="entityB">Given static body</param>
        /// <param name="projection">Given collision projection vector</param>
        public static void ResolveVsStatic(IEntity entityA, IEntity entityB, Vector2 projection)
        {
            var p = entityA.Components.OfType<Position>().FirstOrDefault();
            var v = entityA.Components.OfType<Velocity>().FirstOrDefault();
            var body = entityA.Components.OfType<IBody>().FirstOrDefault();

            p.Value += projection;

            var normal = projection.Normalized();

            //find component of velocity parallel to collision normal
            var dp = Vector2.Dot(v.Value, normal);

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dp < 0)
            {
                var cor = DynamicHelper.GENERIC_COR * body.CorFactor;

                var vn = Vector2.Multiply(normal, dp);
                var vt = v.Value - vn;

                v.Value = vt - cor * vn;
            }
        }

        #endregion Internal Methods
    }
}