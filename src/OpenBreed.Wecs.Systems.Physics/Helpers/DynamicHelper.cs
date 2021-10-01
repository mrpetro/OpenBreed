using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics.Helpers
{
    public class DynamicHelper
    {
        private readonly DynamicBodiesCollisionCheckSystem system;
        private readonly IEntityMan entityMan;

        internal DynamicHelper(DynamicBodiesCollisionCheckSystem system, IEntityMan entityMan)
        {
            this.system = system;
            this.entityMan = entityMan;
        }

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

        internal bool TestVsDynamic(Entity entityA, Entity entityB, float dt, out Vector2 projection)
        {
            var bodyA = entityA.Get<BodyComponent>();
            var posA = entityA.Get<PositionComponent>();
            var bodyB = entityB.Get<BodyComponent>();
            var posB = entityB.Get<PositionComponent>();

            if (bodyA.Fixtures.Count > 0)
            {
                var fixtureA = system.GetFixture(bodyA.Fixtures.First());

                if (bodyB.Fixtures.Count > 0)
                {
                    var fixtureB = system.GetFixture(bodyB.Fixtures.First());
                    return CollisionChecker.Check(posA.Value, fixtureA, posB.Value, fixtureB, out projection);
                }
                else
                    throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }

        internal bool TestVsStatic(Entity dynamicEntity, Entity staticEntity, float dt, out Vector2 projection)
        {
            var bodyA = dynamicEntity.Get<BodyComponent>();
            var posA = dynamicEntity.Get<PositionComponent>();

            if (bodyA.Fixtures.Count > 0)
            {
                var fixtureA = system.GetFixture(bodyA.Fixtures.First());

                var bodyB = staticEntity.Get<BodyComponent>();
                var posB = staticEntity.Get<PositionComponent>();

                if (bodyB.Fixtures.Count > 0)
                {
                    var fixtureB = system.GetFixture(bodyB.Fixtures.First());
                    return CollisionChecker.Check(posA.Value, fixtureA, posB.Value, fixtureB, out projection);
                }
                else
                    throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Resolve collision between two dynamic bodies. Simplified pseudo physics version.
        /// </summary>
        /// <param name="bodyA">First dynamic body</param>
        /// <param name="bodyB">Second dynamic body</param>
        /// <param name="projection">Given collision projection vector</param>
        /// <param name="dt">Delta time</param>
        internal static void ResolveVsDynamic(Entity entityA, Entity entityB, Vector2 projection, float dt)
        {
            var bodyA = entityA.Get<BodyComponent>();
            var posA = entityA.Get<PositionComponent>();
            var velA = entityA.Get<VelocityComponent>();
            var bodyB = entityB.Get<BodyComponent>();
            var posB = entityB.Get<PositionComponent>();
            var velB = entityB.Get<VelocityComponent>();

            //entityA.DebugData = new object[] { "COLLISION_PAIR", bodyA.Aabb.GetCenter(), bodyB.Aabb.GetCenter() };
            //entityB.DebugData = new object[] { "COLLISION_PAIR", bodyA.Aabb.GetCenter(), bodyB.Aabb.GetCenter() };

            posA.Value += projection / 2;
            posB.Value -= projection / 2;

            var normalA = projection.Normalized();
            var normalB = -projection.Normalized();

            //find component of velocity parallel to collision normal
            var dpA = Vector2.Dot(velA.Value, normalA);
            var dpB = Vector2.Dot(velB.Value, normalB);

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dpA < 0)
            {
                var corA = GENERIC_COR * bodyA.CorFactor;
                var vnA = Vector2.Multiply(normalA, dpA);
                var vtA = velA.Value - vnA;
                velA.Value = vtA - corA * vnA;
            }

            //Apply collision response forces if the object is travelling into, and not out of, the collision
            if (dpB < 0)
            {
                var corB = GENERIC_COR * bodyB.CorFactor;
                var vnB = Vector2.Multiply(normalB, dpB);
                var vtB = velB.Value - vnB;
                velB.Value = vtB - corB * vnB;
            }
        }

        /// <summary>
        /// Resolve collision of moving body vs static body. Simplified pseudo physics version.
        /// </summary>
        /// <param name="entityA">Given dynamic body</param>
        /// <param name="entityB">Given static body</param>
        /// <param name="projection">Given collision projection vector</param>
        public static void ResolveVsStatic(Entity entityA, Entity entityB, Vector2 projection)
        {
            var p = entityA.TryGet<PositionComponent>();
            var v = entityA.TryGet<VelocityComponent>();
            var body = entityA.TryGet<BodyComponent>();

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