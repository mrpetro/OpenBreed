using OpenBreed.Physics.Interface;
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
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Gui
{
    [RequireEntityWith(
        typeof(BodyComponent),
        typeof(PositionComponent))]
    public class PhysicsDebugDisplaySystem : MatchingSystemBase<PhysicsDebugDisplaySystem>, IRenderable
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        private List<IEntity> entities = new List<IEntity>();

        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsDebugDisplaySystem(
            IWorld world,
            IPrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;

            //TODO: Replace this with component
            //broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                DrawDynamicEntityAabb(entities[i], clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void DrawDynamicEntityAabb(IEntity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();

            if (!entity.Contains<VelocityComponent>())
                return;

            var aabb = broadphaseDynamic.GetAabb(entity.Id);

            //Test viewport for clippling here
            if (aabb.Max.X < clipBox.Min.X)
                return;

            if (aabb.Min.X > clipBox.Max.X)
                return;

            if (aabb.Max.Y < clipBox.Min.Y)
                return;

            if (aabb.Min.Y > clipBox.Max.Y)
                return;

            primitiveRenderer.DrawRectangle(aabb, Color4.Green);
        }

        #endregion Private Methods
    }
}