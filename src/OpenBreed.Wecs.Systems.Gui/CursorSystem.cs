using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Gui;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Gui.Events;
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
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public CursorSystem(
            IWorld world,
            IViewClient viewClient,
            IInputsMan inputsMan,
            IPrimitiveRenderer primitiveRenderer,
            IEventsMan eventsMan)
        {
            this.viewClient = viewClient;
            this.inputsMan = inputsMan;
            this.primitiveRenderer = primitiveRenderer;
            this.eventsMan = eventsMan;

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
                Update(entities[i], viewBox, depth, cursorPos4, dt);
            }
        }

        public void Update(IEntity entity, Box2 viewBox, int depth, Vector4 cursorPos, float dt)
        {
            var posCmp = entity.Get<PositionComponent>();
            var cursorCmp = entity.Get<CursorInputComponent>();
            var lastPos = posCmp.Value;
            posCmp.Value = new Vector2(cursorPos.X, cursorPos.Y);
            var delta = posCmp.Value - lastPos;

            if (delta.X != 0.0f || delta.Y != 0.0f)
                RaiseCursorMovedEvent(entity);

            if(viewClient.MouseState.IsAnyButtonDown)
            {



                RaiseCursorKeyPressedEvent(entity, 0);

                //if(viewClient.MouseState.IsButtonPressed()
            }

        }

        #endregion Public Methods

        #region Private Methods

        private void RaiseCursorMovedEvent(IEntity entity)
        {
            eventsMan.Raise(entity, new CursorMovedEntityEvent(entity.Id));
        }

        private void RaiseCursorKeyPressedEvent(IEntity entity, int keyId)
        {
            eventsMan.Raise(entity, new CursorKeyPressedEntityEvent(entity.Id, keyId));
        }

        #endregion Private Methods
    }
}