using OpenBreed.Physics.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawDynamicEntityAabb(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        //private void Inputs_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        //{
        //    for (int i = 0; i < entities.Count; i++)
        //    {
        //        var icc = entities[i].Get<CursorInputComponent>();

        //        if (icc.CursorId != 0)
        //            return;

        //        var viewportSystem = renderingModule.ScreenWorld.Systems.OfType<ViewportSystem>().FirstOrDefault();

        //        var gameViewport = renderingModule.ScreenWorld.Entities.FirstOrDefault( item => object.Equals(item.Tag, "GameViewport"));

        //        if (gameViewport == null)
        //            return;

        //        var pos = entities[i].Get<PositionComponent>();

        //        var coord = viewportSystem.ClientToWorld(new OpenTK.Vector4(e.X, e.Y, 0.0f, 1.0f), gameViewport);
        //        pos.Value = new OpenTK.Vector2(coord.X, coord.Y);
        //    }
        //}
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

            // Draw black box
            GL.Color4(Color4.Green);
            primitiveRenderer.DrawRectangle(aabb);
        }

        #endregion Private Methods
    }
}