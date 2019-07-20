using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenTK;
using System;

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
        private const float GENERIC_COR = 1.0f;

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

        internal static void ResolveVsDynamic(DynamicPack dynamicBody, DynamicPack staticBody, Vector2 projection, float dt)
        {
            dynamicBody.Entity.DebugData = new object[] { "COLLISION_PAIR", dynamicBody.Aabb.GetCenter(), staticBody.Aabb.GetCenter() };
            staticBody.Entity.DebugData = new object[] { "COLLISION_PAIR", dynamicBody.Aabb.GetCenter(), staticBody.Aabb.GetCenter() };

            //TODO: To have proper solver for two moving boxes
        }

        internal static void ResolveVsStatic(DynamicPack dynamicBody, StaticPack staticBody, Vector2 projection, float dt)
        {
            dynamicBody.Position.Value += projection;

            var normal = projection.Normalized();

            //find component of velocity parallel to collision normal
            var dp = Vector2.Dot(dynamicBody.Velocity.Value, normal);

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dp < 0)
            {
                var cor = GENERIC_COR * dynamicBody.Body.CorFactor;

                var vn = Vector2.Multiply(normal, dp);
                var vt = dynamicBody.Velocity.Value - vn;

                dynamicBody.Velocity.Value = vt - cor * vn;
            }
        }

        #endregion Internal Methods
    }
}