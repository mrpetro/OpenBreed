using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Helpers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Sandbox.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Systems
{
    public class UiSystem : SystemBase
    {
        #region Private Fields

        private List<Entity> entities = new List<Entity>();
        private readonly IRenderModule renderingModule;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public UiSystem(ICore core, IRenderModule renderingModule, IInputsMan inputsMan)
        {
            this.renderingModule = renderingModule;
            this.inputsMan = inputsMan;

            Require<CursorInputComponent>();
            Require<PositionComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            inputsMan.MouseMove += Inputs_MouseMove;
        }

        private void Inputs_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var icc = entities[i].Get<CursorInputComponent>();

                if (icc.CursorId != 0)
                    return;

                var viewportSystem = renderingModule.ScreenWorld.Systems.OfType<ViewportSystem>().FirstOrDefault();

                var gameViewport = renderingModule.ScreenWorld.Entities.FirstOrDefault( item => object.Equals(item.Tag, "GameViewport"));

                if (gameViewport == null)
                    return;

                var pos = entities[i].Get<PositionComponent>();


                var coord = viewportSystem.ClientToWorld(new OpenTK.Vector4(e.X, e.Y, 0.0f, 1.0f), gameViewport);
                pos.Value = new OpenTK.Vector2(coord.X, coord.Y);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}