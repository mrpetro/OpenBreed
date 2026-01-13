using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Gui.Components;
using OpenBreed.Wecs.Gui.Systems.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Gui.Systems
{
    [RequireEntityWith(
        typeof(CursorInputComponent),
        typeof(PositionComponent))]
    public class CursorSystem : IMatchingSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;
        private readonly IWindow viewClient;

        #endregion Private Fields

        #region Public Constructors

        public CursorSystem(
            IWindow viewClient,
            IInputsMan inputsMan,
            IEventsMan eventsMan)
        {
            this.viewClient = viewClient;
            this.inputsMan = inputsMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IWorldRenderContext context)
        {
            var entities = context.World.GetMatchingEntities(this);

            var cursorPos4 = context.View.FromHostToWorldPoint((Vector2i)inputsMan.CursorPos);

            foreach (var entity in entities)
            {
                Update(entity, context.ViewBox, context.Depth, cursorPos4, context.Dt);
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

            if(inputsMan.IsMousePressed)
            {
                RaiseCursorKeyPressedEvent(entity, 0);

                //if(viewClient.MouseState.IsButtonPressed()
            }

        }

        #endregion Public Methods

        #region Private Methods

        private void RaiseCursorMovedEvent(IEntity entity)
        {
            eventsMan.Raise(new CursorMovedEntityEvent(entity.Id));
        }

        private void RaiseCursorKeyPressedEvent(IEntity entity, int keyId)
        {
            eventsMan.Raise(new CursorKeyPressedEntityEvent(entity.Id, keyId));
        }

        #endregion Private Methods
    }
}