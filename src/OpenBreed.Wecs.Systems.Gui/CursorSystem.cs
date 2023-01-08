using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Gui;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Gui
{
    [RequireEntityWith(
        typeof(CursorInputComponent),
        typeof(PositionComponent))]
    public class CursorSystem : SystemBase<CursorSystem>, IRenderable
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IViewClient viewClient;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public CursorSystem(
            IWorld world,
            IViewClient viewClient,
            IInputsMan inputsMan,
            IPrimitiveRenderer primitiveRenderer)
        {
            this.viewClient = viewClient;
            this.inputsMan = inputsMan;
            this.primitiveRenderer = primitiveRenderer;

            world.GetModule<IRenderableBatch>().Add(this);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        public void Render(Box2 viewBox, int depth, float dt)
        {
            var cursorPos4 = new Vector4(
                inputsMan.CursorPos.X,
                inputsMan.CursorPos.Y,
                0.0f,
                1.0f);

            cursorPos4 = primitiveRenderer.GetScreenToWorldCoords(cursorPos4);

            for (int i = 0; i < entities.Count; i++)
            {
                Render(entities[i], viewBox, depth, cursorPos4, dt);
            }
        }

        public void Render(IEntity entity, Box2 viewBox, int depth, Vector4 cursorPos, float dt)
        {
            var posCmp = entity.Get<PositionComponent>();
            posCmp.Value = new Vector2(cursorPos.X, cursorPos.Y);
        }

        #endregion Public Methods
    }
}