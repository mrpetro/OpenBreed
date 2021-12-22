using OpenBreed.Physics.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Gui
{
    public class PhysicsDebugDisplaySystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        private List<Entity> entities = new List<Entity>();

        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsDebugDisplaySystem(IPrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();

            world.GetModule<IRenderableBatch>().Add(this);
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                DrawDynamicEntityAabb(entities[i], clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void DrawDynamicEntityAabb(Entity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();

            if (!entity.Contains<VelocityComponent>())
                return;

            var aabb = broadphaseDynamic.GetAabb(entity.Id);

            //Test viewport for clippling here
            if (aabb.Right < clipBox.Left)
                return;

            if (aabb.Left > clipBox.Right)
                return;

            if (aabb.Top < clipBox.Bottom)
                return;

            if (aabb.Bottom > clipBox.Top)
                return;

            primitiveRenderer.DrawRectangle(aabb, Color4.Green);
        }

        #endregion Private Methods
    }
}