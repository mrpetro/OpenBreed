using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Components;
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
    /// Tech Demo Class: Viewports & Cameras
    /// </summary>
    public class StateTechDemo1 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_1";

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

        private static byte[] mapB = new byte[] {
            3,3,0,0,0,3,3,3,3,3,
            0,0,0,0,0,0,0,0,1,3,
            7,0,0,0,0,0,0,0,1,3,
            0,0,0,1,0,1,0,0,4,3,
            0,0,0,2,2,2,0,0,2,3,
            0,3,3,0,0,0,3,3,3,3
        };

        private Viewport viewportA;
        private Viewport viewportB;
        private Viewport viewportC;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo1(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public IEntity Camera1 { get; private set; }

        public IEntity Camera2 { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        private Viewport hoverViewport = null;

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (viewportA.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportA;
            else if (viewportB.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportB;
            else if (viewportC.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportC;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Zoom = Tools.GetZoom(Core, hoverViewport.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                    hoverViewport.ScrollBy(Core.Inputs.CursorDelta);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            InitializeWorld();

            Console.Clear();
            Console.WriteLine("---------- Viewports & Cameras --------");
            Console.WriteLine("This demo shows three viewports with two cameras attached to them.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
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
            Core.Rendering.Viewports.Remove(viewportA);
            Core.Rendering.Viewports.Remove(viewportB);
            Core.Rendering.Viewports.Remove(viewportC);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
            Core.Inputs.MouseMove -= Inputs_MouseMove;
            Core.Players.LooseAllControls();
        }

        private void InitializeWorld()
        {
            World = GameWorldHelper.CreateGameWorld(Core, "DEMO1");

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);

            Camera1 = cameraBuilder.Build();

            World.AddEntity(Camera1);

            cameraBuilder.SetPosition(new Vector2(64, 288));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = cameraBuilder.Build();
            World.AddEntity(Camera2);

            viewportA = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.45f, 0.45f);
            viewportA.Clipping = true;
            viewportA.DrawBorder = true;
            viewportA.CameraEntity = Camera1;

            viewportB = (Viewport)Core.Rendering.Viewports.Create(0.55f, 0.05f, 0.40f, 0.45f);
            viewportB.Clipping = true;
            viewportB.DrawBorder = true;
            viewportB.CameraEntity = Camera2;

            viewportC = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.55f, 0.90f, 0.40f);
            viewportC.Clipping = true;
            viewportC.DrawBorder = true;
            viewportC.CameraEntity = Camera2;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new WalkingControl());

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);

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
                        blockBuilder.SetPosition(new Vector2(x * 16 + 5 * 16, y * 16 + 5 * 16));
                        blockBuilder.SetTileId(v);
                        World.AddEntity(blockBuilder.Build());
                    }
                }
            }

            Core.Inputs.KeyDown += Inputs_KeyDown;
            Core.Inputs.MouseMove += Inputs_MouseMove;

            Core.Rendering.Viewports.Add(viewportA);
            Core.Rendering.Viewports.Add(viewportB);
            Core.Rendering.Viewports.Add(viewportC);
        }

        private void Inputs_MouseMove(object sender, MouseMoveEventArgs e)
        {
            //Console.WriteLine($"ScreenPos: {CoreTools.ToConsole(Core.Inputs.CursorPos)}");

            if (hoverViewport != null)
            {
                Console.WriteLine($"ViewportPos: {CoreTools.ToConsole(hoverViewport.FromClientPoint(Core.Inputs.CursorPos))}");
            }
        }

        #endregion Private Methods
    }
}