using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Sandbox.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Systems
{
    public class UiSystem : WorldSystem
    {
        #region Private Fields

        private List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public UiSystem(ICore core) : base(core)
        {
            Require<CursorInputComponent>();
            Require<PositionComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            Core.Inputs.MouseMove += Inputs_MouseMove;
        }

        private void Inputs_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var icc = entities[i].Get<CursorInputComponent>();

                if (icc.CursorId != 0)
                    return;

                var viewportSystem = Core.Rendering.ScreenWorld.Systems.OfType<ViewportSystem>().FirstOrDefault();

                var gameViewport = Core.Rendering.ScreenWorld.Entities.FirstOrDefault( item => object.Equals(item.Tag, "GameViewport"));

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