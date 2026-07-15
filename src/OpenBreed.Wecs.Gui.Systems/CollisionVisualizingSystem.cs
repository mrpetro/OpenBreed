using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
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

namespace OpenBreed.Wecs.Gui.Systems
{
    public class CollisionVisualizingOptions
    {
        public bool Enabled { get; set; } = true;
    }

    [RequireEntityWith(typeof(CollisionComponent))]
    public class CollisionVisualizingSystem : IRenderableSystem
    {
        #region Private Fields

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly CollisionVisualizingOptions visualizingOptions;
        private readonly IEntityMan entityMan;

        private readonly Dictionary<int, Color4<Rgba>> groupsToColors = new Dictionary<int, Color4<Rgba>>();

        #endregion Private Fields

        #region Public Constructors

        public CollisionVisualizingSystem(
            IEntityMan entityMan,
            ICollisionMan<IEntity> collisionMan,
            CollisionVisualizingOptions collisionVisualizingOptions)
        {
            this.entityMan = entityMan;
            this.collisionMan = collisionMan;
            this.visualizingOptions = collisionVisualizingOptions;

            SetupColors();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            if (!visualizingOptions.Enabled)
            {
                return;
            }

            context.View.EnableAlpha();

            try
            {
                foreach (var entity in entities)
                {
                    DrawEntity(entity, context);
                }
            }
            finally
            {
                context.View.DisableAlpha();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawEntity(IEntity entity, IWorldRenderContext context)
        {
            var collisionComponent = entity.Get<CollisionComponent>();

            DrawDynamics(context, collisionComponent.Broadphase, context.ViewBox);
            DrawStatics(context, collisionComponent.Broadphase, context.ViewBox);

            if (collisionComponent.Result.Contacts.Any())
            {
                DrawContacts(context, collisionComponent.Result.Contacts);
            }
        }

        private void DrawContacts(IWorldRenderContext context, List<CollisionContact> contacts)
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                DrawContact(context, contacts[i]);
            }
        }

        private void DrawContact(IWorldRenderContext context, CollisionContact contact)
        {
            var view = context.View;

            var entityA = entityMan.GetById(contact.ItemIdA);
            var posA = entityA.Get<PositionComponent>().Value;

            var entityB = entityMan.GetById(contact.ItemIdB);
            var posB = entityB.Get<PositionComponent>().Value;


            view.PushMatrix();
            view.Translate(new Vector3(posA));

            RenderShape(view, contact.FixtureA.Shape, new Color4<Rgba>(255,0,0,80));

            view.PopMatrix();
            view.PushMatrix();

            view.Translate(new Vector3(posB));

            RenderShape(view, contact.FixtureB.Shape, new Color4<Rgba>(255, 0, 0, 80));

            view.PopMatrix();
        }

        private void DrawDynamicEntityAabb(IWorldRenderContext context, IBroadphaseItem item, Box2 clipBox)
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

            context.View.Context.Primitives.DrawRectangle(context.View, aabb, Color4.Green);

            //primitiveRenderer.DrawRectangle(new Box2(0,0, 100, 100), Color4.Red, filled: false);
        }

        private void DrawDynamics(IWorldRenderContext context, IBroadphase dynamics, Box2 viewBox)
        {
            foreach (var item in dynamics.DynamicItems)
            {
                DrawDynamicEntityAabb(context, item, viewBox);
            }
        }

        private void DrawEntityFixtures(IWorldRenderContext context, IEntity entity)
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

        private void DrawStatics(IWorldRenderContext context, IBroadphase statics, Box2 viewBox)
        {
            var itemIds = statics.QueryStatic(viewBox);

            foreach (var itemId in itemIds)
            {
                var entity = entityMan.GetById(itemId);

                DrawEntityFixtures(context, entity);
            }
        }

        private void RenderShape(Rendering.Abstractions.IRenderView view, IShape shape, Color4<Rgba> color)
        {
            switch (shape)
            {
                case IPolygonShape polygon:
                    throw new NotImplementedException("Polygon shape not implemented yet.");
                    break;

                case IBoxShape box:
                    view.Context.Primitives.DrawRectangle(view, new Box2(box.X, box.Y, box.X + box.Width, box.Y + box.Height), color, filled: true);
                    break;

                case ICircleShape circle:
                    view.Context.Primitives.DrawCircle(view, circle.Center, circle.Radius, color, filled: true);
                    break;

                case IPointShape point:
                    view.Context.Primitives.DrawPoint(view, new Vector2(point.X, point.Y), color, PointType.Circle);
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
                    "ActorBody" => new Color4<Rgba>(0, 127, 127, alpha),
                    "ActorSight" => new Color4<Rgba>(127, 127, 0, alpha),
                    "ActorTrigger" => new Color4<Rgba>(0, 127, 0, alpha),
                    "DoorOpenTrigger" => new Color4<Rgba>(0, 127, 0, alpha),
                    "Projectile" => new Color4<Rgba>(127, 0, 0, alpha),
                    "FullObstacle" => new Color4<Rgba>(127, 127, 127, alpha),
                    "SlopeObstacle" => new Color4<Rgba>(127, 127, 127, alpha),
                    "ActorOnlyObstacle" => new Color4<Rgba>(127, 127, 127, alpha),
                    "SlowdownObstacle" => new Color4<Rgba>(127, 127, 127, alpha),
                    "WorldExitTrigger" => new Color4<Rgba>(0, 127, 0, alpha),
                    "TeleportEntryTrigger" => new Color4<Rgba>(0, 127, 0, alpha),
                    "Trigger" => new Color4<Rgba>(0, 127, 0, alpha),
                    _ => new Color4<Rgba>(127, 127, 127, alpha)
                };

                var groupId = collisionMan.GetGroupId(groupName);

                groupsToColors.Add(groupId, result);
            }
        }

        #endregion Private Methods
    }
}