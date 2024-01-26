using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace OpenBreed.Wecs.Systems.Gui
{
    public class PhysicsDebugDisplaySystem : SystemBase<PhysicsDebugDisplaySystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly IEntityMan entityMan;

        private readonly Dictionary<int, Color4> groupsToColors = new Dictionary<int, Color4>();
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsDebugDisplaySystem(
            IEntityMan entityMan,
            IPrimitiveRenderer primitiveRenderer,
            ICollisionMan<IEntity> collisionMan)
        {
            this.entityMan = entityMan;
            this.primitiveRenderer = primitiveRenderer;
            this.collisionMan = collisionMan;

            SetupColors();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderContext context)
        {
            primitiveRenderer.EnableAlpha();

            try
            {
                var mapEntity = entityMan.GetByTag("Maps").Where(e => e.WorldId == context.World.Id).FirstOrDefault();

                if (mapEntity is null)
                    return;

                var dynamics = mapEntity.Get<BroadphaseDynamicComponent>().Dynamic;

                DrawDynamics(dynamics, context.ViewBox);

                var statics = mapEntity.Get<BroadphaseStaticComponent>().Grid;

                DrawStatics(statics, context.ViewBox);
            }
            finally
            {
                primitiveRenderer.DisableAlpha();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawDynamicEntityAabb(IBroadphaseDynamicElement item, Box2 clipBox)
        {
            var entity = entityMan.GetById(item.ItemId);

            var aabb = item.Aabb;

            //Test viewport for clippling here
            if (aabb.Max.X < clipBox.Min.X)
                return;

            if (aabb.Min.X > clipBox.Max.X)
                return;

            if (aabb.Max.Y < clipBox.Min.Y)
                return;

            if (aabb.Min.Y > clipBox.Max.Y)
                return;

            DrawEntityFixtures(entity);

            primitiveRenderer.DrawRectangle(aabb, Color4.Green);

            //primitiveRenderer.DrawRectangle(new Box2(0,0, 100, 100), Color4.Red, filled: false);
        }

        private void DrawDynamics(IBroadphaseDynamic dynamics, Box2 viewBox)
        {
            foreach (var item in dynamics.Items)
            {
                DrawDynamicEntityAabb(item, viewBox);
            }
        }

        private void DrawEntityFixtures(IEntity entity)
        {
            var posCmp = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();

            primitiveRenderer.PushMatrix();
            primitiveRenderer.Translate(new Vector3(posCmp.Value));

            for (int i = 0; i < bodyCmp.Fixtures.Count; i++)
            {
                var fixture = bodyCmp.Fixtures[i];
                RenderShape(fixture.Shape, fixture.GroupIds.FirstOrDefault());
            }

            primitiveRenderer.PopMatrix();
        }

        private void DrawStatics(IBroadphaseStatic statics, Box2 viewBox)
        {
            var itemIds = statics.QueryStatic(viewBox);

            foreach (var itemId in itemIds)
            {
                var entity = entityMan.GetById(itemId);

                DrawEntityFixtures(entity);
            }
        }

        private void RenderShape(IShape shape, int groupId)
        {
            var color = groupsToColors[groupId];


            switch (shape)
            {
                case IPolygonShape polygon:
                    throw new NotImplementedException("Polygon shape not implemented yet.");
                    break;

                case IBoxShape box:
                    primitiveRenderer.DrawRectangle(new Box2(box.X, box.Y, box.X + box.Width, box.Y + box.Height), color, filled: true);
                    break;

                case ICircleShape circle:
                    primitiveRenderer.DrawCircle(circle.Center, circle.Radius, color, filled: true);
                    break;

                case IPointShape point:
                    primitiveRenderer.DrawPoint(new Vector2(point.X, point.Y), color, PointType.Circle);
                    break;

                default:
                    break;
            }
        }

        private void SetupColors()
        {
            foreach (var groupName in collisionMan.GroupNames)
            {
                var result = groupName switch
                {
                    "ActorBody" => new Color4(0, 255, 255, 50),
                    "EnemyBody" => new Color4(255, 0, 0, 50),
                    "EnemyVisibityRange" => new Color4(255, 255, 0, 50),
                    "ActorTrigger" => new Color4(0, 255, 0, 50),
                    "DoorOpenTrigger" => new Color4(0, 255, 0, 50),
                    "Projectile" => new Color4(255, 0, 0, 50),
                    "FullObstacle" => new Color4(255, 255, 255, 50),
                    "SlopeObstacle" => new Color4(255, 255, 255, 50),
                    "ActorOnlyObstacle" => new Color4(255, 255, 255, 50),
                    "SlowdownObstacle" => new Color4(255, 255, 255, 50),
                    "WorldExitTrigger" => new Color4(0, 255, 0, 50),
                    "TeleportEntryTrigger" => new Color4(0, 255, 0, 50),
                    "Trigger" => new Color4(0, 255, 0, 50),
                    _ => new Color4(255, 255, 255, 50)
                };

                var groupId = collisionMan.GetGroupId(groupName);

                groupsToColors.Add(groupId, result);
            }
        }

        #endregion Private Methods
    }
}