﻿using OpenBreed.Core;
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
    public class CursorSystem : MatchingSystemBase<CursorSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public CursorSystem(
            IViewClient viewClient,
            IInputsMan inputsMan,
            IPrimitiveRenderer primitiveRenderer,
            IEventsMan eventsMan)
        {
            this.viewClient = viewClient;
            this.inputsMan = inputsMan;
            this.primitiveRenderer = primitiveRenderer;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderContext context)
        {
            var cursorPos4 = new Vector4(
                inputsMan.CursorPos.X,
                inputsMan.CursorPos.Y,
                0.0f,
                1.0f);

            cursorPos4 = primitiveRenderer.GetScreenToWorldCoords(cursorPos4);

            for (int i = 0; i < entities.Count; i++)
            {
                Update(entities[i], context.ViewBox, context.Depth, cursorPos4, context.Dt);
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