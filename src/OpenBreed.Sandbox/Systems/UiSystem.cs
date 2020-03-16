using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
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
    public class UiSystem : WorldSystem, ICommandExecutor
    {
        #region Private Fields

        private CommandHandler cmdHandler;

        private List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public UiSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);

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
                var icc = entities[i].GetComponent<CursorInputComponent>();

                if (icc.CursorId != 0)
                    return;

                var viewportSystem = Core.Rendering.ScreenWorld.Systems.OfType<ViewportSystem>().FirstOrDefault();

                var gameViewport = Core.Rendering.ScreenWorld.Entities.FirstOrDefault( item => object.Equals(item.Tag, "GameViewport"));

                if (gameViewport == null)
                    return;

                var coord = viewportSystem.ClientToWorld(new OpenTK.Vector4(e.X, e.Y, 0.0f, 1.0f), gameViewport);

                Console.WriteLine($"{coord.X}, {coord.Y}");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}