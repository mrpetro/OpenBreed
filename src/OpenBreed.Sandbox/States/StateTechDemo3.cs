using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.States
{
    /// <summary>
    /// Tech Demo Class: Pathfinding
    /// </summary>
    public class StateTechDemo3 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_3";

        public World World;

        #endregion Public Fields

        #region Private Fields

        private static byte[] mapA = new byte[] {
            3,3,3,3,3,3,3,3,3,3,
            0,0,0,0,0,0,0,0,1,3,
            0,0,0,0,0,0,0,0,1,3,
            3,0,0,1,0,1,0,0,4,3,
            3,0,0,2,2,2,0,0,2,3,
            3,3,3,3,3,3,3,3,3,3
        };

        private IEntity actor;
        private Viewport viewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo3(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public IEntity Camera1 { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewport.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewport;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Zoom = Tools.GetZoom(Core, hoverViewport.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                    hoverViewport.ScrollBy(Core.Inputs.CursorDelta);

                if (mouseState.IsButtonDown(MouseButton.Left))
                {
                    var worldCoords = hoverViewport.ClientToWorldPoint(Core.Inputs.CursorPos);
                    //var moveToCommand = new MoveToCommand(actor, worldCoords);
                    //moveToCommand.Execute();
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            InitializeWorld();

            Console.Clear();
            Console.WriteLine("---------- Pathfinding --------");
            Console.WriteLine("This demo shows actor pathfinding function (NOT IMPLEMENTED YET)");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("LMB = Set next point destination for Actor");
        }

        protected override void OnLeave()
        {
            DeinitializeWorld();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Inputs_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();

            if (pressedKey.StartsWith("Number"))
            {
                pressedKey = pressedKey.Replace("Number", "");
                Core.StateMachine.SetNextState($"TECH_DEMO_{pressedKey}");
            }
        }

        private void DeinitializeWorld()
        {
            World.RemoveAllEntities();
            Core.Worlds.Remove(World);
            Core.Rendering.Viewports.Remove(viewport);
            Core.Inputs.KeyDown -= Inputs_KeyDown;
            Core.Players.LooseAllControls();
        }

        private void InitializeWorld()
        {
            World = GameWorldHelper.CreateGameWorld(Core, "DEMO3");

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = cameraBuilder.Build();
            World.AddEntity(Camera1);

            viewport = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.9f, 0.9f);
            viewport.CameraEntity = Camera1;

            actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new AiControl());

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            rotateFsm.SetInitialState("Idle");

            World.AddEntity(actor);

            SandBoxHelper.SetupMap(World);

            var rnd = new Random();
            var ymax = mapA.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetPosition(new Vector2((x + 5) * 16, (y + 5) * 16));
                        blockBuilder.SetTileId(v);
                        World.AddEntity(blockBuilder.Build());
                    }
                }
            }

            Core.Inputs.KeyDown += Inputs_KeyDown;
            Core.Rendering.Viewports.Add(viewport);
        }

        #endregion Private Methods
    }
}