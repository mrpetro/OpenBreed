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
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OpenBreed.Wecs.Systems.Gui
{
    public class CollisionVisualizingOptions
    {
        public bool Enabled { get; set; } = true;
    }

    public class CollisionVisualizingSystem : SystemBase<CollisionVisualizingSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly CollisionVisualizingOptions visualizingOptions;
        private readonly IEntityMan entityMan;

        private readonly Dictionary<int, Color4> groupsToColors = new Dictionary<int, Color4>();
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public CollisionVisualizingSystem(
            IEntityMan entityMan,
            IPrimitiveRenderer primitiveRenderer,
            ICollisionMan<IEntity> collisionMan,
            CollisionVisualizingOptions collisionVisualizingOptions)
        {
            this.entityMan = entityMan;
            this.primitiveRenderer = primitiveRenderer;
            this.collisionMan = collisionMan;
            this.visualizingOptions = collisionVisualizingOptions;

            SetupColors();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Worlds.IWorldRenderContext context)
        {
            if (!visualizingOptions.Enabled)
            {
                return;
            }

            context.View.EnableAlpha();

            try
            {
                var mapEntity = entityMan.GetByTag("Maps").Where(e => e.WorldId == context.World.Id).FirstOrDefault();

                if (mapEntity is null)
                    return;

                var collisionComponent = mapEntity.Get<CollisionComponent>();

                DrawDynamics(context, collisionComponent.Broadphase, context.ViewBox);
                DrawStatics(context, collisionComponent.Broadphase, context.ViewBox);

                if (collisionComponent.Result.Contacts.Any())
                {
                    DrawContacts(context, collisionComponent.Result.Contacts);
                }
            }
            finally
            {
                context.View.DisableAlpha();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawContacts(Worlds.IWorldRenderContext context, List<CollisionContact> contacts)
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                DrawContact(context, contacts[i]);
            }
        }

        private void DrawContact(Worlds.IWorldRenderContext context, CollisionContact contact)
        {
            var view = context.View;

            var entityA = entityMan.GetById(contact.ItemIdA);
            var posA = entityA.Get<PositionComponent>().Value;

            var entityB = entityMan.GetById(contact.ItemIdB);
            var posB = entityB.Get<PositionComponent>().Value;


            view.PushMatrix();
            view.Translate(new Vector3(posA));

            RenderShape(view, contact.FixtureA.Shape, new Color4(255,0,0,80));

            view.PopMatrix();
            view.PushMatrix();

            view.Translate(new Vector3(posB));

            RenderShape(view, contact.FixtureB.Shape, new Color4(255, 0, 0, 80));

            view.PopMatrix();
        }

        private void DrawDynamicEntityAabb(Worlds.IWorldRenderContext context, IBroadphaseItem item, Box2 clipBox)
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

            DrawEntityFixtures(context, entity);

            primitiveRenderer.DrawRectangle(context.View, aabb, Color4.Green);

            //primitiveRenderer.DrawRectangle(new Box2(0,0, 100, 100), Color4.Red, filled: false);
        }

        private void DrawDynamics(Worlds.IWorldRenderContext context, IBroadphase dynamics, Box2 viewBox)
        {
            foreach (var item in dynamics.DynamicItems)
            {
                DrawDynamicEntityAabb(context, item, viewBox);
            }
        }

        private void DrawEntityFixtures(Worlds.IWorldRenderContext context, IEntity entity)
        {
            var view = context.View;

            var posCmp = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();

            view.PushMatrix();
            view.Translate(new Vector3(posCmp.Value));

            for (int i = 0; i < bodyCmp.Fixtures.Count; i++)
            {
                var fixture = bodyCmp.Fixtures[i];
                var color = groupsToColors[fixture.GroupIds.FirstOrDefault()];
                RenderShape(view, fixture.Shape, color);
            }

            view.PopMatrix();
        }

        private void DrawStatics(Worlds.IWorldRenderContext context, IBroadphase statics, Box2 viewBox)
        {
            var itemIds = statics.QueryStatic(viewBox);

            foreach (var itemId in itemIds)
            {
                var entity = entityMan.GetById(itemId);

                DrawEntityFixtures(context, entity);
            }
        }

        private void RenderShape(Rendering.Interface.Managers.IRenderView view, IShape shape, Color4 color)
        {
            switch (shape)
            {
                case IPolygonShape polygon:
                    throw new NotImplementedException("Polygon shape not implemented yet.");
                    break;

                case IBoxShape box:
                    primitiveRenderer.DrawRectangle(view, new Box2(box.X, box.Y, box.X + box.Width, box.Y + box.Height), color, filled: true);
                    break;

                case ICircleShape circle:
                    primitiveRenderer.DrawCircle(view, circle.Center, circle.Radius, color, filled: true);
                    break;

                case IPointShape point:
                    primitiveRenderer.DrawPoint(view, new Vector2(point.X, point.Y), color, PointType.Circle);
                    break;

                default:
                    break;
            }
        }

        private void SetupColors()
        {
            byte alpha = 50;

            foreach (var groupName in collisionMan.GroupNames)
            {
                var result = groupName switch
                {
                    "ActorBody" => new Color4(0, 127, 127, alpha),
                    "ActorSight" => new Color4(127, 127, 0, alpha),
                    "ActorTrigger" => new Color4(0, 127, 0, alpha),
                    "DoorOpenTrigger" => new Color4(0, 127, 0, alpha),
                    "Projectile" => new Color4(127, 0, 0, alpha),
                    "FullObstacle" => new Color4(127, 127, 127, alpha),
                    "SlopeObstacle" => new Color4(127, 127, 127, alpha),
                    "ActorOnlyObstacle" => new Color4(127, 127, 127, alpha),
                    "SlowdownObstacle" => new Color4(127, 127, 127, alpha),
                    "WorldExitTrigger" => new Color4(0, 127, 0, alpha),
                    "TeleportEntryTrigger" => new Color4(0, 127, 0, alpha),
                    "Trigger" => new Color4(0, 127, 0, alpha),
                    _ => new Color4(127, 127, 127, alpha)
                };

                var groupId = collisionMan.GetGroupId(groupName);

                groupsToColors.Add(groupId, result);
            }
        }

        #endregion Private Methods
    }
}